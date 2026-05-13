using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
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

        public async Task<AnyEscolar> CrearAnyEscolarAsync(AnyEscolarCrearDTO anyEscolarDto)
        {

            // 1. Validació de dates
            if (anyEscolarDto.DataIniciCurs >= anyEscolarDto.DataFiCurs)
                throw new InvalidOperationException(
                    "La data d'inici del curs ha de ser anterior a la data de finalització.");

            int idAnyEscolar =
                int.Parse(
                    (anyEscolarDto.DataIniciCurs.Year - 2000).ToString() +
                    (anyEscolarDto.DataFiCurs.Year - 2000).ToString()
                );

            // 2. Validar que no existisca ja l'any escolar
            bool existeix = await _context.AnyEscolars
                .AnyAsync(a => a.IdAnyEscolar == idAnyEscolar);

            if (existeix)
                throw new InvalidOperationException(
                    $"Ja existeix un any escolar amb l'identificador {idAnyEscolar}.");


            // 3. Validació de solapament de dates
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

            return anyEscolar;
        }

        public async Task<AnyEscolar> EditarAnyEscolarAsync(AnyEscolarEditarDTO dto)
        {
            var anyEscolar = await _context.AnyEscolars
                .FirstOrDefaultAsync(a => a.IdAnyEscolar == dto.IdAnyEscolar);

            if (anyEscolar == null)
                throw new InvalidOperationException("L'any escolar no existeix.");

            // ✅ Guardem valors antics per a comparar
            bool canviSaldo = anyEscolar.SaldoKarmaInicial != dto.SaldoKarmaInicial;
            bool reiniciaCadaAvaluacio = dto.ReiniciaCadaAvaluacio;

            // ✅ Actualitzar dades de l'any escolar
            anyEscolar.SaldoKarmaInicial = dto.SaldoKarmaInicial;
            anyEscolar.ReiniciaCadaAvaluacio = dto.ReiniciaCadaAvaluacio;
            anyEscolar.Actiu = dto.Actiu;

            // ✅ 3. Actualitzar karma només si cal
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
                        //PENDENT ACTUALITZAR EL COLOR

                        // ✅ Recalcular color amb la lògica REAL del sistema
                        karma.KarmaInicial = await _karmaAlumneService
                            .ObtenirKarmaPerPuntsAsync(
                                dto.IdAnyEscolar,
                                dto.SaldoKarmaInicial);

                        karma.NumPuntsActuals = dto.SaldoKarmaInicial;
                        //PENDENT ACTUALITZAR EL COLOR

                        // ✅ Recalcular color amb la lògica REAL del sistema
                        karma.KarmaActual = await _karmaAlumneService
                            .ObtenirKarmaPerPuntsAsync(
                                dto.IdAnyEscolar,
                                dto.SaldoKarmaInicial);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return anyEscolar;
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


        public async Task<List<AnyEscolar>> GetLlistaAsync()
        {
            return await _context.AnyEscolars
                .AsNoTracking()
                .OrderByDescending(a => a.DataFiCurs)
                .ToListAsync();
        }


    }

}
