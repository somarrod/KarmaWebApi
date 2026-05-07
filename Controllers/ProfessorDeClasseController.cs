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
        // GET: api/professordeclasse
        // Llista completa
        // ==================================================
        [HttpGet]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu")]
        public async Task<ActionResult<IEnumerable<ProfessorDeClasseDisplaySet>>> Llista()
        {
            var llista = await _service.GetLlistaAsync();
            return Ok(llista);
        }

        // ==================================================
        // GET: api/professordeclasse/{id}
        // Instància (DisplaySet)
        // ==================================================
        [HttpGet("{idProfessorDeClasse:long}")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu")]
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
        [HttpPost]
        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu")]
        public async Task<ActionResult<ProfessorDeClasse>> Assignar(
            [FromQuery] string idProfessor,
            [FromQuery] string idClasse,
            [FromQuery] long idMateria)
        {
            try
            {
                var relacio = await _service.AssignarAsync(
                    idProfessor, idClasse, idMateria);

                return Ok(relacio); // ✅ retorna objecte creat
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
        [HttpDelete]
        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu")]
        public async Task<IActionResult> Esborrar(
            [FromQuery] string idProfessor,
            [FromQuery] string idClasse,
            [FromQuery] long idMateria)
        {
            var esborrat = await _service.EsborrarAsync(
                idProfessor, idClasse, idMateria);

            if (!esborrat)
                return NotFound();

            return Ok(); // OK buit
        }
    }
}