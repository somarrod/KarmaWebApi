using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;

namespace KarmaWebAPI.Controllers
{
    [Route("api/privilegiassignat")]
    [ApiController]
    public class PrivilegiAssignatController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public PrivilegiAssignatController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/PrivilegiAssignat
        [HttpGet("llista")]
        public async Task<ActionResult<IEnumerable<PrivilegiAssignat>>> Llista()
        {
            return await _context.PrivilegiAssignat.ToListAsync();
        }

        // GET: api/PrivilegiAssignat/5
        [HttpGet("{idPrivilegiAssignat}")]
        public async Task<ActionResult<PrivilegiAssignat>> Instancia(int idPrivilegiAssignat)
        {
            var privilegiAssignat = await _context.PrivilegiAssignat.FindAsync(idPrivilegiAssignat);

            if (privilegiAssignat == null)
            {
                return NotFound();
            }

            return privilegiAssignat;
        }

        // PUT: api/PrivilegiAssignat/editar
        [HttpPut("editar")]
        public async Task<IActionResult> Editar(int idPrivilegiAssignat, PrivilegiAssignat privilegiAssignat)
        {
            if (idPrivilegiAssignat != privilegiAssignat.IdPrivilegiAssignat)
            {
                return BadRequest();
            }

            _context.Entry(privilegiAssignat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrivilegiAssignatExisteix(idPrivilegiAssignat))
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

        // POST: api/PrivilegiAssignat/crear
        [HttpPost("crear")]
        public async Task<ActionResult<PrivilegiAssignat>> Crear(PrivilegiAssignat privilegiAssignat)
        {
            _context.PrivilegiAssignat.Add(privilegiAssignat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Instancia", new { id = privilegiAssignat.IdPrivilegiAssignat }, privilegiAssignat);
        }

        // DELETE: api/PrivilegiAssignat/eliminar
        [HttpDelete("eliminar")]
        public async Task<IActionResult> Eliminar(int idPrivilegiAssignat)
        {
            var privilegiAssignat = await _context.PrivilegiAssignat.FindAsync(idPrivilegiAssignat);
            if (privilegiAssignat == null)
            {
                return NotFound();
            }

            _context.PrivilegiAssignat.Remove(privilegiAssignat);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrivilegiAssignatExisteix(int id)
        {
            return _context.PrivilegiAssignat.Any(e => e.IdPrivilegiAssignat == id);
        }
    }
}
