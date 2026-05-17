using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.DisplaySets;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/professordeclasse")]
    public class ProfessorDeClasseController : ControllerBase
    {
        private readonly IProfessorDeClasseService _service;

        public ProfessorDeClasseController(IProfessorDeClasseService service)
        {
            _service = service;
        }

        // ==================================================
        // GET: api/professordeclasse/per-anyescolar/2627
        // Llista completa
        // ==================================================
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu,AG_Alumne")]

        [HttpGet("anyescolar/{idAnyEscolar:int}")]
        public async Task<IActionResult> GetPerAnyEscolar(int idAnyEscolar)
        {
            try
            {
                return Ok(await _service.GetLlistaAsync(idAnyEscolar));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        // ==================================================
        // GET: api/professordeclasse/{id}
        // Instància (DisplaySet)
        // ==================================================
        [HttpGet("{idProfessorDeClasse:long}")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu,AG_Alumne")]
        public async Task<ActionResult<ProfessorDeClasseDisplaySet>> Instancia(
            long idProfessorDeClasse)
        {
            var result = await _service.InstanciaAsync(idProfessorDeClasse);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // ==================================================
        // POST: api/professordeclasse
        // Assignar professor a classe i matèria
        // ==================================================
        [HttpPost("assignar")]
        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu")]
        public async Task<ActionResult<ProfessorDeClasseDisplaySet>> Assignar(ProfessorDeClasseCrearDTO dto)
        {
            try
            {
                var relacio = await _service.AssignarProfessorAClasseAsync(dto);

                return Ok(relacio); // retorna objecte creat
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ==================================================
        // DELETE: api/professordeclasse
        // Esborrar relació
        // ==================================================
        [HttpDelete("eliminar/{idProfessorDeClasse:long}")]
        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu")]
        public async Task<IActionResult> Esborrar(long idProfessorDeClasse)
        {
            var esborrat = await _service.EsborrarAsync(idProfessorDeClasse);

            if (!esborrat)
                return NotFound();

            return Ok(); // OK buit
        }
    }
}