using KarmaWebAPI.Data;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;

public class KarmaAlumneService : IKarmaAlumneService
{
    private readonly DatabaseContext _context;

    public KarmaAlumneService(DatabaseContext context)
    {
        _context = context;
    }

    // =====================================================
    // A) OPERACIONS PER AVALUACIÓ
    // =====================================================

    // Crear KarmaAlumne per a TOTS els alumnes en una avaluació
    public async Task CrearPerAvaluacioAsync(long idAvaluacio, double puntsInicials)
    {
        var alumnes = await _context.Alumnes
            .Where(a => a.Actiu)
            .ToListAsync();

        foreach (var alumne in alumnes)
        {
            bool existeix = await _context.KarmaAlumnes.AnyAsync(k =>
                k.NIA == alumne.NIA &&
                k.IdAvaluacio == idAvaluacio);

            if (existeix)
                continue;

            _context.KarmaAlumnes.Add(new KarmaAlumne
            {
                NIA = alumne.NIA,
                IdAvaluacio = idAvaluacio,
                NumPuntsInicials = puntsInicials,
                NumPuntsActuals = puntsInicials,
                KarmaInicial = "",
                KarmaActual = ""
            });
        }

        await _context.SaveChangesAsync();
    }

    // Copiar KarmaAlumne des de l'avaluació anterior (NO reinicia)
    public async Task CopiarPerAvaluacioAsync(
        long idAvaluacioActual,
        long idAvaluacioAnterior)
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
                NIA  = karmaAnterior.NIA,
                IdAvaluacio = idAvaluacioActual,
                NumPuntsInicials = karmaAnterior.NumPuntsActuals,
                NumPuntsActuals = karmaAnterior.NumPuntsActuals,
                KarmaInicial = karmaAnterior.KarmaActual,
                KarmaActual = karmaAnterior.KarmaActual
            });
        }

        await _context.SaveChangesAsync();
    }

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

    // Alumne nou → UNA avaluació concreta
    public async Task CrearPerAlumneIAvaluacioAsync(
        string nia,
        long idAvaluacio,
        double puntsInicials)
    {
        bool existeix = await _context.KarmaAlumnes.AnyAsync(k =>
            k.NIA == nia  &&
            k.IdAvaluacio == idAvaluacio);

        if (existeix)
            return;

        _context.KarmaAlumnes.Add(new KarmaAlumne
        {
            NIA = nia,
            IdAvaluacio = idAvaluacio,
            NumPuntsInicials = puntsInicials,
            NumPuntsActuals = puntsInicials,
            KarmaInicial = "",
            KarmaActual = ""
        });

        await _context.SaveChangesAsync();
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

            _context.KarmaAlumnes.Add(new KarmaAlumne
            {
                NIA = nia,
                IdAvaluacio = avaluacio.IdAvaluacio,
                NumPuntsInicials = puntsInicials,
                NumPuntsActuals = puntsInicials,
                KarmaInicial = "",
                KarmaActual = ""
            });
        }

        await _context.SaveChangesAsync();
    }
}