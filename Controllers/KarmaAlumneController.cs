using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/karmaalumne")]
    public class KarmaAlumneController : ControllerBase
    {
        private readonly IKarmaAlumneService _karmaAlumneService;

        public KarmaAlumneController(IKarmaAlumneService karmaAlumneService)
        {
            _karmaAlumneService = karmaAlumneService;
        }

        // -------------------------------------------------
        // POST: api/karmaalumne/alta-alumne
        // Alumne nou → crear des de l'avaluació en curs
        // -------------------------------------------------
        [HttpPost("alta-alumne")]
        [Authorize(Roles = "AG_Professor")]
        public async Task<IActionResult> AltaAlumne(
            [FromQuery] string nia,
            [FromQuery] int idAnyEscolar)
        {
            await _karmaAlumneService
                .CrearPerAlumneDesdeAvaluacioEnCursAsync(nia, idAnyEscolar);

            return Ok(); // ✅ No retornem objectes ací
        }

        // -------------------------------------------------
        // POST: api/karmaalumne/avaluacio
        // Crear KarmaAlumne per a una avaluació concreta
        // -------------------------------------------------
        [HttpPost("avaluacio")]
        [Authorize(Roles = "AG_Professor")]
        public async Task<IActionResult> CrearPerAvaluacio(
            [FromQuery] string nia,
            [FromQuery] long idAvaluacio,
            [FromQuery] double puntsInicials)
        {
            await _karmaAlumneService
                .CrearPerAlumneAsync(nia, idAvaluacio, puntsInicials);

            return Ok();
        }

        // -------------------------------------------------
        // GET: api/karmaalumne/per-avaluacio/{idAvaluacio}
        // -------------------------------------------------

        [HttpPost("per-classe-avaluacio")]
        [Authorize(Roles = "AG_Professor,AG_Alumne,AG_EquipDirectiu")]
        public async Task<ActionResult<IEnumerable<KarmaAlumne>>> GetPerClasseIAvaluacio(
    [                           FromBody] KarmaPerClasseIAvaluacioDTO request)
        {
            var result = await _karmaAlumneService.GetPerClasseIAvaluacioAsync(request.IdClasse, request.IdAvaluacio, User);

            return Ok(result);
        }
    }
}
