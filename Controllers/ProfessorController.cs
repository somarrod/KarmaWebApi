using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Controllers
{
    [Route("api/professor")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly IProfessorService _service;
        private readonly AccountService _accountService;
        private readonly UserManager<ApiUser> _userManager;

        public ProfessorController(
            IProfessorService service,
            AccountService accountService,
            UserManager<ApiUser> userManager)
        {
            _service = service;
            _accountService = accountService;
            _userManager = userManager;
        }
        //--------------------------------------------------
        // CONSULTES 
        //--------------------------------------------------

        [HttpGet("llista")]
        public async Task<ActionResult<IEnumerable<Professor>>> Llista()
        {
            return Ok(await _service.LlistarProfessorsAsync());
        }


        [HttpGet("llista/actius")]
        public async Task<ActionResult<IEnumerable<Professor>>> LlistaActius()
        {
            var professors = await _service.LlistarProfessorsActiusAsync();
            return Ok(professors);
        }


        [HttpGet("{idProfessor}")]
        public async Task<ActionResult<Professor>> Obtenir(string idProfessor)
        {
            var professor = await _service.ObtenirProfessorPerIdAsync(idProfessor);
            return professor == null ? NotFound() : Ok(professor);
        }

        //--------------------------------------------------
        // SERVEIS 
        //--------------------------------------------------
        [Authorize(Roles = "AG_Admin")]
        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] ProfessorDTO dto)
        {
            ApiUser? user = null;

            try
            {
                var password = FuncionsAuxiliars.ConstruirPasswordProfessor(dto);

                var result = await _accountService.CreateUserAsync(
                    dto.IdProfessor,
                    dto.Email,
                    "AG_Professor",
                    password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors.First().Description);

                user = await _userManager.FindByNameAsync(dto.IdProfessor);

                var professor = await _service.CrearProfessorAsync(dto);
                return Ok(professor);
            }
            catch (Exception ex)
            {
                if (user != null)
                    await _userManager.DeleteAsync(user); // rollback manual

                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu,AG_Professor")]
        [HttpPut("editar")]
        public async Task<IActionResult> Editar([FromBody] ProfessorDTO dto)
        {
            try
            {
                var userId = User.Identity!.Name!;
                bool esAdminOEquip = User.IsInRole("AG_Admin") || User.IsInRole("AG_EquipDirectiu");

                var professor = await _service.EditarProfessorAsync(dto, userId, esAdminOEquip);
                return Ok(professor);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu")]
        [HttpPut("activar/{idProfessor}")]
        public async Task<IActionResult> Activar(string idProfessor)
        {
            var professor = await _service.ActivarProfessorAsync(idProfessor);
            await _accountService.ReactivateUserAsync(professor.Email);
            return Ok(professor);
        }

        [Authorize(Roles = "AG_Admin,AG_EquipDirectiu")]
        [HttpPut("desactivar/{idProfessor}")]
        public async Task<IActionResult> Desactivar(string idProfessor)
        {
            var professor = await _service.DesactivarProfessorAsync(idProfessor);
            await _accountService.InactivateUserAsync(professor.Email);
            return Ok(professor);
        }

        [Authorize(Roles = "AG_Admin")]
        [HttpDelete("eliminar/{idProfessor}")]
        public async Task<IActionResult> Eliminar(string idProfessor)
        {
            await _service.EliminarProfessorAsync(idProfessor);

            var user = await _userManager.FindByNameAsync(idProfessor);
            if (user != null)
                await _userManager.DeleteAsync(user);

            return Ok();
        }
    }
}