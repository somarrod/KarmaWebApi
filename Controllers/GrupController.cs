using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu")]
        public async Task<IActionResult> Instancia(long idGrup)
        {
            var grup = await _grupService.InstanciaAsync(idGrup);
            if (grup == null) return NotFound();
            return Ok(grup);
        }

        // ==================================================
        // GET: api/grup/per-classe/{idClasse}
        // ==================================================
        [HttpGet("per-classe/{idClasse:long}")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu")]
        public async Task<IActionResult> LlistaPerClasse(long idClasse)
        {
            var grups = await _grupService.LlistaPerClasseAsync(idClasse);
            return Ok(grups);
        }

        // ==================================================
        // POST: api/grup/crear
        // ==================================================
        [HttpPost("crear")]
        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu")]
        public async Task<IActionResult> Crear(
            [FromQuery] long idClasse,
            [FromQuery] string nom)
        {
            var grup = await _grupService.CrearAsync(idClasse, nom);
            return Ok(grup);
        }

        // ==================================================
        // DELETE: api/grup/{idGrup}
        // ==================================================
        [HttpDelete("{idGrup:long}")]
        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu")]
        public async Task<IActionResult> Esborrar(long idGrup)
        {
            var ok = await _grupService.EsborrarAsync(idGrup);
            if (!ok) return NotFound();
            return Ok();
        }

        // ==================================================
        // PUT: api/grup/afegir-alumne
        // ==================================================
        [HttpPut("afegir-alumne")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu")]
        public async Task<IActionResult> AfegirAlumne(
            [FromQuery] long idGrup,
            [FromQuery] string nia)
        {
            var alumne = await _grupService.AfegirAlumneAsync(idGrup, nia);
            return Ok(alumne);
        }

        // ==================================================
        // PUT: api/grup/llevar-alumne
        // ==================================================
        [HttpPut("llevar-alumne")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu")]
        public async Task<IActionResult> LlevarAlumne(
            [FromQuery] string nia)
        {
            var alumne = await _grupService.LlevarAlumneAsync(nia);
            return Ok(alumne);
        }

        // ==================================================
        // POST: api/grup/recalcular-karma/{idGrup}
        // ==================================================
        [HttpPost("recalcular-karma/{idGrup:long}")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu")]
        public async Task<IActionResult> RecalcularKarma(long idGrup)
        {
            var karma = await _grupService.RecalcularKarmaBaseAsync(idGrup);
            return Ok(karma);
        }
    }
}