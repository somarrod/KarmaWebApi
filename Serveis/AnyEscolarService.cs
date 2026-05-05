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
        private readonly IPeriodeService _periodeService;

        public AnyEscolarService(DatabaseContext context, IPeriodeService periodeService)
        {
            _context = context;
            _periodeService = periodeService;
        }

        public async Task<AnyEscolar> CrearAnyEscolarAsync(AnyEscolarCrearDto anyEscolarDto)
        {

            var anyEscolar = new AnyEscolar
            {
                IdAnyEscolar = int.Parse((anyEscolarDto.DataIniciCurs.Year -2000).ToString() + (anyEscolarDto.DataFiCurs.Year -2000).ToString()),
                DataIniciCurs = anyEscolarDto.DataIniciCurs,
                DataFiCurs = anyEscolarDto.DataFiCurs,
                SaldoKarmaInicial = anyEscolarDto.SaldoKarmaInicial,
                ReiniciaCadaAvaluacio = anyEscolarDto.ReiniciaCadaAvaluacio,
                Actiu = true
            };

            _context.AnyEscolar.Add(anyEscolar);
            await _context.SaveChangesAsync();

            return anyEscolar;
        }

        public async Task<AnyEscolar> EditarAnyEscolarAsync(AnyEscolarEditarDto dto)
        {
            var anyEscolar = await _context.AnyEscolar
                .FirstOrDefaultAsync(a => a.IdAnyEscolar == dto.IdAnyEscolar);

            if (anyEscolar == null)
                return null;

            anyEscolar.DataIniciCurs = dto.DataIniciCurs;
            anyEscolar.DataFiCurs = dto.DataFiCurs;
            anyEscolar.SaldoKarmaInicial = dto.SaldoKarmaInicial;
            anyEscolar.ReiniciaCadaAvaluacio = dto.ReiniciaCadaAvaluacio;
            anyEscolar.Actiu = dto.Actiu;

            await _context.SaveChangesAsync();

            return anyEscolar; // retorna objecte actualitzat
        }


        public async Task<bool> ExistsAsync(int idAnyEscolar)
        {
            return await _context.AnyEscolar
                .AnyAsync(a => a.IdAnyEscolar == idAnyEscolar);
        }


        public async Task<List<AnyEscolar>> GetLlistaAsync()
        {
            return await _context.AnyEscolar
                .AsNoTracking()
                .ToListAsync();
        }


        /*
        public async Task<IActionResult> ActualitzaKarmaAsync(int idAnyEscolar)
        {
            try
            {
                var configuracioKarmaList = await _context.ConfiguracioKarma
                    .Where(c => c.IdAnyEscolar == idAnyEscolar)
                    .ToListAsync();

                if (configuracioKarmaList != null)
                {
                    foreach (var configuracioKarma in configuracioKarmaList)
                    {
                        var alumnes = await _context.AlumneEnGrup
                           .Where(a => a.IdAnyEscolar == configuracioKarma.IdAnyEscolar &&
                                       a.PuntuacioTotal >= configuracioKarma.KarmaMinim &&
                                       a.PuntuacioTotal <= configuracioKarma.KarmaMaxim)
                           .ToListAsync();

                        alumnes.ForEach(a => a.Karma = configuracioKarma.ColorNivell);
                    }
                }
                await _context.SaveChangesAsync();
                return new OkResult(); // Use OkResult explicitly
            }
            catch (Exception ex)
            {
                throw new Exception("Error actualitzant el karma", ex);
            }
        }*/


    }

}
