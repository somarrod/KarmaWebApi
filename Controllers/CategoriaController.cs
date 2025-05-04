using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace KarmaWebAPI.Controllers
{
    [Route("api/categoria")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CategoriaController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Categoria
        [HttpGet("llista")]
        public async Task<ActionResult<IEnumerable<Categoria>>> Lista()
        {
            return await _context.Categoria.ToListAsync();
        }

        // GET: api/Categoria/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> Instancia(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        // POST: api/Categoria
        [HttpPost("crear")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<ActionResult<Categoria>> Crear(CategoriaCrearDTO categoriaDTO)
        {
            var categoria = new Categoria
            {
                Descripcio = categoriaDTO.Descripcio,
                Activa = true // Asignar valor por defecto
            };

            _context.Categoria.Add(categoria);
            try
            {
                await _context.SaveChangesAsync();                
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

            return new OkObjectResult(categoria);
        }


        // PUT: api/Categoria/5
        [HttpPut("editar")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> Editar(int id, CategoriaEditarDTO categoriaDto)
        {
            if (id != categoriaDto.IdCategoria)
            {
                return BadRequest();
            }

            var categoria = new Categoria
            {
                IdCategoria = categoriaDto.IdCategoria,
                Descripcio = categoriaDto.Descripcio,
                Activa = true // Asignar valor por defecto
            };

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExiste(id))
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


        // DELETE: api/Categoria/5
        [HttpDelete("eliminar")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> Eliminar(int idCategoria)
        {
            var categoria = await _context.Categoria.FindAsync(idCategoria);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categoria.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaExiste(int id)
        {
            return _context.Categoria.Any(e => e.IdCategoria == id);
        }
    }
}
