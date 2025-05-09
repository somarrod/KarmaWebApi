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

        public async Task<ActionResult<AlumneEnGrup>> EditPuntuacioAsync(int idAlumneEnGrup, int Punts)
        {
            var alumneEnGrup = await _context.AlumneEnGrup.FindAsync(idAlumneEnGrup);
            if (alumneEnGrup == null)
            {
                throw new InvalidOperationException($"Alumne en grup amb Id {idAlumneEnGrup} no trobat");
            }
            alumneEnGrup.PuntuacioTotal = alumneEnGrup.PuntuacioTotal + Punts;
            _context.AlumneEnGrup.Update(alumneEnGrup);
            await _context.SaveChangesAsync();

            return new ActionResult<AlumneEnGrup>(alumneEnGrup); // Ensure a value is returned
        }


    }

}
