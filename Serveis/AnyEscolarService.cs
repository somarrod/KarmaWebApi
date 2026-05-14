using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.DisplaySets;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Serveis
{

    public class AnyEscolarService : IAnyEscolarService
    {
        private readonly DatabaseContext _context;
        private readonly IAvaluacioService _avaluacioService;
        private readonly IKarmaAlumneService _karmaAlumneService;

        public AnyEscolarService(DatabaseContext context, IAvaluacioService avaluacioService, IKarmaAlumneService karmaAlumneService)
        {
            _context = context;
            _avaluacioService = avaluacioService;
            _karmaAlumneService = karmaAlumneService;
        }

        public async Task<AnyEscolarDisplaySet> CrearAnyEscolarAsync(AnyEscolarCrearDTO anyEscolarDto)
        {
            if (anyEscolarDto.DataIniciCurs >= anyEscolarDto.DataFiCurs)
                throw new InvalidOperationException(
                    "La data d'inici del curs ha de ser anterior a la data de finalització.");

            int idAnyEscolar =
                int.Parse(
                    (anyEscolarDto.DataIniciCurs.Year - 2000).ToString() +
                    (anyEscolarDto.DataFiCurs.Year - 2000).ToString()
                );

            bool existeix = await _context.AnyEscolars
                .AnyAsync(a => a.IdAnyEscolar == idAnyEscolar);

            if (existeix)
                throw new InvalidOperationException(
                    $"Ja existeix un any escolar amb l'identificador {idAnyEscolar}.");

            bool solapat = await _context.AnyEscolars.AnyAsync(a =>
                anyEscolarDto.DataIniciCurs <= a.DataFiCurs &&
                anyEscolarDto.DataFiCurs >= a.DataIniciCurs);

            if (solapat)
                throw new InvalidOperationException(
                    "Les dates de l'any escolar se solapen amb un altre any escolar existent.");

            var anyEscolar = new AnyEscolar
            {
                IdAnyEscolar = idAnyEscolar,
                DataIniciCurs = anyEscolarDto.DataIniciCurs,
                DataFiCurs = anyEscolarDto.DataFiCurs,
                SaldoKarmaInicial = anyEscolarDto.SaldoKarmaInicial,
                ReiniciaCadaAvaluacio = anyEscolarDto.ReiniciaCadaAvaluacio,
                Actiu = true
            };

            _context.AnyEscolars.Add(anyEscolar);
            await _context.SaveChangesAsync();

            return new AnyEscolarDisplaySet
            {
                IdAnyEscolar = anyEscolar.IdAnyEscolar,
                DataIniciCurs = anyEscolar.DataIniciCurs,
                DataFiCurs = anyEscolar.DataFiCurs,
                SaldoKarmaInicial = anyEscolar.SaldoKarmaInicial,
                ReiniciaCadaAvaluacio = anyEscolar.ReiniciaCadaAvaluacio,
                Actiu = anyEscolar.Actiu
            };
        }

        //public async Task<AnyEscolar> EditarAnyEscolarAsync(AnyEscolarEditarDTO dto)
        //{
        //    var anyEscolar = await _context.AnyEscolars
        //        .FirstOrDefaultAsync(a => a.IdAnyEscolar == dto.IdAnyEscolar);

        //    if (anyEscolar == null)
        //        throw new InvalidOperationException("L'any escolar no existeix.");

        //    // ✅ Guardem valors antics per a comparar
        //    bool canviSaldo = anyEscolar.SaldoKarmaInicial != dto.SaldoKarmaInicial;
        //    bool reiniciaCadaAvaluacio = dto.ReiniciaCadaAvaluacio;

        //    // ✅ Actualitzar dades de l'any escolar
        //    anyEscolar.SaldoKarmaInicial = dto.SaldoKarmaInicial;
        //    anyEscolar.ReiniciaCadaAvaluacio = dto.ReiniciaCadaAvaluacio;
        //    anyEscolar.Actiu = dto.Actiu;

        //    // ✅ 3. Actualitzar karma només si cal
        //    if (canviSaldo && reiniciaCadaAvaluacio)
        //    {
        //        DateOnly avui = DateOnly.FromDateTime(DateTime.Today);


        //        List<long> avaluacionsFutures = await _context.Avaluacions
        //            .Where(a =>
        //                a.IdAnyEscolar == dto.IdAnyEscolar &&
        //                a.DataInicial > avui)
        //            .Select(a => a.IdAvaluacio)
        //            .ToListAsync();

        //        if (avaluacionsFutures.Any())
        //        {
        //            List<KarmaAlumne> karmes = await _context.KarmaAlumnes
        //                .Where(k => avaluacionsFutures.Contains(k.IdAvaluacio))
        //                .ToListAsync();

        //            foreach (var karma in karmes)
        //            {
        //                karma.NumPuntsInicials = dto.SaldoKarmaInicial;
        //                //PENDENT ACTUALITZAR EL COLOR

        //                // ✅ Recalcular color amb la lògica REAL del sistema
        //                karma.KarmaInicial = await _karmaAlumneService
        //                    .ObtenirKarmaPerPuntsAsync(
        //                        dto.IdAnyEscolar,
        //                        dto.SaldoKarmaInicial);

        //                karma.NumPuntsActuals = dto.SaldoKarmaInicial;
        //                //PENDENT ACTUALITZAR EL COLOR

        //                // ✅ Recalcular color amb la lògica REAL del sistema
        //                karma.KarmaActual = await _karmaAlumneService
        //                    .ObtenirKarmaPerPuntsAsync(
        //                        dto.IdAnyEscolar,
        //                        dto.SaldoKarmaInicial);
        //            }
        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //    return anyEscolar;
        //}
        public async Task<AnyEscolarDisplaySet> EditarAnyEscolarAsync(AnyEscolarEditarDTO dto)
        {
            var anyEscolar = await _context.AnyEscolars
                .FirstOrDefaultAsync(a => a.IdAnyEscolar == dto.IdAnyEscolar);

            if (anyEscolar == null)
                throw new InvalidOperationException("L'any escolar no existeix.");

            bool canviSaldo = anyEscolar.SaldoKarmaInicial != dto.SaldoKarmaInicial;
            bool reiniciaCadaAvaluacio = dto.ReiniciaCadaAvaluacio;

            anyEscolar.SaldoKarmaInicial = dto.SaldoKarmaInicial;
            anyEscolar.ReiniciaCadaAvaluacio = dto.ReiniciaCadaAvaluacio;
            anyEscolar.Actiu = dto.Actiu;

            if (canviSaldo && reiniciaCadaAvaluacio)
            {
                DateOnly avui = DateOnly.FromDateTime(DateTime.Today);

                List<long> avaluacionsFutures = await _context.Avaluacions
                    .Where(a =>
                        a.IdAnyEscolar == dto.IdAnyEscolar &&
                        a.DataInicial > avui)
                    .Select(a => a.IdAvaluacio)
                    .ToListAsync();

                if (avaluacionsFutures.Any())
                {
                    List<KarmaAlumne> karmes = await _context.KarmaAlumnes
                        .Where(k => avaluacionsFutures.Contains(k.IdAvaluacio))
                        .ToListAsync();

                    foreach (var karma in karmes)
                    {
                        karma.NumPuntsInicials = dto.SaldoKarmaInicial;

                        karma.KarmaInicial = await _karmaAlumneService
                            .ObtenirKarmaPerPuntsAsync(
                                dto.IdAnyEscolar,
                                dto.SaldoKarmaInicial);

                        karma.NumPuntsActuals = dto.SaldoKarmaInicial;

                        karma.KarmaActual = await _karmaAlumneService
                            .ObtenirKarmaPerPuntsAsync(
                                dto.IdAnyEscolar,
                                dto.SaldoKarmaInicial);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return new AnyEscolarDisplaySet
            {
                IdAnyEscolar = anyEscolar.IdAnyEscolar,
                DataIniciCurs = anyEscolar.DataIniciCurs,
                DataFiCurs = anyEscolar.DataFiCurs,
                SaldoKarmaInicial = anyEscolar.SaldoKarmaInicial,
                ReiniciaCadaAvaluacio = anyEscolar.ReiniciaCadaAvaluacio,
                Actiu = anyEscolar.Actiu
            };
        }

        public async Task<bool> EliminarAnyEscolarAsync(int idAnyEscolar)
        {
            // ===============================
            // Buscar any escolar
            // ===============================
            var any = await _context.AnyEscolars
                .FirstOrDefaultAsync(a => a.IdAnyEscolar == idAnyEscolar);

            if (any == null)
                throw new InvalidOperationException("Any escolar no trobat");

            // ===============================
            // Validar que NO hi ha classes
            // ===============================
            bool teClasses = await _context.Classes
                .AnyAsync(c => c.IdAnyEscolar == idAnyEscolar);

            if (teClasses)
                throw new InvalidOperationException(
                    "No es pot eliminar l'any escolar perquè té classes associades");

            // ===============================
            // Validar que NO hi ha avaluacions
            // ===============================
            bool teAvaluacions = await _context.Avaluacions
                .AnyAsync(a => a.IdAnyEscolar == idAnyEscolar);

            if (teAvaluacions)
                throw new InvalidOperationException(
                    "No es pot eliminar l'any escolar perquè té avaluacions associades");

            // ===============================
            // Eliminar
            // ===============================
            _context.AnyEscolars.Remove(any);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(int idAnyEscolar)
        {
            return await _context.AnyEscolars
                .AnyAsync(a => a.IdAnyEscolar == idAnyEscolar);
        }


        public async Task<List<AnyEscolarDisplaySet>> GetLlistaAsync()
        {
            return await _context.AnyEscolars
                .AsNoTracking()
                .OrderByDescending(a => a.DataFiCurs)
                .Select(a => new AnyEscolarDisplaySet
                {
                    IdAnyEscolar = a.IdAnyEscolar,
                    DataIniciCurs = a.DataIniciCurs,
                    DataFiCurs = a.DataFiCurs,
                    SaldoKarmaInicial = a.SaldoKarmaInicial,
                    ReiniciaCadaAvaluacio = a.ReiniciaCadaAvaluacio,
                    Actiu = a.Actiu
                })
                .ToListAsync();
        }

        public async Task CopiarConfiguracioAsync(int idAnyOrigen, int idAnyDesti)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // ===============================
                // VALIDAR ANYS
                // ===============================
                bool origenExisteix = await _context.AnyEscolars
                    .AnyAsync(a => a.IdAnyEscolar == idAnyOrigen);

                bool destiExisteix = await _context.AnyEscolars
                    .AnyAsync(a => a.IdAnyEscolar == idAnyDesti);

                if (!origenExisteix || !destiExisteix)
                    throw new InvalidOperationException("Any escolar origen o destí no existeix");

                // ===============================
                // VALIDAR QUE DESTÍ ESTÀ BUIT
                // ===============================
                bool tePrivilegis = await _context.Privilegis
                    .AnyAsync(p => p.IdAnyEscolar == idAnyDesti);

                bool teClasses = await _context.Classes
                    .AnyAsync(c => c.IdAnyEscolar == idAnyDesti);

                bool teConfiguracio = await _context.ConfiguracionsKarma
                    .AnyAsync(c => c.IdAnyEscolar == idAnyDesti);

                if (tePrivilegis || teClasses || teConfiguracio)
                    throw new InvalidOperationException(
                        "L'any escolar destí ja té informació i no es pot sobreescriure");

                // ==================================================
                // 1. COPIAR PRIVILEGIS
                // ==================================================
                var privilegisOrigen = await _context.Privilegis
                    .Where(p => p.IdAnyEscolar == idAnyOrigen)
                    .ToListAsync();

                foreach (var p in privilegisOrigen)
                {
                    _context.Privilegis.Add(new Privilegi
                    {
                        Descripcio = p.Descripcio,
                        Tipus = p.Tipus,
                        IdAnyEscolar = idAnyDesti,
                        NivellPrivilegi = p.NivellPrivilegi,
                        Actiu = p.Actiu
                    });
                }

                // ==================================================
                // 2. COPIAR CONFIGURACIÓ KARMA
                // ==================================================
                var configuracionsOrigen = await _context.ConfiguracionsKarma
                    .Where(c => c.IdAnyEscolar == idAnyOrigen)
                    .ToListAsync();

                foreach (var c in configuracionsOrigen)
                {
                    _context.ConfiguracionsKarma.Add(new ConfiguracioKarma
                    {
                        IdAnyEscolar = idAnyDesti,
                        NumPuntsMinim = c.NumPuntsMinim,
                        NumPuntsMaxim = c.NumPuntsMaxim,
                        ColorKarma = c.ColorKarma,
                        NivellPrivilegis = c.NivellPrivilegis
                    });
                }

                // ==================================================
                // 3. COPIAR CLASSES
                // ==================================================
                var classesOrigen = await _context.Classes
                    .Where(c => c.IdAnyEscolar == idAnyOrigen)
                    .ToListAsync();

                foreach (var c in classesOrigen)
                {
                    _context.Classes.Add(new Classe
                    {
                        Nom = c.Nom,
                        IdAnyEscolar = idAnyDesti
                    });
                }

                // ==================================================
                // GUARDAR + COMMIT
                // ==================================================
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                // ===============================
                // ROLLBACK SI ALGUNA COSA FALLA
                // ===============================
                await transaction.RollbackAsync();
                throw;
            }
        }

    }

}
