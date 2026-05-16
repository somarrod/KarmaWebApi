using KarmaWebAPI.DTOs.Avaluacio;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/avaluacio")]
    public class AvaluacioController : ControllerBase
    {
        private readonly IAvaluacioService _avaluacioService;

        public AvaluacioController(IAvaluacioService avaluacioService)
        {
            _avaluacioService = avaluacioService;
        }

        // -------------------------------------------------
        // GET: api/avaluacio/llista
        // -------------------------------------------------
        [HttpGet("llista")]
        [Authorize(Roles = "AG_Professor,AG_Alumne,AG_Admin")]
        public async Task<ActionResult<IEnumerable<Avaluacio>>> Llista()
        {
            try
            {
                var result = await _avaluacioService.GetLlistaAsync(
                    isAdmin: User.IsInRole("AG_Admin")
                );

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // -------------------------------------------------
        // GET: api/avaluacio/per-anyescolar/{idAnyEscolar}
        // -------------------------------------------------
        [HttpGet("per-anyescolar/{idAnyEscolar:int}")]
        [Authorize(Roles = "AG_Professor,AG_Alumne,AG_Admin")]
        public async Task<ActionResult<IEnumerable<Avaluacio>>> LlistaPerAnyEscolar(
            int idAnyEscolar)
        {
            try
            {
                var result = await _avaluacioService.GetLlistaPerAnyEscolarAsync(
                    idAnyEscolar,
                    isAdmin: User.IsInRole("AG_Admin")
                );

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // -------------------------------------------------
        // GET: api/avaluacio/{idAvaluacio}
        // -------------------------------------------------
        [HttpGet("{idAvaluacio:long}")]
        [Authorize(Roles = "AG_Professor,AG_Alumne,AG_Admin")]
        public async Task<ActionResult<Avaluacio>> Instancia(long idAvaluacio)
        {
            try
            {
                var avaluacio = await _avaluacioService.GetByIdAsync(idAvaluacio);
                return Ok(avaluacio);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // -------------------------------------------------
        // POST: api/avaluacio/crear
        // -------------------------------------------------
        [HttpPost("crear")]
        [Authorize(Roles = "AG_Professor,AG_Admin")]
        public async Task<ActionResult<Avaluacio>> Crear(AvaluacioCrearDTO dto)
        {
            try
            {
                var result = await _avaluacioService.CrearAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // -------------------------------------------------
        // PUT: api/avaluacio/editar
        // -------------------------------------------------
        [HttpPut("editar")]
        [Authorize(Roles = "AG_Professor,AG_Admin")]
        public async Task<ActionResult<Avaluacio>> Editar(AvaluacioEditarDTO dto)
        {
            try
            {
                var result = await _avaluacioService.EditarAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // -------------------------------------------------
        // POST: api/avaluacio/{id}/iniciar
        // -------------------------------------------------
        [HttpPost("{idAvaluacio:long}/iniciar")]
        [Authorize(Roles = "AG_Professor,AG_Admin")]
        public async Task<ActionResult<Avaluacio>> Iniciar(long idAvaluacio)
        {
            try
            {
                var avaluacio = await _avaluacioService.IniciarAsync(idAvaluacio);
                return Ok(avaluacio);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // -------------------------------------------------
        // POST: api/avaluacio/{id}/finalitzar
        // -------------------------------------------------
        [HttpPost("{idAvaluacio:long}/finalitzar")]
        [Authorize(Roles = "AG_Professor,AG_Admin")]
        public async Task<ActionResult<Avaluacio>> Finalitzar(long idAvaluacio)
        {
            try
            {
                var avaluacio = await _avaluacioService.FinalitzarAsync(idAvaluacio);
                return Ok(avaluacio);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // -------------------------------------------------
        // DELETE: api/avaluacio/{id}
        // -------------------------------------------------
        [HttpDelete("{idAvaluacio:long}")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Eliminar(long idAvaluacio)
        {
            try
            {
                await _avaluacioService.EsborrarAsync(idAvaluacio);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}