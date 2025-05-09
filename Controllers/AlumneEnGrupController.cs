using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using KarmaWebAPI.Data;
using KarmaWebAPI.Models;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.DisplaySets;

namespace KarmaWebAPI.Controllers
{
    [Route("api/alumneengrup")]
    [ApiController]
    public class AlumneEnGrupController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public AlumneEnGrupController(DatabaseContext context)
        {
            _context = context;
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
            await _context.SaveChangesAsync();

            return Ok(alumneEnGrup);
        }


        // PUT: api/AlumneEnGrup/editar
        [HttpPut("editar")]
        public async Task<IActionResult> Editar(AlumneEnGrupEditarDTO alumneEnGrupDto)
        {
            var alumneEnGrup = await _context.AlumneEnGrup.FindAsync(alumneEnGrupDto.IdAlumneEnGrup);
            if (alumneEnGrup == null)
            {
                return NotFound($"AlumneEnGrup amb Id {alumneEnGrupDto.IdAlumneEnGrup} no trobat");
            }

            alumneEnGrup.PuntuacioTotal = alumneEnGrupDto.PuntuacioTotal;

            _context.AlumneEnGrup.Update(alumneEnGrup);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

            return Ok(alumneEnGrup);
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

            return Ok($"L'alumne {alumneEnGrupDS.NIA} ha segut esborrat del grup {alumneEnGrupDS.IdGrup} de l'any escolar {alumneEnGrupDS.IdAnyEscolar}");
        }

        private bool AlumneEnGrupExisteix(int id)
        {
            return _context.AlumneEnGrup.Any(e => e.IdAlumneEnGrup == id);
        }
    }
}
    