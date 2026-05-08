using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.EntityFrameworkCore;

public class PrivilegiService : IPrivilegiService
{
    private readonly DatabaseContext _context;

    public PrivilegiService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Privilegi> CrearAsync(PrivilegiCrearDTO privilegi)
    {
        var entitat = new Privilegi
        {
            Descripcio = privilegi.Descripcio,
            Tipus = privilegi.Tipus,
            IdAnyEscolar = privilegi.IdAnyEscolar,
            NivellPrivilegi = privilegi.NivellPrivilegi,
            Actiu = true
        };

        _context.Privilegis.Add(entitat);
        await _context.SaveChangesAsync();

        return entitat;
    }

    public async Task<Privilegi> EditarAsync(PrivilegiEditarDTO privilegi)
    {
        var existent = await _context.Privilegis
            .FirstOrDefaultAsync(p => p.IdPrivilegi == privilegi.IdPrivilegi)
            ?? throw new InvalidOperationException("Privilegi no trobat");

        existent.Tipus = privilegi.Tipus;
        existent.Descripcio = privilegi.Descripcio;
        existent.NivellPrivilegi = privilegi.NivellPrivilegi;
        existent.Actiu = privilegi.Actiu;

        await _context.SaveChangesAsync();

        return existent; // ✅ objecte modificat
    }

    public async Task<bool> EliminarAsync(long idPrivilegi)
    {
        var privilegi = await _context.Privilegis.FindAsync(idPrivilegi);
        if (privilegi == null) return false;

        _context.Privilegis.Remove(privilegi);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Privilegi?> InstanciaAsync(long idPrivilegi)
    {
        return await _context.Privilegis
            .FirstOrDefaultAsync(p => p.IdPrivilegi == idPrivilegi);
    }

    public async Task<List<Privilegi>> LlistaPerAnyEscolarAsync(long idAnyEscolar)
    {
        return await _context.Privilegis
            .Where(p => p.IdAnyEscolar == idAnyEscolar)
            .OrderBy(p => p.NivellPrivilegi)
            .ToListAsync();
    }
}
