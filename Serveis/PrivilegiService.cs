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
        // ===============================
        // VALIDAR TIPUS
        // ===============================
        if (privilegi.Tipus != "I" && privilegi.Tipus != "G")
            throw new InvalidOperationException(
                "El tipus de privilegi només pot ser 'I' (individual) o 'G' (grup)");

        // ===============================
        // VALIDAR AnyEscolar
        // ===============================
        bool anyExisteix = await _context.AnyEscolars
            .AnyAsync(a => a.IdAnyEscolar == privilegi.IdAnyEscolar);

        if (!anyExisteix)
            throw new InvalidOperationException(
                "L'any escolar indicat no existeix");

        // ===============================
        // VALIDAR DUPLICAT (per tipus)
        // ===============================
        bool existeixDuplicat = await _context.Privilegis
            .AnyAsync(p =>
                p.Tipus == privilegi.Tipus &&
                p.Descripcio == privilegi.Descripcio);

        if (existeixDuplicat)
            throw new InvalidOperationException(
                "Ja existeix un privilegi amb aquesta descripció per al mateix tipus");

        // ===============================
        // CREAR
        // ===============================
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

    public async Task<Privilegi> EditarAsync(PrivilegiEditarDTO dto)
    {
        // ===============================
        // VALIDAR EXISTÈNCIA
        // ===============================
        var entitat = await _context.Privilegis
            .FirstOrDefaultAsync(p => p.IdPrivilegi == dto.IdPrivilegi);

        if (entitat == null)
            throw new InvalidOperationException("Privilegi no trobat");

        // ===============================
        // VALIDAR TIPUS
        // ===============================
        if (dto.Tipus != "I" && dto.Tipus != "G")
            throw new InvalidOperationException(
                "El tipus de privilegi només pot ser 'I' o 'G'");

        // ===============================
        // VALIDAR DUPLICAT
        // ===============================
        bool existeixDuplicat = await _context.Privilegis
            .AnyAsync(p =>
                p.IdPrivilegi != dto.IdPrivilegi &&
                p.Tipus == dto.Tipus &&
                p.Descripcio == dto.Descripcio);

        if (existeixDuplicat)
            throw new InvalidOperationException(
                "Ja existeix un privilegi amb aquesta descripció per al mateix tipus");

        // ===============================
        // ACTUALITZAR
        // ===============================
        entitat.Descripcio = dto.Descripcio;
        entitat.Tipus = dto.Tipus;
        entitat.NivellPrivilegi = dto.NivellPrivilegi;
        entitat.Actiu = dto.Actiu;

        await _context.SaveChangesAsync();

        return entitat;
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

    public async Task<List<Privilegi>> LlistaPerAnyEscolarAsync(int idAnyEscolar)
    {
        return await _context.Privilegis
            .Where(p => p.IdAnyEscolar == idAnyEscolar)
            .OrderBy(p => p.NivellPrivilegi)
            .ToListAsync();
    }
}
