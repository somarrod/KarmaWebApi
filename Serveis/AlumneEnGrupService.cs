using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
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
    }

}
