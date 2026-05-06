using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs.Avaluacio;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;

public class AvaluacioService : IAvaluacioService
{
    private readonly DatabaseContext _context;
    private readonly IKarmaAlumneService _karmaAlumneService;

    public AvaluacioService(
        DatabaseContext context,
        IKarmaAlumneService karmaAlumneService)
    {
        _context = context;
        _karmaAlumneService = karmaAlumneService;
    }

    // =====================================================
    // CONSULTES
    // =====================================================

    public async Task<List<Avaluacio>> GetLlistaAsync(bool isAdmin)
    {
        if (isAdmin)
        {
            return await _context.Avaluacions
                .AsNoTracking()
                .ToListAsync();
        }

        return await _context.Avaluacions
            .Include(a => a.AnyEscolar)
            .Where(a => a.AnyEscolar.Actiu)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Avaluacio>> GetLlistaPerAnyEscolarAsync(
        int idAnyEscolar,
        bool isAdmin)
    {
        if (isAdmin)
        {
            return await _context.Avaluacions
                .Where(a => a.IdAnyEscolar == idAnyEscolar)
                .AsNoTracking()
                .ToListAsync();
        }

        return await _context.Avaluacions
            .Include(a => a.AnyEscolar)
            .Where(a =>
                a.IdAnyEscolar == idAnyEscolar &&
                a.AnyEscolar.Actiu)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Avaluacio?> GetByIdAsync(long idAvaluacio)
    {
        return await _context.Avaluacions
            .Include(a => a.AnyEscolar)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.IdAvaluacio == idAvaluacio);
    }

    // =====================================================
    // TCREAR
    // =====================================================

    public async Task<Avaluacio> TCrearAsync(AvaluacioTCrearDTO dto)
    {
        using var tx = await _context.Database.BeginTransactionAsync();

        // 🔎 comprovar solapaments
        bool solapa = await _context.Avaluacions.AnyAsync(a =>
            a.IdAnyEscolar == dto.IdAnyEscolar &&
            dto.DataInicial <= a.DataFinal &&
            dto.DataFinal >= a.DataInicial);

        if (solapa)
            throw new InvalidOperationException("Hi ha solapament d'avaluacions");

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

        // ❗ NO inicialitzem Karma ací
        // La inicialització real es fa en TINICIAR_AVALUACIO

        await tx.CommitAsync();
        return avaluacio;
    }

    // =====================================================
    // TEDITAR
    // =====================================================

    public async Task<Avaluacio?> TEditarAsync(AvaluacioTEditarDTO dto)
    {
        using var tx = await _context.Database.BeginTransactionAsync();

        var avaluacio = await _context.Avaluacions
            .FirstOrDefaultAsync(a => a.IdAvaluacio == dto.IdAvaluacio);

        if (avaluacio == null)
            return null;

        bool solapa = await _context.Avaluacions.AnyAsync(a =>
            a.IdAnyEscolar == avaluacio.IdAnyEscolar &&
            a.IdAvaluacio != dto.IdAvaluacio &&
            dto.DataInicial <= a.DataFinal &&
            dto.DataFinal >= a.DataInicial);

        if (solapa)
            throw new InvalidOperationException("Hi ha solapament d'avaluacions");

        avaluacio.Nom = dto.Nom;
        avaluacio.DataInicial = dto.DataInicial;
        avaluacio.DataFinal = dto.DataFinal;
        avaluacio.NotaMinimaKarma = dto.NotaMinimaKarma;
        avaluacio.NotaMaximaKarma = dto.NotaMaximaKarma;

        await _context.SaveChangesAsync();
        await tx.CommitAsync();

        return avaluacio;
    }

    // =====================================================
    // TINICIAR_AVALUACIO
    // =====================================================

    public async Task<Avaluacio?> TIniciarAsync(long idAvaluacio)
    {
        using var tx = await _context.Database.BeginTransactionAsync();

        var avaluacio = await _context.Avaluacions
            .Include(a => a.AnyEscolar)
            .FirstOrDefaultAsync(a => a.IdAvaluacio == idAvaluacio);

        if (avaluacio == null)
            return null;

        var anyEscolar = avaluacio.AnyEscolar;

        var avaluacioAnterior = await _context.Avaluacions
            .Where(a =>
                a.IdAnyEscolar == avaluacio.IdAnyEscolar &&
                a.DataFinal < avaluacio.DataInicial)
            .OrderByDescending(a => a.DataFinal)
            .FirstOrDefaultAsync();

        if (avaluacioAnterior == null)
        {
            // primera avaluació del curs
            await _karmaAlumneService.CrearPerAvaluacioAsync(
                avaluacio.IdAvaluacio,
                anyEscolar.SaldoKarmaInicial);
        }
        else
        {
            if (anyEscolar.ReiniciaCadaAvaluacio)
            {
                await _karmaAlumneService.CrearPerAvaluacioAsync(
                    avaluacio.IdAvaluacio,
                    anyEscolar.SaldoKarmaInicial);
            }
            else
            {
                await _karmaAlumneService.CopiarPerAvaluacioAsync(
                    avaluacio.IdAvaluacio,
                    avaluacioAnterior.IdAvaluacio);
            }
        }

        await tx.CommitAsync();
        return avaluacio;
    }

    // =====================================================
    // TFINALITZAR_AVALUACIO
    // =====================================================

    public async Task<Avaluacio?> TFinalitzarAsync(long idAvaluacio)
    {
        var avaluacio = await _context.Avaluacions
            .FirstOrDefaultAsync(a => a.IdAvaluacio == idAvaluacio);

        if (avaluacio == null)
            return null;

        await _karmaAlumneService.CalcularNotaFinalAsync(idAvaluacio);

        return avaluacio;
    }

    // =====================================================
    // ESBORRAR
    // =====================================================

    public async Task<bool> EsborrarAsync(long idAvaluacio)
    {
        var avaluacio = await _context.Avaluacions
            .FirstOrDefaultAsync(a => a.IdAvaluacio == idAvaluacio);

        if (avaluacio == null)
            return false;

        _context.Avaluacions.Remove(avaluacio);
        await _context.SaveChangesAsync();
        return true;
    }
}