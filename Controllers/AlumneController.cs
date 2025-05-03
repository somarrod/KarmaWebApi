using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using KarmaWebAPI.Serveis.Interfaces;

namespace KarmaWebAPI.Controllers
{
    [Route("api/alumne")]
    [ApiController]
    public class AlumneController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IAlumneService _alumneService;
        private readonly UserManager<ApiUser> _userManager;
        private readonly AccountService _accountService;

        public AlumneController(DatabaseContext context, IAlumneService alumneService, UserManager<ApiUser> userManager, AccountService accountService)
        {
            _context = context;
            _alumneService = alumneService;
            _userManager = userManager;
            _accountService = accountService;
        }

        // GET: api/Alumne/5
        [HttpGet("{nia}")]
        public async Task<ActionResult<Alumne>> Instancia(string nia)
        {
            var alumne = await _context.Alumne.FindAsync(nia);

            if (alumne == null)
            {
                return NotFound();
            }

            return alumne;
        }

        // GET: api/Alumne
        [HttpGet("llista")]
        public async Task<ActionResult<IEnumerable<Alumne>>> Llista()
        {
            return await _context.Alumne.ToListAsync();
        }

        // POST: api/Alumne/crear
        [Authorize(Roles = "AG_Admin, AG_Professor")]
        [HttpPost("crear")]
        public async Task<ActionResult<Alumne>> Crear(AlumneDTO alumneDto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await _alumneService.CrearAlumneAsync(alumneDto);

                    // Fix: Ensure the result is cast or converted to the correct type
                    if (result.Result is OkObjectResult okResult && okResult.Value is Alumne alumne)
                    {
                        String password = FuncionsAuxiliars.ConstruirPasswordAlumne(alumneDto);
                        // Utilizar el nuevo método CreateUserAsync
                        var userCreated = await _accountService.CreateUserAsync(alumneDto.NIA, alumneDto.Email, "AG_Alumne", password);

                        if (!userCreated.Succeeded)
                        {
                            await transaction.RollbackAsync();
                            return BadRequest(userCreated.Errors.First().Description);
                        }
                        else { 
                            await transaction.CommitAsync();
                            return Ok(alumne);
                    }
                }
                    else
                    {
                        await transaction.RollbackAsync();
                        return BadRequest("No ha estat possible crear l'Alumne.");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return StatusCode(500, ex.InnerException!=null ? ex.InnerException.Message : ex.Message);
                }
            }
        }

        // PUT: api/Alumne/editar
        [HttpPut("activar")]
        public async Task<IActionResult> ActivarAlumne(string nia)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var alumne = await _context.Alumne.FindAsync(nia);
                    if (alumne == null)
                    {
                        return NotFound();
                    }
                    var activationResult = await _alumneService.ActivarAlumneAsync(nia);

                    // Fix: Ensure the variable name does not conflict with the outer scope
                    if (activationResult.Result is OkResult okResult)
                    {
                        // Utilizar el nuevo método CreateUserAsync
                        var reactivationResult = await _accountService.ReactivateUserAsync(alumne.Email);

                        if (!(reactivationResult.Succeeded))
                        {
                            await transaction.RollbackAsync();
                            return BadRequest(reactivationResult.Errors.First().Description);
                        }
                        else
                        {
                            await transaction.CommitAsync();
                            return Ok(alumne);
                        }
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return BadRequest("No ha estat possible activar l'alumne.");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return StatusCode(500, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
            }
        }

        // PUT: api/Alumne/editar
        [HttpPut("desactivar")]
        public async Task<IActionResult> DesactivarAlumne(string nia)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var alumne = await _context.Alumne.FindAsync(nia);
                    if (alumne == null)
                    {
                        return NotFound();
                    }
                    var activationResult = await _alumneService.DesactivarAlumneAsync(nia);

                    // Fix: Ensure the variable name does not conflict with the outer scope
                    if (activationResult.Result is OkResult okResult)
                    {
                        // Utilizar el nuevo método CreateUserAsync
                        var reactivationResult = await _accountService.InactivateUserAsync(alumne.Email);

                        if (!(reactivationResult.Succeeded))
                        {
                            await transaction.RollbackAsync();
                            return BadRequest(reactivationResult.Errors.First().Description);
                        }
                        else
                        {
                            await transaction.CommitAsync();
                            return Ok(alumne);
                        }
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return BadRequest("No ha estat possible desactivar l'alumne.");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return StatusCode(500, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
            }
        }

        // DELETE: api/Alumne/eliminar
        [HttpDelete("eliminar")]
        public async Task<IActionResult> Eliminar(string nia)
        {
            var alumne = await _context.Alumne.FindAsync(nia);
            if (alumne == null)
            {
                return NotFound();
            }

            _context.Alumne.Remove(alumne);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlumneExisteix(string nia)
        {
            return _context.Alumne.Any(e => e.NIA == nia);
        }
    }
}
