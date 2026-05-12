using KarmaWebAPI;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class ProfessorService : IProfessorService
{
    private readonly DatabaseContext _context;
    private readonly AccountService _accountService;
    private readonly UserManager<ApiUser> _userManager;

    public ProfessorService(DatabaseContext context, UserManager<ApiUser> userManager, AccountService accountService)
    {
        _context = context;
        _accountService = accountService;
        _userManager = userManager;
    }

    public async Task<List<Professor>> LlistarProfessorsAsync()
    {
        return await _context.Professors
            .OrderBy(p => p.Cognoms)
            .ThenBy(p => p.Nom)
            .ToListAsync();
    }


    public async Task<List<Professor>> LlistarProfessorsActiusAsync()
    {
        return await _context.Professors
            .Where(p => p.Actiu)
            .OrderBy(p => p.Cognoms)
            .ThenBy(p => p.Nom)
            .ToListAsync();
    }


    public async Task<Professor?> ObtenirProfessorPerIdAsync(string idProfessor)
    {
        return await _context.Professors.FindAsync(idProfessor);
    }

    public async Task<Professor> CrearProfessorAsync(ProfessorDTO dto)
    {
        var professor = new Professor
        {
            IdProfessor = dto.IdProfessor,
            Nom = dto.Nom,
            Cognoms = dto.Cognoms,
            Email = dto.Email,
            Actiu = true,
            PertanyAEquipDirectiu = dto.PertanyAEquipDirectiu
        };

        _context.Professors.Add(professor);
        await _context.SaveChangesAsync();

        var user = await _userManager.FindByNameAsync(dto.IdProfessor)
            ?? throw new InvalidOperationException("Usuari identity no trobat");

        if (dto.PertanyAEquipDirectiu &&
            !await _userManager.IsInRoleAsync(user, "AG_EquipDirectiu"))
        {
            await _userManager.AddToRoleAsync(user, "AG_EquipDirectiu");
        }

        return professor;
    }

    public async Task<Professor> EditarProfessorAsync(
        ProfessorDTO dto,
        string userId,
        bool esAdminOEquipDirectiu)
    {
        var professor = await _context.Professors.FindAsync(dto.IdProfessor)
            ?? throw new KeyNotFoundException("Professor no trobat");

        if (!esAdminOEquipDirectiu && dto.IdProfessor != userId)
            throw new UnauthorizedAccessException();

        professor.Nom = dto.Nom;
        professor.Cognoms = dto.Cognoms;
        professor.Email = dto.Email;
        professor.PertanyAEquipDirectiu = dto.PertanyAEquipDirectiu;

        var user = await _userManager.FindByNameAsync(dto.IdProfessor)
            ?? throw new InvalidOperationException("Usuari identity no trobat");

        user.Email = dto.Email;
        await _userManager.UpdateAsync(user);

        bool estaEnEquip = await _userManager.IsInRoleAsync(user, "AG_EquipDirectiu");

        if (dto.PertanyAEquipDirectiu && !estaEnEquip)
            await _userManager.AddToRoleAsync(user, "AG_EquipDirectiu");

        if (!dto.PertanyAEquipDirectiu && estaEnEquip)
            await _userManager.RemoveFromRoleAsync(user, "AG_EquipDirectiu");

        await _context.SaveChangesAsync();
        return professor;
    }

    public async Task<Professor> ActivarProfessorAsync(string idProfessor)
    {
        var professor = await _context.Professors.FindAsync(idProfessor)
            ?? throw new KeyNotFoundException("Professor no trobat");

        professor.Actiu = true;
        await _context.SaveChangesAsync();
        return professor;
    }

    public async Task<Professor> DesactivarProfessorAsync(string idProfessor)
    {
        var professor = await _context.Professors.FindAsync(idProfessor)
            ?? throw new KeyNotFoundException("Professor no trobat");

        professor.Actiu = false;
        await _context.SaveChangesAsync();
        return professor;
    }

    public async Task EliminarProfessorAsync(string idProfessor)
    {
        var professor = await _context.Professors.FindAsync(idProfessor)
            ?? throw new KeyNotFoundException("Professor no trobat");

        _context.Professors.Remove(professor);
        await _context.SaveChangesAsync();
    }

    public bool ProfessorExisteix(string idProfessor)
    {
        return _context.Professors.Any(p => p.IdProfessor == idProfessor);
    }

    public async Task SincronitzarIdentityAsync()
{
    var professors = await _context.Professors.ToListAsync();

    foreach (var professor in professors)
    {
            ProfessorDTO dto = new ProfessorDTO
            {
                Cognoms = professor.Cognoms,
                Nom = professor.Nom,
                IdProfessor = professor.IdProfessor
            };
            string password = FuncionsAuxiliars.ConstruirPasswordProfessor(dto);
            
            await _accountService.EnsureUserWithRoleAsync(
                professor.IdProfessor,
                professor.Email,
                "AG_Professor",
                password);

            if (professor.PertanyAEquipDirectiu)
            {
                await _accountService.EnsureUserWithRoleAsync(
                    professor.IdProfessor,
                    professor.Email,
                    "AG_EquipDirectiu",
                    password);
            }

        if (professor.Actiu)
        {
            await _accountService.ReactivateUserAsync(professor.IdProfessor);
        }
    }
}
}