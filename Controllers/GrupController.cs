using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/grup")]
    public class GrupController : ControllerBase
    {
        private readonly IGrupService _grupService;

        public GrupController(IGrupService grupService)
        {
            _grupService = grupService;
        }

        // ==================================================
        // GET: api/grup/{idGrup}
        // ==================================================
        [HttpGet("{idGrup:long}")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu,AG_Alumne")]

        public async Task<IActionResult> Instancia(long idGrup)
        {
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;
            var nia = User.Identity?.Name; // o claim personalitzat

            var grup = await _grupService.InstanciaAsync(idGrup, rol!, nia);

            if (grup == null)
                return NotFound();

            return Ok(grup);
        }


        // ==================================================
        // GET: api/grup/per-classe/{idClasse}
        // ==================================================
        [HttpGet("per-classe/{idClasse:long}")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu,AG_Alumne")]
        public async Task<IActionResult> LlistaPerClasse(long idClasse)
        {
            try
            {
                var rol = User.FindFirst(ClaimTypes.Role)?.Value;
                var nia = User.Identity?.Name; // o el claim que uses per al NIA

                var grups = await _grupService.LlistaPerClasseAsync(idClasse, rol!, nia);

                return Ok(grups);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // ==================================================
        // POST: api/grup/crear
        // ==================================================
        [HttpPost("crear")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> Crear(GrupCrearDTO dto)
        {
            var grup = await _grupService.CrearAsync(dto);
            return Ok(grup);
        }

        // ==================================================
        // DELETE: api/grup/{idGrup}
        // ==================================================
        [HttpDelete("eliminar/{idGrup:long}")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> Esborrar(long idGrup)
        {
            var ok = await _grupService.EsborrarAsync(idGrup);
            if (!ok) return NotFound();
            return Ok();
        }

        // ==================================================
        // POST: api/grup/afegir-alumne
        // ==================================================
        [HttpPost("assignar-alumne")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> AssignarAlumne(
            [FromBody] AssignarAlumneAGrupDTO dto)
        {
            try
            {
                var result = await _grupService.AfegirAlumneAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        // ==================================================
        // POST: api/grup/llevar-alumne
        // ==================================================
        [HttpPost("llevar-alumne")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> LlevarAlumne([FromBody] string nia)
        {
            try
            {
                var result = await _grupService.LlevarAlumneAsync(nia);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // ==================================================
        // POST: api/grup/recalcular-karma/{idGrup}
        // ==================================================
        [HttpPost("calcular-karma-grup/{idGrup:long}")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> RecalcularKarmaBase(long idGrup)
        {
            var karma = await _grupService.CalcularKarmaBaseAsync(idGrup);
            return Ok(karma);
        }
    }
}