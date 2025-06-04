using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using KarmaWebAPI.Serveis.Interfaces;

namespace KarmaWebAPI.Controllers
{
    [Route("api/grup")]
    [ApiController]
    public class GrupController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IProfessorService _professorService;   

        public GrupController(DatabaseContext context, IProfessorService professorService)
        {
            _context = context;
            _professorService = professorService;
        }

        // GET: api/Grup
        [HttpGet("llista")]
        public async Task<ActionResult<IEnumerable<Grup>>> Llista()
        {
            return await _context.Grup.ToListAsync();
        }


        // GET: api/Grup/25/ESO1A
        [HttpGet("{idAnyEscolar}/{idGrup}")]
        public async Task<ActionResult<Grup>> Instancia(int idAnyEscolar, string idGrup)
        {
            var grup = await _context.Grup.FindAsync(idAnyEscolar, idGrup);

            if (grup == null)
            {
                return NotFound();
            }

            return grup;
        }


        #region Serveis
        // POST: api/Grup/crear
        [HttpPost]
        [Route("crear")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<ActionResult<Grup>> Crear(GrupCrearDTO grupDTO)
        {
            if (grupDTO == null)
            {
                return BadRequest("El grup no pot ser null");
            }

            //pte validar que existe IdProfessorTutor
            //validar que existe IdProfessorTutor
            if (grupDTO.IdProfessorTutor != null && !(_professorService.ProfessorExisteix(grupDTO.IdProfessorTutor)))
            {
                return BadRequest("El id del professor introduit no és correcte");
            }

            var grup = new Grup
            {
                IdAnyEscolar = grupDTO.IdAnyEscolar,
                IdProfessorTutor = grupDTO.IdProfessorTutor,
                IdGrup = grupDTO.IdGrup,
                Descripcio = grupDTO.Descripcio
            };
            _context.Grup.Add(grup);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e) {
                return StatusCode(500, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
            
            return Ok(grup);
        }


        //PUT: api/Grup/5
        [HttpPut("editar")]
        public async Task<IActionResult> Editar(GrupEditarDTO grupDto)
        {
            if (grupDto == null)
            {
                return BadRequest("El grup no pot ser null");
            }

            if (grupDto.IdAnyEscolar == 0 || string.IsNullOrEmpty(grupDto.IdGrup))
            {
                return BadRequest("El grup no pot ser null");
            }

            if (!GrupExists(grupDto.IdAnyEscolar, grupDto.IdGrup))
            {
                return NotFound();
            }

            //validar que existe IdProfessorTutor
            if (grupDto.IdProfessorTutor != null && !(_professorService.ProfessorExisteix(grupDto.IdProfessorTutor))) {
                return BadRequest("El id del professor introduit no és correcte");
            }
                    

            var grup = new Grup
            {
                IdAnyEscolar = grupDto.IdAnyEscolar,
                IdGrup = grupDto.IdGrup,
                IdProfessorTutor = grupDto.IdProfessorTutor,          
                Descripcio = grupDto.Descripcio
            };

            _context.Entry(grup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrupExists(grup.IdAnyEscolar, grup.IdGrup))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(grup);
        }


        // DELETE: api/Grup/5
        [HttpDelete("eliminar")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Delete(int idAnyEscolar, string idGrup)
        {
            var grup = await _context.Grup.FindAsync(idAnyEscolar, idGrup);
            if (grup == null)
            {
                return NotFound();
            }

            _context.Grup.Remove(grup);
            await _context.SaveChangesAsync();

            return Ok($"Grup {idGrup} de l'any escolar {idAnyEscolar} ha estat esborrat");
        }
        #endregion Serveis

        private bool GrupExists(int idAnyEscolar, string idGrup)
        {
            return _context.Grup.Any(e => e.IdAnyEscolar == idAnyEscolar && e.IdGrup == idGrup);
        }
    }
}
