using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using KarmaWebAPI.Serveis.Interfaces;
using static KarmaWebAPI.FuncionsAuxiliars; // Cambiar a una directiva de uso estático para evitar el error CS0138

using static KarmaWebAPI.FuncionsAuxiliars;
namespace KarmaWebAPI.Controllers
{
    [Route("api/professor")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IProfessorService _professorService;   
        private readonly UserManager<ApiUser> _userManager;
        private readonly AccountService _accountService;

        public ProfessorController(DatabaseContext context, IProfessorService professorService , UserManager<ApiUser> userManager, AccountService accountService)
        {
            _context = context;
            _professorService = professorService;
            _userManager = userManager;
            _accountService = accountService;
        }

        // GET: api/Professor/5
        [HttpGet("{idProfessor}")]
        public async Task<ActionResult<Professor>> Instancia(string idProfessor)
        {
            var professor = await _context.Professor.FindAsync(idProfessor);

            if (professor == null)
            {
                return NotFound();
            }

            return professor;
        }

        // GET: api/Professor
        [HttpGet("llista")]
        public async Task<ActionResult<IEnumerable<Professor>>> Llista()
        {
            return await _context.Professor.ToListAsync();
        }


        // POST: api/Professor/crear
        [Authorize(Roles = "AG_Admin")]
        [HttpPost("crear")]
        public async Task<ActionResult<Professor>> Crear(ProfessorDTO professorDto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await _professorService.CrearProfessorAsync(professorDto);

                    // Fix: Ensure the result is cast or converted to the correct type
                    if (result.Result is OkObjectResult okResult && okResult.Value is Professor professor)
                    {

                        var password = FuncionsAuxiliars.ConstruirPasswordProfessor(professorDto);

                        // Utilizar el nuevo método CreateUserAsync
                        var userCreated = await _accountService.CreateUserAsync(professorDto.IdProfessor, professorDto.Email, "AG_Professor", password);

                        if (!userCreated.Succeeded)
                        {
                            await transaction.RollbackAsync();
                            return BadRequest(userCreated.Errors.First().Description);
                        }
                        else { 
                            await transaction.CommitAsync();
                            return Ok(professor);
                    }
                }
                    else
                    {
                        await transaction.RollbackAsync();
                        return BadRequest("No ha estat possible crear el Professor.");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return StatusCode(500, ex.InnerException!=null ? ex.InnerException.Message : ex.Message);
                }
            }
        }


        // PUT: api/Professor/editar
        [HttpPut("editar")]
        public async Task<IActionResult> Editar(ProfessorDTO professorDto)
        {
            var professor = new Professor
            {
                IdProfessor = professorDto.IdProfessor,
                Nom = professorDto.Nom,
                Cognoms = professorDto.Cognoms,
                Email = professorDto.Email
            };

            _context.Entry(professor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_professorService.ProfessorExisteix(professorDto.IdProfessor))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }


        // PUT: api/Alumne/editar
        [HttpPut("activar")]
        public async Task<IActionResult> ActivarProfessor(String idProfessor)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var professor = await _context.Professor.FindAsync(idProfessor);
                    if (professor == null)
                    {
                        return NotFound();
                    }
                    var activationResult = await _professorService.ActivarProfessorAsync(idProfessor);

                    // Fix: Ensure the variable name does not conflict with the outer scope
                    if (activationResult.Result is OkResult okResult)
                    {
                        // Utilizar el nuevo método CreateUserAsync
                        var reactivationResult = await _accountService.ReactivateUserAsync(professor.Email);

                        if (!(reactivationResult.Succeeded))
                        {
                            await transaction.RollbackAsync();
                            return BadRequest(reactivationResult.Errors.First().Description);
                        }
                        else
                        {
                            await transaction.CommitAsync();
                            return Ok(professor);
                        }
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return BadRequest("No ha estat possible activar el professor.");
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
        public async Task<IActionResult> DesactivarProfessor(String idProfessor)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var professor = await _context.Professor.FindAsync(idProfessor);
                    if (professor == null)
                    {
                        return NotFound();
                    }
                    var activationResult = await _professorService.DesactivarProfessorAsync(idProfessor);

                    // Fix: Ensure the variable name does not conflict with the outer scope
                    if (activationResult.Result is OkResult okResult)
                    {
                        // Utilizar el nuevo método CreateUserAsync
                        var reactivationResult = await _accountService.InactivateUserAsync(professor.Email);

                        if (!(reactivationResult.Succeeded))
                        {
                            await transaction.RollbackAsync();
                            return BadRequest(reactivationResult.Errors.First().Description);
                        }
                        else
                        {
                            await transaction.CommitAsync();
                            return Ok(professor);
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



        // DELETE: api/Professor/eliminar
        [HttpDelete("eliminar")]
        public async Task<IActionResult> Eliminar(string idProfessor)
        {
            var professor = await _context.Professor.FindAsync(idProfessor);
            if (professor == null)
            {
                return NotFound();
            }

            _context.Professor.Remove(professor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //private bool ProfessorExisteix(string id)
        //{
        //    return _context.Professor.Any(e => e.IdProfessor == id);
        //}
    }
}
