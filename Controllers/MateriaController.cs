using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/materia")]
    public class MateriaController : ControllerBase
    {
        private readonly IMateriaService _service;

        public MateriaController(IMateriaService service)
        {
            _service = service;
        }

        // =========================
        // GET instància
        // =========================
        [HttpGet("{idMateria}")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> Instancia(int idMateria)
        {
            var materia = await _service.InstanciaAsync(idMateria, User);
            if (materia == null)
                return NotFound("Matèria no trobada");

            return Ok(materia);
        }

        // =========================
        // GET llista
        // =========================
        [HttpGet("llista")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> Llista()
        {
            var llista = await _service.LlistaAsync(User);
            return Ok(llista);
        }

        // =========================
        // POST crear
        // =========================
        [HttpPost("crear")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Crear(MateriaCrearDTO dto)
        {
            var materia = await _service.CrearAsync(dto);
            return Ok(materia);
        }

        // =========================
        // PUT editar
        // =========================
        [HttpPut("editar")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Editar(MateriaEditarDTO dto)
        {
            var materia = await _service.EditarAsync(dto);
            return Ok(materia);
        }

        // =========================
        // PUT activar / desactivar
        // =========================
        [HttpPut("activar")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> Activar(int idMateria)
        {
            var materia = await _service.ActivarAsync(idMateria);
            return Ok(materia);
        }

        [HttpPut("desactivar")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> Desactivar(int idMateria)
        {
            var materia = await _service.DesactivarAsync(idMateria);
            return Ok(materia);
        }

        // =========================
        // DELETE eliminar
        // =========================
        [HttpDelete("{idMateria}")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Eliminar(int idMateria)
        {
            var ok = await _service.EliminarAsync(idMateria);
            if (!ok)
                return NotFound();

            return Ok();
        }
    }
}