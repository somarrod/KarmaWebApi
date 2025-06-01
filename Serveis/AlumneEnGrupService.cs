using Humanizer;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace KarmaWebAPI.Serveis
{

    public class AlumneEnGrupService : IAlumneEnGrupService
    {
        private readonly DatabaseContext _context;
        private readonly IGrupService _grup;

        public AlumneEnGrupService(DatabaseContext context, IGrupService grup)
        {
            _context = context;
            _grup = grup;
        }

        public ICollection<VPrivilegiPeriode> GetPrivilegisPeriode(int idAlumneEnGrup)
        {
            return _context.VPrivilegiPeriode
                            .Where(v => idAlumneEnGrup == v.IdAlumneEnGrup)
                            .ToList();
        }

        public async Task<ActionResult<AlumneEnGrup>> AfegirPuntuacioAsync(int idAlumneEnGrup, int punts)
        {
            var alumneEnGrup = await _context.AlumneEnGrup.FindAsync(idAlumneEnGrup);
            if (alumneEnGrup == null)
            {
                throw new InvalidOperationException($"Alumne en grup amb Id {idAlumneEnGrup} no trobat");
            }

            if (alumneEnGrup.PuntuacioTotal == null)
            {
                alumneEnGrup.PuntuacioTotal = 0; // Inicialitzar a 0 si és null
            }
            alumneEnGrup.PuntuacioTotal = alumneEnGrup.PuntuacioTotal + punts;

            var confList = await _context.ConfiguracioKarma
                            .Where(c => c.IdAnyEscolar == alumneEnGrup.IdAnyEscolar &&
                                        punts >= c.KarmaMinim &&
                                        punts <= c.KarmaMaxim)
                            .ToListAsync();

            if (confList != null && confList.Count() > 0)
            {
                foreach (var configuracioKarma in confList) //Sólo debe haber 1
                {
                    alumneEnGrup.Karma = configuracioKarma.ColorNivell;
                }
            }

            _context.AlumneEnGrup.Update(alumneEnGrup);
            await _context.SaveChangesAsync();

            return new ActionResult<AlumneEnGrup>(alumneEnGrup); // Ensure a value is returned
        }


        public async Task<ActionResult<AlumneEnGrup>> ResetPuntuacioTotalAsync(int idAlumneEnGrup, int puntuacioTotal)
        {
            var alumneEnGrup = await _context.AlumneEnGrup.FindAsync(idAlumneEnGrup);
            if (alumneEnGrup == null)
            {
                throw new InvalidOperationException($"Alumne en grup amb Id {idAlumneEnGrup} no trobat");
            }

            alumneEnGrup.PuntuacioTotal = puntuacioTotal;

            var confList = await _context.ConfiguracioKarma
                            .Where(c => c.IdAnyEscolar == alumneEnGrup.IdAnyEscolar &&
                                        puntuacioTotal >= c.KarmaMinim &&
                                        puntuacioTotal <= c.KarmaMaxim)
                            .ToListAsync();

            if (confList != null && confList.Count() > 0)
            {
                foreach (var configuracioKarma in confList) //Sólo debe haber 1
                {
                    alumneEnGrup.Karma = configuracioKarma.ColorNivell;
                }
            }

            _context.AlumneEnGrup.Update(alumneEnGrup);
            await _context.SaveChangesAsync();

            await _grup.calculaKarmaBaseAsync(alumneEnGrup.IdAnyEscolar, alumneEnGrup.IdGrup);


            return new ActionResult<AlumneEnGrup>(alumneEnGrup); // Ensure a value is returned
        }


    }

}
