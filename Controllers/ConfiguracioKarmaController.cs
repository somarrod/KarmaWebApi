using KarmaWebAPI.DTOs;
//using KarmaWebAPI.DTOs.ConfiguracioKarma;
using KarmaWebAPI.Models;
//using KarmaWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/configuracio-karma")]
    public class ConfiguracioKarmaController : ControllerBase
    {
        private readonly IConfiguracioKarmaService _service;

        public ConfiguracioKarmaController(IConfiguracioKarmaService service)
        {
            _service = service;
        }

        // -------------------------------------------------
        // GET: api/configuracio-karma/anyescolar/2024
        // -------------------------------------------------
        [HttpGet("anyescolar/{idAnyEscolar:int}")]
        [Authorize(Roles = "AG_Professor,AG_Admin,AG_Alumne")]
        public async Task<ActionResult<IEnumerable<ConfiguracioKarma>>> GetPerAnyEscolar(
            int idAnyEscolar)
        {
            var result = await _service.GetPerAnyEscolarAsync(idAnyEscolar);
            return Ok(result);
        }

        // -------------------------------------------------
        // POST: api/configuracio-karma/crear
        // -------------------------------------------------
        [HttpPost("crear")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<ActionResult<ConfiguracioKarma>> Crear(
            ConfiguracioKarmaCrearDTO dto)
        {
            try
            {
                var result = await _service.CrearAsync(dto);
                return Ok(result); // ✅ objecte creat
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // -------------------------------------------------
        // PUT: api/configuracio-karma/editar
        // -------------------------------------------------
        [HttpPut("editar")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<ActionResult<ConfiguracioKarma>> Editar(
            ConfiguracioKarmaEditarDTO dto)
        {
            try
            {
                var result = await _service.EditarAsync(dto);

                if (result == null)
                    return NotFound();

                return Ok(result); // ✅ objecte editat
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // -------------------------------------------------
        // DELETE: api/configuracio-karma/{id}
        // -------------------------------------------------
        [HttpDelete("{id:long}")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Eliminar(long id)
        {
            var esborrat = await _service.EsborrarAsync(id);

            if (!esborrat)
                return NotFound();

            return Ok(); // ✅ OK buit
        }

        // -------------------------------------------------
        // POST: api/configuracio-karma/validar/2024
        // -------------------------------------------------
        [HttpPost("validar/{idAnyEscolar:int}")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> ValidarConfiguracio(
            int idAnyEscolar)
        {
            try
            {
                await _service.ValidarConfiguracioCompletaAsync(idAnyEscolar);
                return Ok(); // configuració correcta
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}