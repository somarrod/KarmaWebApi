using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using KarmaWebAPI.Serveis.Interfaces;
//using KarmaWebAPI.Serveis;
//using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

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


        [HttpPut("editar")]
        [Authorize(Roles = "AG_Admin, AG_Professor")]
        public async Task<IActionResult> EditarAlumne([FromBody] AlumneDTO alumneDto)
        {
            var alumne = await _context.Alumne.FindAsync(alumneDto.NIA);
            if (alumne == null)
            {
                return NotFound();
            }

            // Comprovar si l'email ja existeix en altres alumnes
            var emailExists =   await _context.Alumne.AnyAsync(a => a.Email == alumneDto.Email && a.NIA != alumneDto.NIA);

            if (emailExists)
            {
                return BadRequest("L'email ja està en ús per un altre alumne.");
            }

            // Iniciar la transacció
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Actualitzar les dades de l'alumne
                    alumne.Nom = alumneDto.Nom;
                    alumne.Cognoms = alumneDto.Cognoms;
                    alumne.Email = alumneDto.Email;

                    _context.Alumne.Update(alumne);
                    await _context.SaveChangesAsync();

                    // Actualitzar l'email en AspNetUsers
                    var user = await _userManager.FindByNameAsync(alumneDto.NIA);
                    if (user != null)
                    {
                        user.Email = alumneDto.Email;

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

                    return Ok(alumne);
                }
                catch (Exception ex)
                {
                    // Tirar enrere la transacció en cas d'error
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Error actualitzant l'alumne: {ex.Message}");
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

        [HttpDelete("eliminar")]
        [Authorize(Roles = "AG_Admin, AG_Professor")]
        public async Task<IActionResult> Eliminar(String nia)
        {
            var alumne = await _context.Alumne.FindAsync(nia);
            if (alumne == null)
            {
                return NotFound();
            }

            // Comprovar si l'alumne està en una relació en AlumneEnGrup
            var alumneEnGrupExists = await _context.AlumneEnGrup.AnyAsync(aeg => aeg.NIA== nia);
            if (alumneEnGrupExists)
            {
                return BadRequest("No es pot esborrar l'alumne perquè està en un grup. El pot inactivar.");
            }

            // Iniciar la transacció
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Eliminar l'alumne
                    _context.Alumne.Remove(alumne);
                    await _context.SaveChangesAsync();

                    // Eliminar l'usuari en AspNetUsers
                    var user = await _userManager.FindByNameAsync(nia);
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

                    return Ok("Alumne eliminat correctament.");
                }
                catch (Exception ex)
                {
                    // Tirar enrere la transacció en cas d'error
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Error eliminant l'alumne: {ex.Message}");
                }
            }
        }


        private bool AlumneExisteix(string nia)
        {
            return _context.Alumne.Any(e => e.NIA == nia);
        }
    }
}
