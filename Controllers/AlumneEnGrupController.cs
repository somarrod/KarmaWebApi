using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;

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

        // GET: api/AlumneEnGrup
        [HttpGet("llista")]
        public async Task<ActionResult<IEnumerable<AlumneEnGrup>>> Llista()
        {
            return await _context.AlumneEnGrup.ToListAsync();
        }

        // GET: api/AlumneEnGrup/5
        [HttpGet("{idAlumneEnGrup}")]
        public async Task<ActionResult<AlumneEnGrup>> Instancia(int idAlumneEnGrup)
        {
            var alumneEnGrup = await _context.AlumneEnGrup.FindAsync(idAlumneEnGrup);

            if (alumneEnGrup == null)
            {
                return NotFound();
            }

            return alumneEnGrup;
        }

        // PUT: api/AlumneEnGrup/5
        [HttpPut("editar")]
        public async Task<IActionResult> Editar(int idAlumneEnGrup, AlumneEnGrup alumneEnGrup)
        {
            if (idAlumneEnGrup != alumneEnGrup.IdAlumneEnGrup)
            {
                return BadRequest();
            }

            _context.Entry(alumneEnGrup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlumneEnGrupExisteix(idAlumneEnGrup))
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

        // POST: api/AlumneEnGrup
        [HttpPost("crear")]
        public async Task<ActionResult<AlumneEnGrup>> Crear(AlumneEnGrup alumneEnGrup)
        {
            _context.AlumneEnGrup.Add(alumneEnGrup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("ObtenerAlumneEnGrup", new { id = alumneEnGrup.IdAlumneEnGrup }, alumneEnGrup);
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Eliminar(int IdAlumneEnGrup)
        {
            var alumneEnGrup = await _context.AlumneEnGrup.FindAsync(IdAlumneEnGrup);
            if (alumneEnGrup == null)
            {
                return NotFound();
            }

            _context.AlumneEnGrup.Remove(alumneEnGrup);
            await _context.SaveChangesAsync();

            return NoContent();
                }

        private bool AlumneEnGrupExisteix(int id)
        {
            return _context.AlumneEnGrup.Any(e => e.IdAlumneEnGrup == id);
        }
    }
}
    