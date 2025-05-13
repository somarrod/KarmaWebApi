using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace KarmaWebAPI.Controllers
{

    [ApiController]
    [Route("api/account")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly SignInManager<ApiUser> _signInManager;

        public AuthController(UserManager<ApiUser> userManager, SignInManager<ApiUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        
        [HttpPost("registeradmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDTO model)
        {
            var user = new ApiUser { UserName = model.Email, Email = model.Email, Login = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {

                // Asignar el rol al usuario
                var roleResult = await _userManager.AddToRoleAsync(user, "AG_Admin");
                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok("Administrador creat correctament.");
            }

            return BadRequest(result.Errors);
        }


        [HttpGet("agents")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> GetAgents()
        {
            var users = _userManager.Users.ToList();
            var agents = new List<AgentDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                agents.Add(new AgentDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles
                });
            }

            return Ok(agents);
        }
    
      
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Buscar el usuario por email
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return Unauthorized(new { message = "Invàlid intent de login" });
            }

            // Verificar si el usuario está activo
            if (!user.IsActive)
            {
                return Unauthorized(new { message = "L'usuari està desactivat" });
            }

            // Intentar iniciar sesión
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new { message = "Login correcte" });
            }

            return Unauthorized(new { message = "Invàlid intent de login" });
        }

        [HttpGet("accessdenied")]
        public IActionResult AccessDenied()
        {
            return Unauthorized();
        }


        [Authorize]
        [HttpPost("logout")]
       // [ValidateAntiForgeryToken]

       public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Ok(new { message = "Logout correcte" });
        }


        [HttpPost("canviapassword")]
        [Authorize]
        public async Task<IActionResult> CanviaPassword([FromBody] AuthCanviaPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (User.Identity is null) 
            {
                return NotFound("Usuari no connectat");
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound("Usuari no trobat");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.PasswordActual, model.NouPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password canviat correctament");
        }


        [Authorize(Roles = "AG_Admin")]
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] AuthResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("Usuari no trobat");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NouPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok($"Password canviat correctament. Nou Password = {model.NouPassword}");
        }




    }

}
