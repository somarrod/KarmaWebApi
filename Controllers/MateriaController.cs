using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

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
                return NotFound("Matèria no trobada");
            }

            if (User.IsInRole("AG_Professor") && !materia.Activa) 
            {
                return NotFound("Matèria no trobada");
            }

                return materia;
        }

        // GET: api/Materia
        [HttpGet("llista")]
        [Authorize(Roles = "AG_Admin, AG_Professor")]
        public async Task<ActionResult<IEnumerable<Materia>>> Llista()
        {

            if (User.IsInRole("AG_Admin"))
            {
                // Devuelve todas las materias si el usuario tiene el rol AG_Admin
                return await _context.Materia.ToListAsync();
            }
            else 
            {
                // Devuelve solo las materias activas si el usuario tiene el rol AG_Professor
                return await _context.Materia.Where(m => m.Activa).ToListAsync();
            }
        }


        // POST: api/Materia
        [HttpPost("crear")]
        [Authorize(Roles = "AG_Admin")]
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
        [Authorize(Roles = "AG_Admin")]
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

        // PUT: api/Materia/activar
        [HttpPut("activar")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> ActivarMateria(int idMateria)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var materia = await _context.Materia.FindAsync(idMateria);

                    if (materia != null)
                    {
                        materia.Activa = true;

                        _context.Entry(materia).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        await _context.Database.CommitTransactionAsync();

                        return Ok(materia);
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return NotFound("La matèria indicada no existeix");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
            }
        }


        // PUT: api/Categoria/desactivar
        [HttpPut("desactivar")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> DesactivarMateria(int idMateria)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var materia = await _context.Materia.FindAsync(idMateria);

                    if (materia != null)
                    {
                        materia.Activa = false;

                        _context.Entry(materia).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        await _context.Database.CommitTransactionAsync();

                        return Ok(materia);
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return NotFound("La matèria indicada no existeix");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
            }
        }

        // DELETE: api/Materia/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AG_Admin")]
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
