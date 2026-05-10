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
            var result = await _avaluacioService.GetLlistaAsync(
                isAdmin: User.IsInRole("AG_Admin")
            );

            return Ok(result);
        }

        // -------------------------------------------------
        // GET: api/avaluacio/llista-per-anyescolar?idAnyEscolar=2024
        // -------------------------------------------------
        [HttpGet("llista-per-anyescolar")]
        [Authorize(Roles = "AG_Professor,AG_Alumne,AG_Admin")]
        public async Task<ActionResult<IEnumerable<Avaluacio>>> LlistaPerAnyEscolar(
            [FromQuery] int idAnyEscolar)
        {
            var result = await _avaluacioService.GetLlistaPerAnyEscolarAsync(
                idAnyEscolar,
                isAdmin: User.IsInRole("AG_Admin")
            );

            return Ok(result);
        }

        // -------------------------------------------------
        // GET: api/avaluacio/{idAvaluacio}
        // -------------------------------------------------
        [HttpGet("{idAvaluacio:long}")]
        [Authorize(Roles = "AG_Professor,AG_Alumne,AG_Admin")]
        public async Task<ActionResult<Avaluacio>> Instancia(int idAvaluacio)
        {
            var avaluacio = await _avaluacioService.GetByIdAsync(idAvaluacio);

            if (avaluacio == null)
                return NotFound();

            return Ok(avaluacio);
        }

        // -------------------------------------------------
        // POST: api/avaluacio/crear  
        // -------------------------------------------------
        [HttpPost("crear")]
        [Authorize(Roles = "AG_Professor,AG_Admin")]
        public async Task<ActionResult<Avaluacio>> Crear(AvaluacioTCrearDTO dto)
        {
            try
            {
                var result = await _avaluacioService.TCrearAsync(dto);
                return Ok(result); // torna l'objecte creat
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // -------------------------------------------------
        // PUT: api/avaluacio/editar  
        // -------------------------------------------------
        [HttpPut("editar")]
        [Authorize(Roles = "AG_Professor,AG_Admin")]
        public async Task<ActionResult<Avaluacio>> Editar(AvaluacioTEditarDTO dto)
        {
            try
            {
                var result = await _avaluacioService.TEditarAsync(dto);

                if (result == null)
                    return NotFound();

                return Ok(result); // torna l'objecte actualitzat
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // -------------------------------------------------
        // POST: api/avaluacio/{id}/iniciar  
        // -------------------------------------------------
        [HttpPost("{idAvaluacio:long}/iniciar")]
        [Authorize(Roles = "AG_Professor,AG_Admin")]
        public async Task<ActionResult<Avaluacio>> TIniciar(int idAvaluacio)
        {
            var avaluacio = await _avaluacioService.TIniciarAsync(idAvaluacio);

            if (avaluacio == null)
                return NotFound();

            return Ok(avaluacio); // retorna l’objecte
        }

        // -------------------------------------------------
        // POST: api/avaluacio/{id}/finalitzar  
        // -------------------------------------------------
        [HttpPost("{idAvaluacio:long}/finalitzar")]
        [Authorize(Roles = "AG_Professor,AG_Admin")]
        public async Task<ActionResult<Avaluacio>> Finalitzar(int idAvaluacio)
        {
            var avaluacio = await _avaluacioService.TFinalitzarAsync(idAvaluacio);

            if (avaluacio == null)
                return NotFound();

            return Ok(avaluacio); // retorna l’objecte
        }


        // -------------------------------------------------
        // DELETE: api/avaluacio/{id}   (ESBORRAR)
        // -------------------------------------------------
        [HttpDelete("{idAvaluacio}")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Eliminar(int idAvaluacio)
        {
            var esborrat = await _avaluacioService.EsborrarAsync(idAvaluacio);

            if (!esborrat)
                return NotFound(); // no existia

            return Ok(); // esborrat correctament (sense cos)
        }
    }
}