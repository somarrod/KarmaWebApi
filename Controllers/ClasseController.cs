using KarmaWebAPI.DTOs;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/classe")]
    [Authorize(Roles = "AG_Admin,AG_Professor")]
    public class ClasseController : ControllerBase
    {
        private readonly IClasseService _service;

        public ClasseController(IClasseService service)
        {
            _service = service;
        }

        [HttpGet("per-any/{idAnyEscolar:int}")]
        public async Task<IActionResult> Llista(int idAnyEscolar)
        {
            try
            {
                return Ok(await _service.LlistaAsync(idAnyEscolar));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{idClasse:long}")]
        public async Task<IActionResult> Instancia(long idClasse)
        {
            try
            {
                return Ok(await _service.InstanciaAsync(idClasse));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear(ClasseCrearDTO dto)
        {
            try
            {
                return Ok(await _service.CrearAsync(dto));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("editar")]
        public async Task<IActionResult> Editar(ClasseEditarDTO dto)
        {
            try
            {
                return Ok(await _service.EditarAsync(dto));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{idClasse:long}")]
        public async Task<IActionResult> Eliminar(long idClasse)
        {
            try
            {
                await _service.EsborrarAsync(idClasse);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
   
}
