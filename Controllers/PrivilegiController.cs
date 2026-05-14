using Humanizer;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/privilegi")]
    public class PrivilegiController : ControllerBase
    {
        private readonly IPrivilegiService _privilegiService;

        public PrivilegiController(IPrivilegiService privilegiService)
        {
            _privilegiService = privilegiService;
        }

        // ==================================================
        // GET: api/privilegi/{id}
        // Consultar instància
        // ==================================================
        [HttpGet("{idPrivilegi:long}")]
        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu,AG_Professor,AG_Alumne")]
        public async Task<IActionResult> Instancia(long idPrivilegi)
        {
            var privilegi = await _privilegiService.InstanciaAsync(idPrivilegi);
            if (privilegi == null)
                return NotFound();

            return Ok(privilegi);
        }

        // ==================================================
        // GET: api/privilegi/per-any-escolar/{idAnyEscolar}
        // Consultar llista per any escolar
        // ==================================================
        [HttpGet("per-any-escolar/{idAnyEscolar}")]
        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu,AG_Professor,AG_Alumne")]
        public async Task<IActionResult> LlistaPerAnyEscolar(int idAnyEscolar)
        {
            var llista = await _privilegiService.LlistaPerAnyEscolarAsync(idAnyEscolar);
            return Ok(llista);
        }

        // ==================================================
        // POST: api/privilegi
        // Crear privilegi
        // ==================================================
        [HttpPost("crear")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> Crear(
            [FromBody] PrivilegiCrearDTO privilegi)
        {

            try
            {
                var resultat = await _privilegiService.CrearAsync(privilegi);
                return Ok(resultat);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error intern del servidor" });
            }
        }

        // ==================================================
        // PUT: api/privilegi
        // Editar privilegi
        // ==================================================
        [HttpPut("editar")]
        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu,AG_Professor")]
        public async Task<IActionResult> Editar(
            [FromBody] PrivilegiEditarDTO privilegi)
        {

           try
            {
                var resultat = await _privilegiService.EditarAsync(privilegi);
                return Ok(resultat);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error intern del servidor" });
            }
        }

        // ==================================================
        // DELETE: api/privilegi/{id}
        // Eliminar privilegi
        // ==================================================
        [HttpDelete("eliminar/{idPrivilegi:long}")]
        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu,AG_Professor")]
        public async Task<IActionResult> Eliminar(long idPrivilegi)
        {
            var ok = await _privilegiService.EliminarAsync(idPrivilegi);
            if (!ok)
                return NotFound();

            return Ok();
        }
    }
}