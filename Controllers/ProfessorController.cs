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
    [Route("api/professor")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IProfessorService _professorService;   
        private readonly AccountService _accountService;
        private readonly UserManager<ApiUser> _userManager;

        public ProfessorController(DatabaseContext context, IProfessorService professorService , UserManager<ApiUser> userManager, AccountService accountService)
        {
            _context = context;
            _professorService = professorService;
            _accountService = accountService;
            _userManager = userManager;
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
        [Authorize(Roles = "AG_Admin, AG_Professor")]
        public async Task<IActionResult> Editar(ProfessorDTO professorDto)
        {
            var professor = await _context.Professor.FindAsync(professorDto.IdProfessor);
            if (professor == null)
            {
                return NotFound();
            }

            // Comprovar si l'email ja existeix en altres professors
            var emailExists = await _context.Professor.AnyAsync(p => p.Email == professorDto.Email && p.IdProfessor != professorDto.IdProfessor);
            if (emailExists)
            {
                return BadRequest("L'email ja està en ús per un altre professor.");
            }

            // Iniciar la transacció
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Actualitzar les dades del professor
                    professor.Nom = professorDto.Nom;
                    professor.Cognoms = professorDto.Cognoms;
                    professor.Email = professorDto.Email;

                    _context.Professor.Update(professor);
                    await _context.SaveChangesAsync();

                    // Actualitzar l'email en AspNetUsers
                    var user = await _userManager.FindByNameAsync(professorDto.IdProfessor.ToString());
                    if (user != null)
                    {
                        user.Email = professorDto.Email;

                        var result = await _userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            throw new Exception("Error actualitzant l'email en AspNetUsers.");
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Error actualitzant l'email en AspNetUsers.");
                    }

                    // Confirmar la transacció
                    await transaction.CommitAsync();

                    return Ok(professor);
                }
                catch (Exception ex)
                {
                    // Tirar enrere la transacció en cas d'error
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Error actualitzant el professor: {ex.Message}");
                }
            }
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


        [HttpDelete("eliminar")]
        [Authorize(Roles = "AG_Admin, AG_Professor")]
        public async Task<IActionResult> EliminarProfessor(String idProfessor)
        {
            var professor = await _context.Professor.FindAsync(idProfessor);
            if (professor == null)
            {
                return NotFound();
            }

            // Comprovar si el professor està en una relació en ProfessorEnGrup
            var professorEnGrupExists = await _context.ProfessorDeGrup.AnyAsync(peg => peg.IdProfessor == idProfessor);
            if (professorEnGrupExists)
            {
                return BadRequest("No es pot esborrar el professor perquè està en una relació en ProfessorEnGrup. S'ha de desactivar.");
            }

            // Comprovar si el professor està en una relació en ProfessorEnGrup
            var esProfessorTutor = await _context.Grup.AnyAsync(g => g.IdProfessorTutor == idProfessor);
            if (esProfessorTutor)
            {
                return BadRequest("No es pot esborrar el professor perquè es tutor. S'ha de desactivar.");
            }

            // Iniciar la transacció
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Eliminar el professor
                    _context.Professor.Remove(professor);
                    await _context.SaveChangesAsync();

                    // Eliminar l'usuari en AspNetUsers
                    var user = await _userManager.FindByNameAsync(idProfessor.ToString());
                    if (user != null)
                    {
                        var result = await _userManager.DeleteAsync(user);
                        if (!result.Succeeded)
                        {
                            throw new Exception("Error eliminant l'usuari en AspNetUsers.");
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Error eliminant l'usuari en AspNetUsers.");
                    }

                    // Confirmar la transacció
                    await transaction.CommitAsync();

                    return Ok("Professor eliminat correctament.");
                }
                catch (Exception ex)
                {
                    // Tirar enrere la transacció en cas d'error
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Error eliminant el professor: {ex.Message}");
                }
            }
        }

    }
}
