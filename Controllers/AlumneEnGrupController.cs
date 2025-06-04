using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using KarmaWebAPI.Data;
using KarmaWebAPI.Models;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.DisplaySets;
using KarmaWebAPI.Serveis;
using KarmaWebAPI.Serveis.Interfaces;
using System.Net.NetworkInformation;

namespace KarmaWebAPI.Controllers
{
    [Route("api/alumneengrup")]
    [ApiController]
    public class AlumneEnGrupController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IAlumneEnGrupService _alumneEnGrupService;
        private readonly IGrupService _grupService;

        public AlumneEnGrupController(DatabaseContext context, IAlumneEnGrupService alumneEnGrupService, IGrupService grupService)
        {
            _context = context;
            _alumneEnGrupService = alumneEnGrupService;
            _grupService = grupService;
        }

        // GET: api/AlumneEnGrup/5
        [HttpGet("{idAlumneEnGrup}")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_Alumne")]
        public async Task<ActionResult<AlumneEnGrup>> Instancia(int idAlumneEnGrup)
        {
            var alumneEnGrup = await _context.AlumneEnGrup.FindAsync(idAlumneEnGrup);

            if (alumneEnGrup == null)
            {
                return NotFound();
            }

            return alumneEnGrup;
        }

        // GET: api/AlumneEnGrup/5
        [HttpGet("instancia-per-alumne-i-grup")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_Alumne")]
        public async Task<ActionResult<AlumneEnGrup>> InstanciaPerCamps(String nia, int idAnyEscolar, String idGrup)
        {
            var alumneEnGrup = await _context.AlumneEnGrup.FirstOrDefaultAsync(a => a.NIA == nia && a.IdAnyEscolar == idAnyEscolar && a.IdGrup == idGrup);

            if (alumneEnGrup == null)
            {
                return NotFound();
            }

            return alumneEnGrup;
        }

        // GET: api/AlumneEnGrup
        [HttpGet("llista")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_Alumne")]
        public async Task<ActionResult<IEnumerable<AlumneEnGrup>>> Llista()
        {
            var AlumneEnGrupList = new List<AlumneEnGrup>();
            var userName = User.Identity.Name;

            if (User.IsInRole("AG_Admin"))
            {
                // Retornar tots els alumnes en del grup
                AlumneEnGrupList = await _context.AlumneEnGrup
                            .Include(c => c.Grup)
                            .Include(c=> c.Alumne)
                            .ToListAsync();
            }
            else
            {
                if (User.IsInRole("AG_Professor"))
                    //AG_Professor
                    //EXIST(Grup.AnyEscolar) WHERE(Grup.AnyEscolar.Actiu = true) = true AND
                    //EXIST(Grup.ProfessorsDeGrup ) WHERE(Grup.ProfessorsDeGrup.Professor = Agent.Professor) = true
                    AlumneEnGrupList = await _context.AlumneEnGrup
                            .Include(c => c.Grup)
                            .Include(c => c.Alumne)
                            .Include(c => c.AnyEscolar)
                             .Where(c => c.AnyEscolar.Actiu == true
                                            && _context.ProfessorDeGrup.Any(pg => pg.IdProfessor == userName && pg.IdGrup == c.IdGrup))
                            .ToListAsync();
                else
                {
                    AlumneEnGrupList = await _context.AlumneEnGrup
                            .Include(c => c.Grup)
                            .Include(a => a.Alumne)
                            .Include(c => c.AnyEscolar)
                            .Where(c => c.AnyEscolar.Actiu == true && c.Alumne.NIA == userName)
                            .ToListAsync();
                }
            }


            var AlumneEnGrupDTOList = AlumneEnGrupList.Select(a => new AlumneEnGrupDisplaySet
            {
                IdAlumneEnGrup = a.IdAlumneEnGrup,
                NIA = a.Alumne.NIA,
                IdAnyEscolar = a.IdAnyEscolar,
                IdGrup = a.Grup.IdGrup,
                PuntuacioTotal = a.PuntuacioTotal,
                Karma = a.Karma
            }).ToList();

            return Ok(AlumneEnGrupDTOList);
        }


        // POST: api/AlumneEnGrup
        [HttpPost("crear")]
        public async Task<ActionResult<AlumneEnGrup>> Crear(AlumneEnGrupTCREARDTO alumneEnGrupDto)
        {
            var anyEscolar = await _context.AnyEscolar.FindAsync(alumneEnGrupDto.IdAnyEscolar);
            var grup = await _context.Grup.FindAsync(alumneEnGrupDto.IdAnyEscolar, alumneEnGrupDto.IdGrup);
            var alumne = await _context.Alumne.FindAsync(alumneEnGrupDto.NIA);

            if (alumne == null)
            {
                return BadRequest("L'alumne introduit no existeix.");
            }
            if (grup == null) 
            {
                return BadRequest("El grup introduit no existeix.");
            }
            if ( anyEscolar == null)
            {
                return BadRequest("El any escolar introduit no existeix.");
            }

            // Comprovar si ja existeix un registre per al mateix alumne i grup
            var existentAlumneEnGrup = await _context.AlumneEnGrup
                                       .FirstOrDefaultAsync(a => a.NIA == alumneEnGrupDto.NIA && 
                                                            a.IdGrup == alumneEnGrupDto.IdGrup && 
                                                            a.IdAnyEscolar == alumneEnGrupDto.IdAnyEscolar);

            if (existentAlumneEnGrup != null)
            {
                return BadRequest("Ja existeix un registre per a aquest alumne en aquest grup.");
            }

            var alumneEnGrup = new AlumneEnGrup
            {
                NIA = alumneEnGrupDto.NIA,
                IdGrup = alumneEnGrupDto.IdGrup,
                IdAnyEscolar = alumneEnGrupDto.IdAnyEscolar,
                PuntuacioTotal = 0,
                Karma = "CREACIÓ"
            };
            _context.AlumneEnGrup.Add(alumneEnGrup);
            try
            {
                await _context.SaveChangesAsync();

                await _alumneEnGrupService.AfegirPuntuacioAsync(alumneEnGrup.IdAlumneEnGrup, 0);

                await _grupService.calculaKarmaBaseAsync(grup.IdAnyEscolar, grup.IdGrup);
            }
            catch (Exception e) 
            {
                return StatusCode(500, e.InnerException != null ? e.InnerException.Message : e.Message);
            }

            return Ok(alumneEnGrup);
        }


        // Replace the following method in the file:

        [HttpPut("reset-puntuacio-total")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> ResetPuntuacioTotal(int idAlumneEnGrup)
        {
            try
            {
                var result = await _alumneEnGrupService.ResetPuntuacioTotalAsync(
                    idAlumneEnGrup,
                   0
                );

                return result.Result ?? Ok(result.Value);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error intern del servidor: {ex.Message}");
            }
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Eliminar(int IdAlumneEnGrup)
        {
            var alumneEnGrup = await _context.AlumneEnGrup.FindAsync(IdAlumneEnGrup);
            var alumneEnGrupDS = new AlumneEnGrupDisplaySet
            {
                IdGrup = alumneEnGrup.IdGrup,
                IdAnyEscolar = alumneEnGrup.IdAnyEscolar,
                NIA = alumneEnGrup.NIA
            };
        

            if (alumneEnGrup == null)
            {
                return NotFound();
            }

            _context.AlumneEnGrup.Remove(alumneEnGrup);
            await _context.SaveChangesAsync();

            await _grupService.calculaKarmaBaseAsync(alumneEnGrupDS.IdAnyEscolar, alumneEnGrupDS.IdGrup);

            return Ok($"L'alumne {alumneEnGrupDS.NIA} ha segut esborrat del grup {alumneEnGrupDS.IdGrup} de l'any escolar {alumneEnGrupDS.IdAnyEscolar}");
        }

    }
}
    