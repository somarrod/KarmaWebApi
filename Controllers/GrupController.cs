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
        // PUT: api/grup/editar
        // ==================================================
        [HttpPut("editar")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> Editar(GrupEditarDTO dto)
        {
            var grup = await _grupService.EditarAsync(dto);
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



        /// <summary>
        /// Recalcula el karma base d'un grup.
        /// </summary>
        /// <remarks>
        /// Aquest endpoint recalcula el color de karma base del grup en funció dels punts dels alumnes.
        ///
        /// --> Com funciona:
        /// - Si es proporciona <paramref name="idAvaluacio"/>, el càlcul es fa per a eixa avaluació concreta.
        /// - Si no es proporciona:
        ///     1️ es busca l'avaluació activa segons la data actual.
        ///     2️ si no n'hi ha cap, es pren l'última avaluació del curs escolar.
        ///
        /// -->  Ús habitual:
        /// - Per recalcular manualment el karma d’un grup.
        /// - Per proves amb avaluacions específiques.
        /// </remarks>
        /// <param name="idGrup">Identificador del grup</param>
        /// <param name="idAvaluacio">
        /// (Opcional) Id de l'avaluació per a la qual es vol recalcular el karma.
        /// </param>
        /// <returns>Color de karma base resultant</returns>
        [HttpPost("calcular-karma-grup")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> RecalcularKarmaBase(CalcularKarmaBaseGrupDTO dto)
        {
            var karma = await _grupService.CalcularKarmaBaseAsync(dto.IdGrup, dto.IdAvaluacio);

            return Ok(new
            {
                IdGrup = dto.IdGrup,
                IdAvaluacio = dto.IdAvaluacio,
                KarmaBase = karma
            });
        }

    }
}