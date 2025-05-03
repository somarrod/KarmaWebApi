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

        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new ApiUser { UserName = model.Email, Email = model.Email, login = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {

                // Asignar el rol al usuario
                var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }

            return BadRequest(result.Errors);
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


    }

}
