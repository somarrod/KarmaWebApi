using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/alumne")]
    public class AlumneController : ControllerBase
    {
        private readonly IAlumneService _alumneService;

        public AlumneController(IAlumneService alumneService)
        {
            _alumneService = alumneService;
        }

        // ==================================================
        // GET: api/alumne/{nia}
        // ==================================================
        [HttpGet("{nia}")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_Alumne,AG_EquipDirectiu")]
        public async Task<IActionResult> Instancia(string nia)
        {
            var alumne = await _alumneService.InstanciaAsync(nia, User);
            return Ok(alumne);
        }

        // ==================================================
        // GET: api/alumne/llista
        // ==================================================
        [HttpGet("llista")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_Alumne,AG_EquipDirectiu")]
        public async Task<IActionResult> Llista()
        {
            var alumnes = await _alumneService.LlistaAsync(User);
            return Ok(alumnes);
        }

        // ==================================================
        // POST: api/alumne/crear
        // ==================================================
        [HttpPost("crear")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> Crear([FromBody] AlumneDTO dto)
        {
            var alumne = await _alumneService.CrearAsync(dto);
            return Ok(alumne);
        }

        // ==================================================
        // PUT: api/alumne/editar
        // ==================================================
        [HttpPut("editar")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> Editar([FromBody] AlumneDTO dto)
        {
            var alumne = await _alumneService.EditarAsync(dto);
            return Ok(alumne);
        }

        // ==================================================
        // PUT: api/alumne/activar
        // ==================================================
        [HttpPut("activar")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> Activar([FromQuery] string nia)
        {
            var alumne = await _alumneService.ActivarAsync(nia);
            return Ok(alumne);
        }

        // ==================================================
        // PUT: api/alumne/desactivar
        // ==================================================
        [HttpPut("desactivar")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> Desactivar([FromQuery] string nia)
        {
            var alumne = await _alumneService.DesactivarAsync(nia);
            return Ok(alumne);
        }

        // ==================================================
        // PUT: api/alumne/assignar-classe
        // (el mètode AssignarClasseAsync l'afegiràs tu al service)
        // ==================================================
        [HttpPut("assignar-classe")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu")]
        public async Task<IActionResult> AssignarClasse(
            [FromQuery] string nia,
            [FromQuery] long idClasse)
        {
            var alumne = await _alumneService.AssignarClasseAsync(nia, idClasse);
            return Ok(alumne);
        }
    }
}