using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;

namespace KarmaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriaController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public MateriaController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Materia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Materia>>> ObtenerMaterias()
        {
            return await _context.Materies.ToListAsync();
        }

        // GET: api/Materia/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Materia>> ObtenerMateria(int id)
        {
            var materia = await _context.Materies.FindAsync(id);

            if (materia == null)
            {
                return NotFound();
            }

            return materia;
        }

        // PUT: api/Materia/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarMateria(int id, Materia materia)
        {
            if (id != materia.IdMateria)
            {
                return BadRequest();
            }

            _context.Entry(materia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MateriaExiste(id))
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

        // POST: api/Materia
        [HttpPost]
        public async Task<ActionResult<Materia>> CrearMateria(Materia materia)
        {
            _context.Materies.Add(materia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("ObtenerMateria", new { id = materia.IdMateria }, materia);
        }

        // DELETE: api/Materia/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarMateria(int id)
        {
            var materia = await _context.Materies.FindAsync(id);
            if (materia == null)
            {
                return NotFound();
            }

            _context.Materies.Remove(materia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MateriaExiste(int id)
        {
            return _context.Materies.Any(e => e.IdMateria == id);
        }
    }
}
