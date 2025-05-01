using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;

namespace KarmaWebAPI.Controllers
{
    [Route("api/periode")]
    [ApiController]
    public class PeriodeController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public PeriodeController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Periode
        [HttpGet("llista")]
        public async Task<ActionResult<IEnumerable<Periode>>> Llista()
        {
            return await _context.Periode.ToListAsync();
        }

        // GET: api/Periode/5
        [HttpGet("{idPeriode}")]
        public async Task<ActionResult<Periode>> Instancia(int idPeriode)
        {
            var periode = await _context.Periode.FindAsync(idPeriode);

            if (periode == null)
            {
                return NotFound();
            }

            return periode;
        }

        // PUT: api/Periode/editar
        [HttpPut("editar")]
        public async Task<IActionResult> Editar(int idPeriode, Periode periode)
        {
            if (idPeriode != periode.IdPeriode)
            {
                return BadRequest();
            }

            _context.Entry(periode).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeriodeExisteix(idPeriode))
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

        // POST: api/Periode/crear
        [HttpPost("crear")]
        public async Task<ActionResult<Periode>> Crear(Periode periode)
        {
            _context.Periode.Add(periode);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Instancia", new { id = periode.IdPeriode }, periode);
        }

        // DELETE: api/Periode/eliminar
        [HttpDelete("eliminar")]
        public async Task<IActionResult> Eliminar(int idPeriode)
        {
            var periode = await _context.Periode.FindAsync(idPeriode);
            if (periode == null)
            {
                return NotFound();
            }

            _context.Periode.Remove(periode);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PeriodeExisteix(int id)
        {
            return _context.Periode.Any(e => e.IdPeriode == id);
        }
    }
}
