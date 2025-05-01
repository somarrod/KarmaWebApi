using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;

namespace KarmaWebAPI.Controllers
{
    [Route("api/materia")]
    [ApiController]
    public class MateriaController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public MateriaController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Materia/5
        [HttpGet("{idMateria}")]
        public async Task<ActionResult<Materia>> ObtenerMateria(int idMateria)
        {
            var materia = await _context.Materia.FindAsync(idMateria);

            if (materia == null)
            {
                return NotFound();
            }

            return materia;
        }

        // GET: api/Materia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Materia>>> ObtenerMaterias()
        {
            return await _context.Materia.ToListAsync();
        }


        // POST: api/Materia
        [HttpPost]
        public async Task<ActionResult<Materia>> CrearMateria(MateriaCrearDTO materiaDto)
        {
            var materia = new Materia
            {
                Nom = materiaDto.Nom,
                Activa = true
            };

            _context.Materia.Add(materia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("CrearMateria", new { id = materia.IdMateria }, materia);
        }


        // PUT: api/Materia/5
        [HttpPut("editar")]
        public async Task<IActionResult> EditarMateria(MateriaEditarDTO materiaDto)
        {

            var materia = new Materia
            {
                IdMateria = materiaDto.IdMateria,
                Nom = materiaDto.Nom,
                Activa = materiaDto.Activa
            };

            _context.Entry(materia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MateriaExiste(materiaDto.IdMateria))
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


        // DELETE: api/Materia/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarMateria(int idMateria)
        {
            var materia = await _context.Materia.FindAsync(idMateria);
            if (materia == null)
            {
                return NotFound();
            }

            _context.Materia.Remove(materia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MateriaExiste(int idMateria)
        {
            return _context.Materia.Any(e => e.IdMateria == idMateria);
        }
    }
}
