using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Serveis;

namespace KarmaWebAPI.Controllers
{
    [Route("api/alumne")]
    [ApiController]
    public class AlumneController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IAlumneService _alumneService;

        public AlumneController(DatabaseContext context, IAlumneService alumneService)
        {
            _context = context;
            _alumneService = alumneService;
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

        // GET: api/Alumne
        [HttpGet("llista")]
        public async Task<ActionResult<IEnumerable<Alumne>>> Llista()
        {
            return await _context.Alumne.ToListAsync();
        }

        // POST: api/Alumne/crear
        [HttpPost("crear")]
        public async Task<ActionResult<Alumne>> Crear(AlumneDTO alumneDto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await _alumneService.CrearAlumneAsync(alumneDto);

                    // Fix: Ensure the result is cast or converted to the correct type
                    if (result.Result is OkObjectResult okResult && okResult.Value is Alumne alumne)
                    {

                       //ací if()


                        await transaction.CommitAsync();
                        return CreatedAtAction(nameof(Instancia), new { nia = alumne.NIA }, alumne);
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return BadRequest("Failed to create Alumne.");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
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
