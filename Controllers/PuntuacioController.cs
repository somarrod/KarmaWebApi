using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;

namespace KarmaWebAPI.Controllers
{
    [Route("api/puntuacio")]
    [ApiController]
    public class PuntuacioController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public PuntuacioController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Puntuacio
        [HttpGet("llista")]
        public async Task<ActionResult<IEnumerable<Puntuacio>>> Llista()
        {
            return await _context.Puntuacio.ToListAsync();
        }

        // GET: api/Puntuacio/5
        [HttpGet("{idPuntuacio}")]
        public async Task<ActionResult<Puntuacio>> Instancia(int idPuntuacio)
        {
            var puntuacio = await _context.Puntuacio.FindAsync(idPuntuacio);

            if (puntuacio == null)
            {
                return NotFound();
            }

            return puntuacio;
        }

        // PUT: api/Puntuacio/editar
        [HttpPut("editar")]
        public async Task<IActionResult> Editar(int idPuntuacio, Puntuacio puntuacio)
        {
            if (idPuntuacio != puntuacio.IdPuntuacio)
            {
                return BadRequest();
            }

            _context.Entry(puntuacio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuntuacioExisteix(idPuntuacio))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Puntuacio/crear
        [HttpPost("crear")]
        public async Task<ActionResult<Puntuacio>> Crear(Puntuacio puntuacio)
        {
            _context.Puntuacio.Add(puntuacio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Instancia", new { id = puntuacio.IdPuntuacio }, puntuacio);
        }

        // DELETE: api/Puntuacio/eliminar
        [HttpDelete("eliminar")]
        public async Task<IActionResult> Eliminar(int idPuntuacio)
        {
            var puntuacio = await _context.Puntuacio.FindAsync(idPuntuacio);
            if (puntuacio == null)
            {
                return NotFound();
            }

            _context.Puntuacio.Remove(puntuacio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PuntuacioExisteix(int id)
        {
            return _context.Puntuacio.Any(e => e.IdPuntuacio == id);
        }
    }
}
