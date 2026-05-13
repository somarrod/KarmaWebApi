using KarmaWebAPI.DTOs;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/tipus-categoria")]
    
    public class TipusCategoriaController : ControllerBase
    {
        private readonly ITipusCategoriaService _service;

        public TipusCategoriaController(ITipusCategoriaService service)
        {
            _service = service;
        }

        // ============================================
        // LLISTA
        // GET api/tipus-categoria/llista
        // ============================================
        [Authorize(Roles = "AG_Admin, AG_Professor, AG_Alumne")]
        [HttpGet("llista")]
        public async Task<IActionResult> Llista()
        {
            var llista = await _service.LlistaAsync();
            return Ok(llista);
        }

        // ============================================
        // LLISTA ACTIUS
        // GET api/tipus-categoria/actius
        // ============================================
        [HttpGet("llista-actius")]
        public async Task<IActionResult> LlistaActius()
        {
            var llista = await _service.LlistaActiusAsync();
            return Ok(llista);
        }


        // ============================================
        // INSTÀNCIA
        // GET api/tipus-categoria/{id}
        // ============================================
        [HttpGet("{idTipusCategoria:long}")]
        [Authorize(Roles = "AG_Admin, AG_Professor, AG_Alumne")]
        public async Task<IActionResult> Instancia(long idTipusCategoria)
        {
            var resultat = await _service.InstanciaAsync(idTipusCategoria);

            if (resultat == null)
                return NotFound($"No existeix cap tipus de categoria amb id {idTipusCategoria}");

            return Ok(resultat);
        }

        // ============================================
        // CREAR
        // POST api/tipus-categoria/crear
        // ============================================
        [Authorize(Roles = "AG_Admin")]
        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] TipusCategoriaCrearDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultat = await _service.CrearAsync(dto);
            return Ok(resultat);
        }

        // ============================================
        // EDITAR
        // PUT api/tipus-categoria/editar
        // ============================================
        [Authorize(Roles = "AG_Admin")]
        [HttpPut("editar")]
        public async Task<IActionResult> Editar([FromBody] TipusCategoriaEditarDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existeix = await _service.ExisteixAsync(dto.IdTipusCategoria);

            if (!existeix)
                return NotFound($"No existeix cap tipus de categoria amb id {dto.IdTipusCategoria}");

            var resultat = await _service.EditarAsync(dto);

            return Ok(resultat);
        }

        // ============================================
        // ELIMINAR
        // DELETE api/tipus-categoria/eliminar/{id}
        // ============================================
        [Authorize(Roles = "AG_Admin")]
        [HttpDelete("eliminar/{idTipusCategoria:long}")]
        public async Task<IActionResult> Eliminar(long idTipusCategoria)
        {
            var existeix = await _service.ExisteixAsync(idTipusCategoria);

            if (!existeix)
                return NotFound($"No existeix cap tipus de categoria amb id {idTipusCategoria}");

            await _service.EliminarAsync(idTipusCategoria);

            return Ok();
        }
    }
}