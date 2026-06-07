using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs.Avaluacio;
using KarmaWebAPI.DTOs.DisplaySets;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.EntityFrameworkCore;

public class AvaluacioService : IAvaluacioService
{
    private readonly DatabaseContext _context;
    private readonly IKarmaAlumneService _karmaAlumneService;
    private readonly IGrupService _grupService;

    public AvaluacioService(
        DatabaseContext context,
        IKarmaAlumneService karmaAlumneService,
        IGrupService grupService)
    {
        _context = context;
        _karmaAlumneService = karmaAlumneService;
        _grupService = grupService;
    }

    // =====================================================
    // CONSULTES
    // =====================================================

    public async Task<List<AvaluacioDisplaySet>> GetLlistaAsync(bool isAdmin)
    {
        var query = _context.Avaluacions.AsQueryable();

        if (!isAdmin)
            query = query.Where(a => a.AnyEscolar.Actiu);

        return await query
            .OrderBy(a => a.Nom)
            .Select(a => new AvaluacioDisplaySet
            {
                IdAvaluacio = a.IdAvaluacio,
                Nom = a.Nom,
                DataInicial = a.DataInicial,
                DataFinal = a.DataFinal,
                NotaMinimaKarma = a.NotaMinimaKarma,
                NotaMaximaKarma = a.NotaMaximaKarma,
                IdAnyEscolar = a.IdAnyEscolar
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<AvaluacioDisplaySet>> GetLlistaPerAnyEscolarAsync(
        int idAnyEscolar,
        bool isAdmin)
    {
        var query = _context.Avaluacions
            .Where(a => a.IdAnyEscolar == idAnyEscolar);

        if (!isAdmin)
            query = query.Where(a => a.AnyEscolar.Actiu);

        return await query
            .OrderBy(a => a.Nom)
            .Select(a => new AvaluacioDisplaySet
            {
                IdAnyEscolar = a.IdAnyEscolar,
                IdAvaluacio = a.IdAvaluacio,
                Nom = a.Nom,
                DataInicial = a.DataInicial,
                DataFinal = a.DataFinal,
                NotaMinimaKarma = a.NotaMinimaKarma,
                NotaMaximaKarma = a.NotaMaximaKarma
                
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<AvaluacioDisplaySet> GetByIdAsync(long idAvaluacio)
    {
        var result = await _context.Avaluacions
            .Where(a => a.IdAvaluacio == idAvaluacio)
            .Select(a => new AvaluacioDisplaySet
            {
                IdAnyEscolar = a.IdAnyEscolar,
                IdAvaluacio = a.IdAvaluacio,
                Nom = a.Nom,
                DataInicial = a.DataInicial,
                DataFinal = a.DataFinal,
                NotaMinimaKarma = a.NotaMinimaKarma,
                NotaMaximaKarma = a.NotaMaximaKarma
                
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (result == null)
            throw new InvalidOperationException("L'avaluació no existeix");

        return result;
    }

    // =====================================================
    // TCREAR
    // =====================================================

    public async Task<AvaluacioDisplaySet> CrearAsync(AvaluacioCrearDTO dto)
    {
        using var tx = await _context.Database.BeginTransactionAsync();

        bool solapa = await _context.Avaluacions.AnyAsync(a =>
            a.IdAnyEscolar == dto.IdAnyEscolar &&
            dto.DataInicial <= a.DataFinal &&
            dto.DataFinal >= a.DataInicial);


        if (dto.DataFinal <= dto.DataInicial)
            throw new InvalidOperationException(
                "La data final ha de ser posterior a la data inicial");

        if (solapa)
            throw new InvalidOperationException("Hi ha solapament d'avaluacions");

        var anyEscolar = await _context.AnyEscolars
            .FirstOrDefaultAsync(a => a.IdAnyEscolar == dto.IdAnyEscolar);

        if (anyEscolar == null)
            throw new InvalidOperationException("L'any escolar no existeix");

        // ===============================
        // VALIDAR DATES DINS DE L'ANY ESCOLAR
        // ===============================
        if (dto.DataInicial < anyEscolar.DataIniciCurs ||
            dto.DataFinal > anyEscolar.DataFiCurs)
        {
            throw new InvalidOperationException(
                "Les dates de l'avaluació han d'estar dins de les dates de l'any escolar");
        }


        bool existeixMateixNom = await _context.Avaluacions
            .AnyAsync(a =>
                a.IdAnyEscolar == dto.IdAnyEscolar &&
                a.Nom == dto.Nom);

        if (existeixMateixNom)
            throw new InvalidOperationException(
                "Ja existeix una avaluació amb aquest nom dins del mateix any escolar");

        var avaluacio = new Avaluacio
        {
            Nom = dto.Nom,
            DataInicial = dto.DataInicial,
            DataFinal = dto.DataFinal,
            NotaMinimaKarma = dto.NotaMinimaKarma,
            NotaMaximaKarma = dto.NotaMaximaKarma,
            IdAnyEscolar = dto.IdAnyEscolar
        };

        _context.Avaluacions.Add(avaluacio);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();

        return await GetByIdAsync(avaluacio.IdAvaluacio);
    }

    // =====================================================
    // TEDITAR
    // =====================================================
    public async Task<AvaluacioDisplaySet> EditarAsync(AvaluacioEditarDTO dto)
    {
        using var tx = await _context.Database.BeginTransactionAsync();

        //Validacions de les dades
        var avaluacio = await _context.Avaluacions
            .FirstOrDefaultAsync(a => a.IdAvaluacio == dto.IdAvaluacio);
        if (avaluacio == null)
            throw new InvalidOperationException("L'avaluació no existeix");

        if (dto.DataFinal <= dto.DataInicial)
            throw new InvalidOperationException(
                "La data final ha de ser posterior a la data inicial");

        bool solapa = await _context.Avaluacions.AnyAsync(a =>
            a.IdAnyEscolar == avaluacio.IdAnyEscolar &&
            a.IdAvaluacio != dto.IdAvaluacio &&
            dto.DataInicial <= a.DataFinal &&
            dto.DataFinal >= a.DataInicial);
        if (solapa)
            throw new InvalidOperationException("Hi ha solapament d'avaluacions");


        var anyEscolar = await _context.AnyEscolars
            .FirstOrDefaultAsync(a => a.IdAnyEscolar == avaluacio.IdAnyEscolar);
        if (anyEscolar == null)
            throw new InvalidOperationException("L'any escolar no existeix");

        // ===============================
        // VALIDAR DATES DINS DE L'ANY ESCOLAR
        // ===============================
        if (dto.DataInicial < anyEscolar.DataIniciCurs ||
            dto.DataFinal > anyEscolar.DataFiCurs)
        {
            throw new InvalidOperationException(
                "Les dates de l'avaluació han d'estar dins de les dates de l'any escolar");
        }

         bool existeixMateixNom = await _context.Avaluacions
                .AnyAsync(a =>
                    a.IdAnyEscolar == avaluacio.IdAnyEscolar &&
                    a.IdAvaluacio != dto.IdAvaluacio &&
                    a.Nom == dto.Nom);
        if (existeixMateixNom)
            throw new InvalidOperationException(
                "Ja existeix una altra avaluació amb aquest nom dins del mateix any escolar");

        //Fi - Validacions de les dades

        // Actualització de les dades
        avaluacio.Nom = dto.Nom;
        avaluacio.DataInicial = dto.DataInicial;
        avaluacio.DataFinal = dto.DataFinal;
        avaluacio.NotaMinimaKarma = dto.NotaMinimaKarma;
        avaluacio.NotaMaximaKarma = dto.NotaMaximaKarma;
        // Actualització de les dades
        await _context.SaveChangesAsync();
        await tx.CommitAsync();

        return await GetByIdAsync(avaluacio.IdAvaluacio);
    }

    // =====================================================
    // TINICIAR / FINALITZAR
    // =====================================================


    public async Task<AvaluacioDisplaySet> IniciarAsync(long idAvaluacio)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = await IniciarCoreAsync(idAvaluacio);

                // UN ÚNIC SAVE
                await _context.SaveChangesAsync();

                await tx.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();

                throw new InvalidOperationException(
                    ex.InnerException?.Message ?? ex.Message);
            }
        });
    }

    public async Task<AvaluacioDisplaySet> IniciarCoreAsync(long idAvaluacio)
    {
        var avaluacio = await _context.Avaluacions
            .Include(a => a.AnyEscolar)
            .FirstOrDefaultAsync(a => a.IdAvaluacio == idAvaluacio)
            ?? throw new InvalidOperationException("L'avaluació no existeix");

        var anyEscolar = avaluacio.AnyEscolar;

        var avaluacioAnterior = await _context.Avaluacions
            .Where(a =>
                a.IdAnyEscolar == avaluacio.IdAnyEscolar &&
                a.DataFinal < avaluacio.DataInicial)
            .OrderByDescending(a => a.DataFinal)
            .FirstOrDefaultAsync();

        if (avaluacioAnterior == null || anyEscolar.ReiniciaCadaAvaluacio)
        {
            await _karmaAlumneService.CrearPerAvaluacioCoreAsync(
                avaluacio.IdAvaluacio,
                anyEscolar.SaldoKarmaInicial);
        }
        else
        {
            await _karmaAlumneService.CopiarPerAvaluacioCoreAsync(
                avaluacio.IdAvaluacio,
                avaluacioAnterior.IdAvaluacio);
        }

        await _context.SaveChangesAsync();

        // OBTENIR GRUPS DEL CURS ESCOLAR
        var grups = await _context.Grups
            .Where(g => g.Classe.IdAnyEscolar == avaluacio.IdAnyEscolar)
            .ToListAsync();

        // RECALCULAR KARMA BASE
        foreach (var grup in grups)
        {
            await _grupService.CalcularKarmaBaseCoreAsync(
                grup.IdGrup,
                idAvaluacio,
                saveChanges: false);
        }

        // NO SaveChanges ací
        return await GetByIdAsync(idAvaluacio);
    }

    public async Task<AvaluacioDisplaySet> FinalitzarAsync(long idAvaluacio)
    {
        var existeix = await _context.Avaluacions
            .AnyAsync(a => a.IdAvaluacio == idAvaluacio);

        if (!existeix)
            throw new InvalidOperationException("L'avaluació no existeix");

        await _karmaAlumneService.CalcularNotaFinalAsync(idAvaluacio);

        return await GetByIdAsync(idAvaluacio);
    }

    // =====================================================
    // ESBORRAR
    // =====================================================

    public async Task<bool> EsborrarAsync(long idAvaluacio)
    {
        var avaluacio = await _context.Avaluacions
            .FirstOrDefaultAsync(a => a.IdAvaluacio == idAvaluacio);

        if (avaluacio == null)
            throw new InvalidOperationException("L'avaluació no existeix");

        _context.Avaluacions.Remove(avaluacio);
        await _context.SaveChangesAsync();

        return true;
    }
}