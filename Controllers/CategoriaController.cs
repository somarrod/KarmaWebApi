using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using KarmaWebAPI.Serveis.Interfaces;

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
        public async Task<IActionResult> Editar(CategoriaEditarDTO categoriaDto)
        {
            var categoria = new Categoria
            {
                IdCategoria = categoriaDto.IdCategoria,
                Descripcio = categoriaDto.Descripcio
            };

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExiste(categoriaDto.IdCategoria))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(categoria);
        }


        // PUT: api/Categoria/activar
        [HttpPut("activar")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> ActivarCategoria(int idCategoria)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var categoria = await _context.Categoria.FindAsync(idCategoria);

                    if (categoria != null)
                    {
                        categoria.Activa = true;

                        _context.Entry(categoria).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        await _context.Database.CommitTransactionAsync();

                        return Ok(categoria);
                    }
                    else 
                    {
                        await transaction.RollbackAsync();
                        return NotFound("La categoria indicada no existeix");
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
        public async Task<IActionResult> DesactivarCategoria(int idCategoria)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var categoria = await _context.Categoria.FindAsync(idCategoria);

                    if (categoria != null)
                    {
                        categoria.Activa = false;

                        _context.Entry(categoria).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        await _context.Database.CommitTransactionAsync();

                        return Ok(categoria);
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return NotFound("La categoria indicada no existeix");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
            }
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

            var desc = categoria.Descripcio;

            _context.Categoria.Remove(categoria);
            await _context.SaveChangesAsync();

            return Ok($"La categoria '{desc}' ha estat esborrada.");
        }

        private bool CategoriaExiste(int id)
        {
            return _context.Categoria.Any(e => e.IdCategoria == id);
        }
    }
}
