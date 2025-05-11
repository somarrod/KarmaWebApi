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

        public AlumneEnGrupService(DatabaseContext context)
        {
            _context = context;
        }

        public ICollection<VPrivilegiPeriode> GetPrivilegisPeriode(int idAlumneEnGrup)
        {
            return _context.VPrivilegiPeriode
                            .Where(v => idAlumneEnGrup == v.IdAlumneEnGrup)
                            .ToList();
        }

        public async Task<ActionResult<AlumneEnGrup>> EditPuntuacioAsync(int idAlumneEnGrup, int punts)
        {
            var alumneEnGrup = await _context.AlumneEnGrup.FindAsync(idAlumneEnGrup);
            if (alumneEnGrup == null)
            {
                throw new InvalidOperationException($"Alumne en grup amb Id {idAlumneEnGrup} no trobat");
            }
            alumneEnGrup.PuntuacioTotal = punts;
            
            var confList = await _context.ConfiguracioKarma
                            .Where(c => c.IdAnyEscolar == alumneEnGrup.IdAnyEscolar &&
                                        punts >= c.KarmaMinim &&
                                        punts <= c.KarmaMaxim)
                            .ToListAsync();
            
            if (confList != null && confList.Count()>0) 
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
        }

    }

}
