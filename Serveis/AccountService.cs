using System.Linq;
using System.Threading.Tasks;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class AccountService
{
    private readonly UserManager<ApiUser> _userManager;
    private readonly DatabaseContext _context;

    public AccountService(UserManager<ApiUser> userManager, DatabaseContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IdentityResult> CreateUserAsync(string id, string email, string role)
    {
        // Verificar si el email ya existe en la base de datos
        var existingUser = await _context.Users
                                .Where(u => u.Email == email)
                                .FirstOrDefaultAsync();

        if (existingUser != null)
        {
            // El email ya está registrado
            return IdentityResult.Failed(new IdentityError
            {
                Code = "DuplicateEmail",
                Description = "L'email utilitzat ja està prèviament registrat."
            });
        }

        // Crear el nuevo usuario
        var user = new ApiUser
        {
            UserName = id,
            Email = email,
            login = email
        };

        var userCreated = await _userManager.CreateAsync(user, "Password1!");

        if (userCreated.Succeeded)
        {
            // Asignar el rol al usuario
            var userMng = await _userManager.FindByEmailAsync(email);
            var roleResult = await _userManager.AddToRoleAsync(userMng,role);
            if (!roleResult.Succeeded)
            {
                // Reemplazar BadRequest con una excepción personalizada o un manejo adecuado
                throw new System.Exception(string.Join("; ", roleResult.Errors.Select(e => e.Description)));
            }
            // await _signInManager.SignInAsync(user, isPersistent: false);
        }
        else
        {
            // Reemplazar BadRequest con una excepción personalizada o un manejo adecuado
            throw new System.Exception(string.Join("; ", userCreated.Errors.Select(e => e.Description)));
        }
        return userCreated;
    }


    public async Task<IdentityResult> ReactivateUserAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "UserNotFound",
                Description = "L'usuari no existeix."
            });
        }

        user.IsActive = true;
        var result = await _userManager.UpdateAsync(user);
        return result;
    }


    public async Task<IdentityResult> InactivateUserAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "UserNotFound",
                Description = "L'usuari no existeix."
            });
        }

        user.IsActive = false;
        var result = await _userManager.UpdateAsync(user);
        return result;
    }
}
