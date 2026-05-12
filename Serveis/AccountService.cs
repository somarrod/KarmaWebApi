using System.Linq;
using System.Threading.Tasks;
using KarmaWebAPI;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

public class AccountService
{
    private readonly UserManager<ApiUser> _userManager;
    private readonly DatabaseContext _context;

    public AccountService(UserManager<ApiUser> userManager, DatabaseContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IdentityResult> CreateUserAsync(string id, string? email, string role, string password)
    {
        string login = null;

        if (string.IsNullOrWhiteSpace(email))
        {
            email = $"{id}@alumnat.val";
            login = id;
        }
        else 
        {
            login = email;
        }

        if (email != null)
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
        }

        // Crear el nuevo usuario
        ApiUser user = new ApiUser
            {
                UserName = id,
                Email = email,
                Login = login
            };
      

        var userCreated = await _userManager.CreateAsync(user, password);

        if (userCreated.Succeeded)
        {
            ApiUser? userMng;
            userMng = await _userManager.FindByEmailAsync(email);

            if (userMng == null)
                throw new Exception("Usuari creat però no trobat per a assignar rol.");

            var roleResult = await _userManager.AddToRoleAsync(userMng, role);
            if (!roleResult.Succeeded)
            {
                throw new Exception(string.Join("; ",
                    roleResult.Errors.Select(e => e.Description)));
            }
        }
        else
        {
            throw new Exception(string.Join("; ",
                userCreated.Errors.Select(e => e.Description)));
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


    public async Task BloquejarUsuariAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new InvalidOperationException("Usuari no trobat");

        await _userManager.SetLockoutEndDateAsync(
            user,
            DateTimeOffset.MaxValue
        );
    }


    public async Task DesbloquejarUsuariAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new InvalidOperationException("Usuari no trobat");

        await _userManager.SetLockoutEndDateAsync(
            user,
            null
        );
    }

    public async Task EnsureUserWithRoleAsync(
    string userName,
    string email,
    string role,
    string password)

    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
        {
            user = new ApiUser
            {
                UserName = userName,
                Email = email,
                Login = userName,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception(string.Join("; ",
                    result.Errors.Select(e => e.Description)));
        }

        if (!await _userManager.IsInRoleAsync(user, role))
        {
            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
                throw new Exception(string.Join("; ",
                    roleResult.Errors.Select(e => e.Description)));
        }
    }


}
