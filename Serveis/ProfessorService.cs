
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProfessorService: IProfessorService
{
    private readonly DatabaseContext _context;

    public ProfessorService(DatabaseContext context)
    {
        _context = context;
    }

    public List<Professor> GetProfessors()
    {
        return _context.Professor.ToList();
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

        _context.Professor.Add(professor);
        await _context.SaveChangesAsync();

        return new OkObjectResult(professor);
    }

    public async Task<ActionResult<Professor>> ActivarProfessorAsync(String idProfessor)
    {
        var professor = await _context.Professor.FindAsync(idProfessor);

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
        var professor = await _context.Professor.FindAsync(idProfessor);

        if (professor == null)
        {
            return new NotFoundResult();
        }

        professor.Actiu = false;

        _context.Entry(professor).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return new OkResult();
    }

    public bool ProfessorExisteix(string idProfessor)
    {
        return _context.Professor.Any(e => e.IdProfessor == idProfessor);
    }

   
}
