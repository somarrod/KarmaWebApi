using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using KarmaWebAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/alumne")]
    public class AlumneController : ControllerBase
    {
        private readonly IAlumneService _alumneService;
        private readonly AccountService _accountService;
        private readonly DatabaseContext _context;

        public AlumneController(
            IAlumneService alumneService,
            AccountService accountService,
            DatabaseContext context)
        {
            _alumneService = alumneService;
            _accountService = accountService;
            _context = context;
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
            try
            {
                var alumne = await _alumneService.CrearAsync(dto);
                return Ok(alumne);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500,
                    new { error = "S'ha produït un error intern en crear l'alumne." });
            }
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
        // PUT: api/alumne/activar/{nia}
        // ==================================================
        [HttpPut("activar/{nia}")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> Activar(string nia)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Activar alumne (BD)
                var alumne = await _alumneService.ActivarAsync(nia);

                // 2. Reactivar usuari Identity (UserName = NIA)
                await _accountService.ReactivateUserAsync(nia);

                // 3. Commit
                await tx.CommitAsync();
                return Ok(alumne);
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return StatusCode(500, new
                {
                    error = "No s'ha pogut activar l'alumne",
                    detail = ex.Message
                });
            }
        }

        // ==================================================
        // PUT: api/alumne/desactivar/{nia}
        // ==================================================
        [HttpPut("desactivar/{nia}")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> Desactivar(string nia)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Desactivar alumne (BD)
                var alumne = await _alumneService.DesactivarAsync(nia);

                // 2. Desactivar usuari Identity (UserName = NIA)
                await _accountService.InactivateUserAsync(nia);

                // 3. Commit
                await tx.CommitAsync();
                return Ok(alumne);
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return StatusCode(500, new
                {
                    error = "No s'ha pogut desactivar l'alumne",
                    detail = ex.Message
                });
            }
        }

        // ==================================================
        // PUT: api/alumne/assignar-classe
        // (gestiona IdAnyEscolar + IdClasse dins del service)
        // ==================================================
        [HttpPut("assignar-classe")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu")]
        public async Task<IActionResult> AssignarClasse(
            AlumneAssignarClasseDTO dto)
        {
            var alumne = await _alumneService.AssignarClasseAsync(dto);
            return Ok(alumne);
        }

        // ==================================================
        // PUT: api/alumne/assignar-grup
        // Assigna un grup a un alumne:
        // - Desvincula el grup anterior (si en tenia)
        // - El nou grup ha de pertànyer a la mateixa classe
        // ==================================================
        [HttpPut("assignar-grup")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu")]
        public async Task<IActionResult> AssignarGrup(
            [FromQuery] string nia,
            [FromQuery] long idGrup)
        {
            try
            {
                var alumne = await _alumneService.AssignarGrupAsync(nia, idGrup);
                return Ok(alumne);
            }
            catch (InvalidOperationException ex)
            {
                // Errors funcionals (classe incorrecta, alumne/grup inexistent, etc.)
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    error = "S'ha produït un error assignant el grup a l'alumne"
                });
            }
        }


        [HttpPost("sincronitzar-identity")]
        [Authorize(Roles = "AG_Admin, AG_Professor")]
        public async Task<IActionResult> SincronitzarIdentity()
        {
            await _alumneService.SincronitzarIdentityAsync();
            return Ok();
        }


    }
}