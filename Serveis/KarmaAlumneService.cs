using KarmaWebAPI.Data;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;

public class KarmaAlumneService : IKarmaAlumneService
{

    private readonly DatabaseContext _context;
    private readonly IConfiguracioKarmaService _configuracioKarmaService;

    public KarmaAlumneService(
        DatabaseContext context,
        IConfiguracioKarmaService configuracioKarmaService)
    {
        _context = context;
        _configuracioKarmaService = configuracioKarmaService;
    }



    // =====================================================
    // A) OPERACIONS PER AVALUACIÓ
    // =====================================================

    // Crear KarmaAlumne per a TOTS els alumnes en una avaluació
    public async Task CrearPerAvaluacioAsync(long idAvaluacio, double puntsInicials)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                await CrearPerAvaluacioCoreAsync(idAvaluacio, puntsInicials);
                await _context.SaveChangesAsync();

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        });
    }
    public async Task CrearPerAvaluacioCoreAsync(long idAvaluacio, double puntsInicials)
    {
        var avaluacio = await _context.Avaluacions
            .Include(a => a.AnyEscolar)
            .FirstOrDefaultAsync(a => a.IdAvaluacio == idAvaluacio)
            ?? throw new InvalidOperationException("Avaluació no trobada");

        // Filtrar alumnes del mateix curs escolar de l'avaluació
        var alumnes = await _context.Alumnes
            .Where(a =>
                a.Actiu &&
                a.IdClasse != null &&
                _context.Classes.Any(c =>
                    c.IdClasse == a.IdClasse &&
                    c.IdAnyEscolar == avaluacio.IdAnyEscolar))
            .ToListAsync();


        foreach (var alumne in alumnes)
        {
            bool existeix = await _context.KarmaAlumnes.AnyAsync(k =>
                k.NIA == alumne.NIA &&
                k.IdAvaluacio == idAvaluacio);

            if (existeix)
                continue;

            var karma = await ObtenirKarmaPerPuntsAsync(
                avaluacio.IdAnyEscolar,
                puntsInicials);

            _context.KarmaAlumnes.Add(new KarmaAlumne
            {
                NIA = alumne.NIA,
                IdAvaluacio = idAvaluacio,
                NumPuntsInicials = puntsInicials,
                NumPuntsActuals = puntsInicials,
                KarmaInicial = karma,
                KarmaActual = karma
            });
        }

        // ❌ IMPORTANT: NO SaveChanges
    }

    public async Task CopiarPerAvaluacioAsync(long idAvaluacioActual, long idAvaluacioAnterior)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                await CopiarPerAvaluacioCoreAsync(
                    idAvaluacioActual,
                    idAvaluacioAnterior);

                await _context.SaveChangesAsync();

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        });
    }

    public async Task CopiarPerAvaluacioCoreAsync(long idAvaluacioActual,long idAvaluacioAnterior)
    {
        var karmesAnteriors = await _context.KarmaAlumnes
            .Where(k => k.IdAvaluacio == idAvaluacioAnterior)
            .ToListAsync();

        foreach (var karmaAnterior in karmesAnteriors)
        {
            bool existeix = await _context.KarmaAlumnes.AnyAsync(k =>
                k.NIA == karmaAnterior.NIA &&
                k.IdAvaluacio == idAvaluacioActual);

            if (existeix)
                continue;

            _context.KarmaAlumnes.Add(new KarmaAlumne
            {
                NIA = karmaAnterior.NIA,
                IdAvaluacio = idAvaluacioActual,
                NumPuntsInicials = karmaAnterior.NumPuntsActuals,
                NumPuntsActuals = karmaAnterior.NumPuntsActuals,
                KarmaInicial = karmaAnterior.KarmaActual,
                KarmaActual = karmaAnterior.KarmaActual
            });
        }
    }

    // Copiar KarmaAlumne des de l'avaluació anterior (NO reinicia)
    //public async Task CopiarPerAvaluacioAsync(long idAvaluacioActual, long idAvaluacioAnterior)
    //{
    //    var karmesAnteriors = await _context.KarmaAlumnes
    //        .Where(k => k.IdAvaluacio == idAvaluacioAnterior)
    //        .ToListAsync();

    //    foreach (var karmaAnterior in karmesAnteriors)
    //    {
    //        bool existeix = await _context.KarmaAlumnes.AnyAsync(k =>
    //            k.NIA == karmaAnterior.NIA &&
    //            k.IdAvaluacio == idAvaluacioActual);

    //        if (existeix)
    //            continue;

    //        _context.KarmaAlumnes.Add(new KarmaAlumne
    //        {
    //            NIA  = karmaAnterior.NIA,
    //            IdAvaluacio = idAvaluacioActual,
    //            NumPuntsInicials = karmaAnterior.NumPuntsActuals,
    //            NumPuntsActuals = karmaAnterior.NumPuntsActuals,
    //            KarmaInicial = karmaAnterior.KarmaActual,
    //            KarmaActual = karmaAnterior.KarmaActual
    //        });
    //    }

    //    await _context.SaveChangesAsync();
    //}

    // Calcular nota final de TOTS els alumnes d'una avaluació
    public async Task CalcularNotaFinalAsync(long idAvaluacio)
    {
        var karmes = await _context.KarmaAlumnes
            .Include(k => k.Avaluacio)
            .Where(k => k.IdAvaluacio == idAvaluacio)
            .ToListAsync();

        foreach (var karma in karmes)
        {
            var avaluacio = karma.Avaluacio;


            karma.KarmaActual = await ObtenirKarmaPerPuntsAsync(
                karma.Avaluacio.IdAnyEscolar,
                karma.NumPuntsActuals);

            if (karma.NumPuntsActuals <= avaluacio.NotaMinimaKarma)
            {
                karma.NotaKarma = 0;
            }
            else if (karma.NumPuntsActuals >= avaluacio.NotaMaximaKarma)
            {
                karma.NotaKarma = 10;
            }
            else
            {
                karma.NotaKarma =
                    10 * (karma.NumPuntsActuals - avaluacio.NotaMinimaKarma) /
                    (avaluacio.NotaMaximaKarma - avaluacio.NotaMinimaKarma);
            }
        }

        await _context.SaveChangesAsync();
    }

    // =====================================================
    // B) OPERACIONS PER ALUMNE
    // =====================================================

    public async Task CrearPerAlumneAsync(
    string nia,
    long idAvaluacio,
    double puntsInicials)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                await CrearPerAlumneCoreAsync(nia, idAvaluacio, puntsInicials);
                await _context.SaveChangesAsync();

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        });
    }
    public async Task CrearPerAlumneCoreAsync(
                string nia,
                long idAvaluacio,
                double puntsInicials)
    {
        var alumne = await _context.Alumnes
            .FirstOrDefaultAsync(a => a.NIA == nia && a.Actiu)
            ?? throw new InvalidOperationException("Alumne no trobat o no actiu");

        var existeix = await _context.KarmaAlumnes
            .AnyAsync(k => k.NIA == nia && k.IdAvaluacio == idAvaluacio);

        if (existeix) return;

        var avaluacio = await _context.Avaluacions
            .Include(a => a.AnyEscolar)
            .FirstOrDefaultAsync(a => a.IdAvaluacio == idAvaluacio)
            ?? throw new InvalidOperationException("Avaluació no trobada");

        var karma = await ObtenirKarmaPerPuntsAsync(
            avaluacio.IdAnyEscolar,
            puntsInicials);

        _context.KarmaAlumnes.Add(new KarmaAlumne
        {
            NIA = nia,
            IdAvaluacio = idAvaluacio,
            NumPuntsInicials = puntsInicials,
            NumPuntsActuals = puntsInicials,
            KarmaInicial = karma,
            KarmaActual = karma
        });
    }

    public async Task<KarmaAlumne?> ObtenirKarmaAlumnePerDataAsync(string nia, DateOnly data)
    {
        // ✅ 1. Trobar avaluació en curs en eixa data
        var avaluacio = await _context.Avaluacions
            .Where(a =>
                a.DataInicial <= data &&
                a.DataFinal >= data)
            .FirstOrDefaultAsync();

        if (avaluacio == null)
            return null;

        // ✅ 2. Recuperar karma per eixa avaluació
        return await _context.KarmaAlumnes
            .FirstOrDefaultAsync(k =>
                k.NIA == nia &&
                k.IdAvaluacio == avaluacio.IdAvaluacio);
    }


    // Alumne nou → DES DE l’avaluació en curs fins a les futures
    public async Task CrearPerAlumneDesdeAvaluacioEnCursAsync(
        string nia,
        int idAnyEscolar)
    {
        var anyEscolar = await _context.AnyEscolars
            .FirstAsync(a => a.IdAnyEscolar == idAnyEscolar);

        var avaluacions = await _context.Avaluacions
            .Where(a => a.IdAnyEscolar == idAnyEscolar)
            .OrderBy(a => a.DataInicial)
            .ToListAsync();

        if (!avaluacions.Any())
            return;

        var hui = DateOnly.FromDateTime(DateTime.Now);

        int indexActual = avaluacions.FindIndex(a =>
            a.DataInicial <= hui && a.DataFinal >= hui);

        if (indexActual == -1)
            return;

        double puntsInicials = anyEscolar.SaldoKarmaInicial;

        for (int i = indexActual; i < avaluacions.Count; i++)
        {
            var avaluacio = avaluacions[i];

            bool existeix = await _context.KarmaAlumnes.AnyAsync(k =>
                k.NIA == nia &&
                k.IdAvaluacio == avaluacio.IdAvaluacio);

            if (existeix)
                continue;

            if (i == indexActual)
            {
                puntsInicials = anyEscolar.SaldoKarmaInicial;
            }
            else
            {
                if (anyEscolar.ReiniciaCadaAvaluacio)
                {
                    puntsInicials = anyEscolar.SaldoKarmaInicial;
                }
                else
                {
                    var karmaAnterior = await _context.KarmaAlumnes
                        .FirstOrDefaultAsync(k =>
                            k.NIA == nia &&
                            k.IdAvaluacio == avaluacions[i - 1].IdAvaluacio);

                    puntsInicials = karmaAnterior?.NumPuntsActuals
                                    ?? anyEscolar.SaldoKarmaInicial;
                }
            }


            var karma = await ObtenirKarmaPerPuntsAsync(
                avaluacio.IdAnyEscolar,
                puntsInicials);

            _context.KarmaAlumnes.Add(new KarmaAlumne
            {
                NIA = nia,
                IdAvaluacio = avaluacio.IdAvaluacio,
                NumPuntsInicials = puntsInicials,
                NumPuntsActuals = puntsInicials,
                KarmaInicial = karma,
                KarmaActual = karma
            });
            ;
        }

        await _context.SaveChangesAsync();
    }


    public async Task RecalcularKarmaAsync(long idKarmaAlumne)
    {
        var karmaAlumne = await _context.KarmaAlumnes
            .Include(k => k.Avaluacio)
            .FirstOrDefaultAsync(k => k.IdKarmaAlumne == idKarmaAlumne);

        if (karmaAlumne == null)
            return;

        var karma = await ObtenirKarmaPerPuntsAsync(
            karmaAlumne.Avaluacio.IdAnyEscolar,
            karmaAlumne.NumPuntsActuals);

        karmaAlumne.KarmaActual = karma;

        await _context.SaveChangesAsync();
    }

    //MÈTODE PRIVAT PER A OBTENIR EL KARMA EN FUNCIó DELS PUNTS
    public async Task<string> ObtenirKarmaPerPuntsAsync(
    int idAnyEscolar,
    double punts)
    {
        var configuracions = await _context.ConfiguracionsKarma
            .Where(c => c.IdAnyEscolar == idAnyEscolar)
            .OrderBy(c => c.NumPuntsMinim)
            .ToListAsync();

        var configuracio = configuracions
            .FirstOrDefault(c =>
                punts >= c.NumPuntsMinim &&
                punts < c.NumPuntsMaxim);

        return configuracio?.ColorKarma ?? string.Empty;
    }
}