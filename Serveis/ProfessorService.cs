
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProfessorService: IProfessorService
{
    private readonly DatabaseContext _context;
    private readonly UserManager<ApiUser> _userManager;
    public ProfessorService(DatabaseContext context, UserManager<ApiUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public List<Professor> GetProfessors()
    {
        return _context.Professors.ToList();
    }


    public async Task<ActionResult<Professor>> CrearProfessorAsync(ProfessorDTO professorDto)
    {
        var professor = new Professor
        {
            IdProfessor= professorDto.IdProfessor,
            Nom = professorDto.Nom,
            Cognoms = professorDto.Cognoms,
            Actiu = true,
            Email = professorDto.Email
        };

        _context.Professors.Add(professor);
        await _context.SaveChangesAsync();

        return new OkObjectResult(professor);
    }

    public async Task<ActionResult<Professor>> ActivarProfessorAsync(String idProfessor)
    {
        var professor = await _context.Professors.FindAsync(idProfessor);

        if (professor == null)
        {
            return new NotFoundResult();
        }

        professor.Actiu = true;

        _context.Entry(professor).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return new OkResult();
    }

    public async Task<ActionResult<Professor>> DesactivarProfessorAsync(String idProfessor)
    {
        var professor = await _context.Professors.FindAsync(idProfessor);

        if (professor == null)
        {
            return new NotFoundResult();
        }

        professor.Actiu = false;

        _context.Entry(professor).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return new OkResult();
    }


    public async Task PertanyEquipDirectiuAsync(string idProfessor, bool pertanyAEquipDirectiu)
    {
        var professor = await _context.Professors
            .FirstOrDefaultAsync(p => p.IdProfessor == idProfessor);

        if (professor == null)
            throw new InvalidOperationException("Professor no trobat");

        professor.PertanyAEquipDirectiu = pertanyAEquipDirectiu;

        var user = await _userManager.FindByNameAsync(idProfessor);
        if (user == null)
            throw new InvalidOperationException("Usuari identity no trobat");

        if (pertanyAEquipDirectiu)
        {
            if (!await _userManager.IsInRoleAsync(user, "AG_EquipDirectiu"))
                await _userManager.AddToRoleAsync(user, "AG_EquipDirectiu");
        }
        else
        {
            if (await _userManager.IsInRoleAsync(user, "AG_EquipDirectiu"))
                await _userManager.RemoveFromRoleAsync(user, "AG_EquipDirectiu");
        }

        await _context.SaveChangesAsync();
    }


    public bool ProfessorExisteix(string idProfessor)
    {
        return _context.Professors.Any(e => e.IdProfessor == idProfessor);
    }

   
}
