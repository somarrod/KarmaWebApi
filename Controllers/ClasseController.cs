using KarmaWebAPI.DTOs;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/classe")]
     public class ClasseController : ControllerBase
    {
        private readonly IClasseService _service;

        public ClasseController(IClasseService service)
        {
            _service = service;
        }

        [Authorize(Roles = "AG_Admin,AG_Professor,AG_Alumne")]
        [HttpGet("per-anyescolar/{idAnyEscolar:int}")]
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

        [Authorize(Roles = "AG_Admin,AG_Professor,AG_Alumne")]
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

        [Authorize(Roles = "AG_Admin,AG_Professor")]
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

        [Authorize(Roles = "AG_Admin,AG_Professor")]
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

        [Authorize(Roles = "AG_Admin")]
        [HttpDelete("eliminar/{idClasse:long}")]
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

        [Authorize(Roles = "AG_Admin, AG_Professor")]
        [HttpPost("assignar-alumnes")]
        public async Task<IActionResult> AssignarAlumnes(
                    [FromBody] AssignarAlumnesAClasseDTO dto)
        {
            try
            {
                await _service.AssignarAlumnesAsync(dto.IdClasse, dto.NIAs);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("desassignar-alumnes")]
        public async Task<IActionResult> DesassignarAlumnes(
            [FromBody] DesassignarAlumnesDeClasseDTO dto)
        {
            try
            {
                await _service.DesassignarAlumnesAsync(dto.NIAs);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }

}
