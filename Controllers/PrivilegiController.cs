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
        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu,AG_Professor")]
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
        [HttpGet("per-any-escolar/{idAnyEscolar:long}")]
        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu,AG_Professor")]
        public async Task<IActionResult> LlistaPerAnyEscolar(long idAnyEscolar)
        {
            var llista = await _privilegiService.LlistaPerAnyEscolarAsync(idAnyEscolar);
            return Ok(llista);
        }

        // ==================================================
        // POST: api/privilegi
        // Crear privilegi
        // ==================================================
        [HttpPost]
        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu")]
        public async Task<IActionResult> Crear(
            [FromBody] PrivilegiCrearDTO privilegi)
        {
            var creat = await _privilegiService.CrearAsync(privilegi);
            return Ok(creat);
        }

        // ==================================================
        // PUT: api/privilegi
        // Editar privilegi
        // ==================================================
        [HttpPut]
        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu")]
        public async Task<IActionResult> Editar(
            [FromBody] PrivilegiEditarDTO privilegi)
        {
            var actualitzat = await _privilegiService.EditarAsync(privilegi);
            return Ok(actualitzat);
        }

        // ==================================================
        // DELETE: api/privilegi/{id}
        // Eliminar privilegi
        // ==================================================
        [HttpDelete("{idPrivilegi:long}")]
        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu")]
        public async Task<IActionResult> Eliminar(long idPrivilegi)
        {
            var ok = await _privilegiService.EliminarAsync(idPrivilegi);
            if (!ok)
                return NotFound();

            return Ok();
        }
    }
}