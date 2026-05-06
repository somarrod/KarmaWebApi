
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.Avaluacio;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.EntityFrameworkCore;

public class AvaluacioService: IAvaluacioService
{
    private readonly DatabaseContext _context;

    public AvaluacioService(DatabaseContext context)
    {
        _context = context;
    }

    public List<Avaluacio> GetAvaluacions()
    {
        return _context.Avaluacions.ToList();
    }

    
    //CONSULTES -----------------------------------------------------------------------------------

    // -------------------------------------------------
    // Llista general
    // -------------------------------------------------
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

    // -------------------------------------------------
    // Llista per AnyEscolar
    // -------------------------------------------------
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

    // -------------------------------------------------
    // Get per Id
    // -------------------------------------------------
    public async Task<Avaluacio?> GetByIdAsync(int idAvaluacio)
    {
        return await _context.Avaluacions
            .Include(a => a.AnyEscolar)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.IdAvaluacio == idAvaluacio);
    }


    //TRANSACCIONS -----------------------------------------------------------------------------------
    // ------------------------
    // TCREAR
    // ------------------------
    public async Task<Avaluacio> TCrearAsync(AvaluacioTCrearDTO dto)
    {
        using var tx = await _context.Database.BeginTransactionAsync();

        // 1. Comprovar solapaments
        bool solapa = await _context.Avaluacions.AnyAsync(a =>
            a.IdAnyEscolar == dto.IdAnyEscolar &&
            dto.DataInicial <= a.DataFinal &&
            dto.DataFinal >= a.DataInicial);

        if (solapa)
            throw new InvalidOperationException("S'ha produït un solapament entre avaluacions");

        //2. Comprova la nota
        if (dto.NotaMinimaKarma >= dto.NotaMaximaKarma) 
            throw new InvalidOperationException("La nota mínima ha de ser menor que la nota màxima");

        // 3. Crear avaluació
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

        // 4. Crear KarmaAlumne inicial per a cada alumne (TCREAR segons XMI)
        // → ací vindrà la crida a KarmaAlumne.TCREAR (ho deixem preparat)
        // PENDENT SOFIA

        await tx.CommitAsync();
        return avaluacio;
    }

    // ------------------------
    // TEDITAR
    // ------------------------
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
            throw new InvalidOperationException("S'ha produït un solapament entre avaluacions");

        //2. Comprova la nota
        if (dto.NotaMinimaKarma >= dto.NotaMaximaKarma)
            throw new InvalidOperationException("La nota mínima ha de ser menor que la nota màxima");

        avaluacio.Nom = dto.Nom;
        avaluacio.DataInicial = dto.DataInicial;
        avaluacio.DataFinal = dto.DataFinal;
        avaluacio.NotaMinimaKarma = dto.NotaMinimaKarma;
        avaluacio.NotaMaximaKarma = dto.NotaMaximaKarma;

        await _context.SaveChangesAsync();
        await tx.CommitAsync();

        return avaluacio;
    }

    // ------------------------
    // TINICIAR_AVALUACIO
    // ------------------------

    public async Task<Avaluacio?> TIniciarAsync(int idAvaluacio)
    {
        var avaluacio = await _context.Avaluacions
            .Include(a => a.AnyEscolar)
            .FirstOrDefaultAsync(a => a.IdAvaluacio == idAvaluacio);

        if (avaluacio == null)
            return null;

        // 🔹 ACÍ va tota la lògica real:
        // - Reiniciar karma
        // - O copiar de l'avaluació anterior
        // (KarmaAlumne, etc.)

        await _context.SaveChangesAsync();

        return avaluacio; // objecte resultant
    }


    // ------------------------
    // TFINALITZAR_AVALUACIO
    // ------------------------
    public async Task<Avaluacio?> TFinalitzarAsync(int idAvaluacio)
    {
        var avaluacio = await _context.Avaluacions
            .FirstOrDefaultAsync(a => a.IdAvaluacio == idAvaluacio);

        if (avaluacio == null)
            return null;

        // 🔹 Lògica:
        // - Calcul de notes finals
        // - KarmaAlumne.CalcularNota()

        await _context.SaveChangesAsync();

        return avaluacio; // objecte resultant
    }


    public async Task<bool> EsborrarAsync(int idAvaluacio)
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




