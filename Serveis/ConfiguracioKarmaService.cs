using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
//using KarmaWebAPI.DTOs.ConfiguracioKarma;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;

public class ConfiguracioKarmaService : IConfiguracioKarmaService
{
    private readonly DatabaseContext _context;

    public ConfiguracioKarmaService(DatabaseContext context)
    {
        _context = context;
    }

    // -------------------------------------------------
    // GET per AnyEscolar
    // -------------------------------------------------
    public async Task<List<ConfiguracioKarma>> GetPerAnyEscolarAsync(int idAnyEscolar)
    {
        return await _context.ConfiguracionsKarma
            .Where(c => c.IdAnyEscolar == idAnyEscolar)
            .OrderBy(c => c.NumPuntsMinim)
            .AsNoTracking()
            .ToListAsync();
    }

    // -------------------------------------------------
    // CREAR
    // -------------------------------------------------
    public async Task<ConfiguracioKarma> CrearAsync(ConfiguracioKarmaCrearDTO dto)
    {
        if (dto.NumPuntsMinim >= dto.NumPuntsMaxim)
            throw new InvalidOperationException("Rang de punts incorrecte");

        await ComprovarNoSolapamentAsync(
            dto.IdAnyEscolar,
            dto.NumPuntsMinim,
            dto.NumPuntsMaxim);

        var configuracio = new ConfiguracioKarma
        {
            NumPuntsMinim = dto.NumPuntsMinim,
            NumPuntsMaxim = dto.NumPuntsMaxim,
            ColorKarma = dto.ColorKarma,
            NivellPrivilegis = dto.NivellPrivilegis,
            IdAnyEscolar = dto.IdAnyEscolar
        };

        _context.ConfiguracionsKarma.Add(configuracio);
        await _context.SaveChangesAsync();

        return configuracio;
    }

    // -------------------------------------------------
    // EDITAR
    // -------------------------------------------------
    public async Task<ConfiguracioKarma?> EditarAsync(ConfiguracioKarmaEditarDTO dto)
    {
        var configuracio = await _context.ConfiguracionsKarma
            .FirstOrDefaultAsync(c => c.IdConfiguracioKarma == dto.IdConfiguracioKarma);

        if (configuracio == null)
            return null;

        if (dto.NumPuntsMinim >= dto.NumPuntsMaxim)
            throw new InvalidOperationException("Rang de punts incorrecte");

        await ComprovarNoSolapamentAsync(
            configuracio.IdAnyEscolar,
            dto.NumPuntsMinim,
            dto.NumPuntsMaxim,
            configuracio.IdConfiguracioKarma);

        configuracio.NumPuntsMinim = dto.NumPuntsMinim;
        configuracio.NumPuntsMaxim = dto.NumPuntsMaxim;
        configuracio.ColorKarma = dto.ColorKarma;
        configuracio.NivellPrivilegis = dto.NivellPrivilegis;

        await _context.SaveChangesAsync();
        return configuracio;
    }

    // -------------------------------------------------
    // ESBORRAR
    // -------------------------------------------------
    public async Task<bool> EsborrarAsync(long idConfiguracioKarma)
    {
        var configuracio = await _context.ConfiguracionsKarma
            .FirstOrDefaultAsync(c => c.IdConfiguracioKarma == idConfiguracioKarma);

        if (configuracio == null)
            return false;

        _context.ConfiguracionsKarma.Remove(configuracio);
        await _context.SaveChangesAsync();
        return true;
    }

    // -------------------------------------------------
    // VALIDACIÓ FORTA (NO buits)
    // -------------------------------------------------
    public async Task ValidarConfiguracioCompletaAsync(int idAnyEscolar)
    {
        var configs = await _context.ConfiguracionsKarma
            .Where(c => c.IdAnyEscolar == idAnyEscolar)
            .OrderBy(c => c.NumPuntsMinim)
            .ToListAsync();

        if (!configs.Any())
            throw new InvalidOperationException(
                "No hi ha cap configuració de karma definida.");

        for (int i = 1; i < configs.Count; i++)
        {
            var anterior = configs[i - 1];
            var actual = configs[i];

            // rangs han de ser contigus: [a,b) [b,c)
            if (actual.NumPuntsMinim != anterior.NumPuntsMaxim)
            {
                throw new InvalidOperationException(
                    $"Hi ha un buit entre {anterior.NumPuntsMaxim} i {actual.NumPuntsMinim}. " +
                    "Els rangs de karma han de ser contigus.");
            }
        }
    }

    // -------------------------------------------------
    // PRIVATE: comprovació de solapaments [min,max)
    // -------------------------------------------------
    private async Task ComprovarNoSolapamentAsync(
        int idAnyEscolar,
        double min,
        double max,
        long? idConfiguracioActual = null)
    {
        bool solapa = await _context.ConfiguracionsKarma
            .Where(c =>
                c.IdAnyEscolar == idAnyEscolar &&
                (idConfiguracioActual == null ||
                 c.IdConfiguracioKarma != idConfiguracioActual))
            .AnyAsync(c =>
                min < c.NumPuntsMaxim &&
                max > c.NumPuntsMinim);

        if (solapa)
            throw new InvalidOperationException(
                "Els rangs de karma no poden solapar-se.");
    }
}