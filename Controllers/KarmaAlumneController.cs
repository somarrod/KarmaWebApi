using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KarmaWebAPI.Models;

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
        [Authorize(Roles = "AG_Professor,AG_Admin")]
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
        [Authorize(Roles = "AG_Professor,AG_Admin")]
        public async Task<IActionResult> CrearPerAvaluacio(
            [FromQuery] string nia,
            [FromQuery] long idAvaluacio,
            [FromQuery] double puntsInicials)
        {
            await _karmaAlumneService
                .CrearPerAlumneIAvaluacioAsync(nia, idAvaluacio, puntsInicials);

            return Ok();
        }

        // -------------------------------------------------
        // GET: api/karmaalumne/per-avaluacio/{idAvaluacio}
        // -------------------------------------------------
        [HttpGet("per-avaluacio/{idAvaluacio:long}")]
        [Authorize(Roles = "AG_Professor,AG_Admin,AG_Alumne")]
        public async Task<ActionResult<IEnumerable<KarmaAlumne>>> GetPerAvaluacio(
            long idAvaluacio)
        {
            // Aquest mètode usarà directament el context o un mètode del servei
            // si vols, després el podem encapsular també

            return Ok(); // placeholder si encara no l’exposes
        }
    }
}
