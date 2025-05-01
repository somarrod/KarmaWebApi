using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;

namespace KarmaWebAPI.Controllers
{
    [Route("api/alumne")]
    [ApiController]
    public class AlumneController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public AlumneController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Alumne
        [HttpGet("llista")]
        public async Task<ActionResult<IEnumerable<Alumne>>> Llista()
        {
            return await _context.Alumne.ToListAsync();
        }

        // GET: api/Alumne/5
        [HttpGet("{nia}")]
        public async Task<ActionResult<Alumne>> Instancia(string nia)
        {
            var alumne = await _context.Alumne.FindAsync(nia);

            if (alumne == null)
            {
                return NotFound();
            }

            return alumne;
        }

        // PUT: api/Alumne/editar
        [HttpPut("editar")]
        public async Task<IActionResult> Editar(string nia, Alumne alumne)
        {
            if (nia != alumne.NIA)
            {
                return BadRequest();
            }

            _context.Entry(alumne).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlumneExisteix(nia))
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

        // POST: api/Alumne/crear
        [HttpPost("crear")]
        public async Task<ActionResult<Alumne>> Crear(Alumne alumne)
        {
            _context.Alumne.Add(alumne);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Instancia", new { nia = alumne.NIA }, alumne);
        }

        // DELETE: api/Alumne/eliminar
        [HttpDelete("eliminar")]
        public async Task<IActionResult> Eliminar(string nia)
        {
            var alumne = await _context.Alumne.FindAsync(nia);
            if (alumne == null)
            {
                return NotFound();
            }

            _context.Alumne.Remove(alumne);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlumneExisteix(string nia)
        {
            return _context.Alumne.Any(e => e.NIA == nia);
        }
    }
}
