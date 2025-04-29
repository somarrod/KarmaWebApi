using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace KarmaWebAPI.Controllers
{
    [Route("api/grup")]
    [ApiController]
    public class GrupController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public GrupController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Grup
        [HttpGet("llista")]
        public async Task<ActionResult<IEnumerable<Grup>>> Llista()
        {
            return await _context.Grups.ToListAsync();
        }

        // GET: api/Grup/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Grup>> Instancia(int idAnyEscolar, string idGrup)
        {
            var grup = await _context.Grups.FindAsync(idAnyEscolar, idGrup);

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

            var grup = new Grup
            {
                IdAnyEscolar = grupDTO.IdAnyEscolar,
                IdProfessorTutor = grupDTO.IdProfessorTutor,
                IdGrup = grupDTO.IdGrup,
                Descripcio = grupDTO.Descripcio
            };
            _context.Grups.Add(grup);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Instancia), new { idGrup = grup.IdGrup }, grup);
        }


        // PUT: api/Grup/5
        //[HttpPut("editar")]
        //public async Task<IActionResult> Editar(int idAnyEscolar, string idGrup, Grup grup)
        //{
        //    if (idAnyEscolar != grup.IdAnyEscolar || idGrup != grup.IdGrup)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(grup).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GrupExists(idAnyEscolar, idGrup))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}


        // DELETE: api/Grup/5
        [HttpDelete("eliminar")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Delete(int idAnyEscolar, string idGrup)
        {
            var grup = await _context.Grups.FindAsync(idAnyEscolar, idGrup);
            if (grup == null)
            {
                return NotFound();
            }

            _context.Grups.Remove(grup);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion Serveis

        private bool GrupExists(int idAnyEscolar, string idGrup)
        {
            return _context.Grups.Any(e => e.IdAnyEscolar == idAnyEscolar && e.IdGrup == idGrup);
        }
    }
}
