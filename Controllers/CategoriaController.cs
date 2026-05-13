using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Controllers
{
    namespace KarmaWebAPI.Controllers
    {
        [ApiController]
        [Route("api/categoria")]
        public class CategoriaController : ControllerBase
        {
            private readonly ICategoriaService _service;

            public CategoriaController(ICategoriaService service)
            {
                _service = service;
            }

            // ==================================================
            // GET: api/categoria/{idCategoria}
            // Instància
            // ==================================================
            [HttpGet("{idCategoria:long}")]
            [Authorize(Roles = "AG_Admin,AG_Professor,AG_Alumne")]
            public async Task<IActionResult> Instancia(long idCategoria)
            {
                var categoria = await _service.InstanciaAsync(idCategoria, User);

                if (categoria == null)
                    return NotFound("Categoria no trobada");

                return Ok(categoria);
            }

            // ==================================================
            // GET: api/categoria/llista
            // Admin -> totes
            // Professor -> només actives
            // ==================================================
            [HttpGet("llista")]
            [Authorize(Roles = "AG_Admin,AG_Professor,AG_Alumne")]
            public async Task<IActionResult> Llista()
            {
                var llista = await _service.LlistaAsync(User);
                return Ok(llista);
            }

            // ==================================================
            // POST: api/categoria/crear
            // ==================================================
            [HttpPost("crear")]
            [Authorize(Roles = "AG_Admin")]
            public async Task<IActionResult> Crear(CategoriaCrearDTO dto)
            {
                var categoria = await _service.CrearAsync(dto);
                return Ok(categoria);
            }

            // ==================================================
            // PUT: api/categoria/editar
            // ==================================================
            [HttpPut("editar")]
            [Authorize(Roles = "AG_Admin")]
            public async Task<IActionResult> Editar(CategoriaEditarDTO dto)
            {
                var categoria = await _service.EditarAsync(dto);
                return Ok(categoria);
            }

            // ==================================================
            // PUT: api/categoria/activar
            // ==================================================
            [HttpPut("activar")]
            [Authorize(Roles = "AG_Admin")]
            public async Task<IActionResult> Activar(long idCategoria)
            {
                var categoria = await _service.ActivarAsync(idCategoria);
                return Ok(categoria);
            }

            // ==================================================
            // PUT: api/categoria/desactivar
            // ==================================================
            [HttpPut("desactivar")]
            [Authorize(Roles = "AG_Admin")]
            public async Task<IActionResult> Desactivar(long idCategoria)
            {
                var categoria = await _service.DesactivarAsync(idCategoria);
                return Ok(categoria);
            }

            // ==================================================
            // DELETE: api/categoria/{idCategoria}
            // ==================================================
            [HttpDelete("{idCategoria:long}")]
            [Authorize(Roles = "AG_Admin")]
            public async Task<IActionResult> Eliminar(long idCategoria)
            {
                var ok = await _service.EliminarAsync(idCategoria);

                if (!ok)
                    return NotFound();

                return Ok();
            }
        }
    }
}