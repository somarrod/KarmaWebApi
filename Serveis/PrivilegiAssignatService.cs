using KarmaWebAPI.Data;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Serveis
{
    public class PrivilegiAssignatService : IPrivilegiAssignatService
    {
        private readonly DatabaseContext _context;

        public PrivilegiAssignatService(DatabaseContext context)
        {
            _context = context;
        }

        // ==================================================
        // ASSIGNAR PRIVILEGI
        // ==================================================
        public async Task<List<PrivilegiAssignat>> AssignarAsync(string nia, long idPrivilegi)
        {
            var alumne = await _context.Alumnes
                .Include(a => a.Grup)
                .FirstOrDefaultAsync(a => a.NIA == nia)
                ?? throw new InvalidOperationException("Alumne no trobat");

            var privilegi = await _context.Privilegis
                .FirstOrDefaultAsync(p => p.IdPrivilegi == idPrivilegi && p.Actiu)
                ?? throw new InvalidOperationException("Privilegi no vàlid");

            var ara = DateTime.Now;
            var codiIntern = $"PA-{idPrivilegi}-{Guid.NewGuid().ToString("N")[..8]}";

            var assignats = new List<PrivilegiAssignat>();

            if (privilegi.Tipus == "I")
            {
                assignats.Add(CrearAssignacio(alumne.NIA, privilegi, codiIntern, ara));
            }
            else if (privilegi.Tipus == "G")
            {
                if (alumne.IdGrup == null)
                    throw new InvalidOperationException("L'alumne no té grup");

                var alumnesGrup = await _context.Alumnes
                    .Where(a => a.IdGrup == alumne.IdGrup)
                    .ToListAsync();

                foreach (var a in alumnesGrup)
                {
                    assignats.Add(CrearAssignacio(a.NIA, privilegi, codiIntern, ara));
                }
            }
            else
            {
                throw new InvalidOperationException("Tipus de privilegi desconegut");
            }

            _context.PrivilegisAssignats.AddRange(assignats);
            await _context.SaveChangesAsync();

            return assignats;
        }

        public PrivilegiAssignat CrearAssignacio(
            string nia,
            Privilegi privilegi,
            string codiIntern,
            DateTime ara)
        {
            return new PrivilegiAssignat
            {
                NIA = nia,
                IdPrivilegi = privilegi.IdPrivilegi,

                NivellPrivilegi = privilegi.NivellPrivilegi,
                Descripcio = privilegi.Descripcio,
                Tipus = privilegi.Tipus,

                DataCreacio = ara,
                DataExecucio = null,
                CodiIntern = codiIntern
            };
        }

        // ==================================================
        // EXECUTAR (PER CODI INTERN)
        // ==================================================
        public async Task<bool> ExecutarAsync(string codiIntern)
        {
            var assignacions = await _context.PrivilegisAssignats
                .Where(p => p.CodiIntern == codiIntern && p.DataExecucio == null)
                .ToListAsync();

            if (!assignacions.Any())
                return false;

            var ara = DateTime.Now;
            foreach (var p in assignacions)
                p.DataExecucio = ara;

            await _context.SaveChangesAsync();
            return true;
        }

        // ==================================================
        // LLISTA PER ALUMNE
        // ==================================================
        public async Task<List<PrivilegiAssignat>> LlistaPerAlumneAsync(string nia)
        {
            return await _context.PrivilegisAssignats
                .Include(p => p.Privilegi)
                .Where(p => p.NIA == nia)
                .OrderByDescending(p => p.DataCreacio)
                .ToListAsync();
        }

        // ==================================================
        // LLISTA PER GRUP
        // ==================================================
        public async Task<List<PrivilegiAssignat>> LlistaPerGrupAsync(long idGrup)
        {
            return await _context.PrivilegisAssignats
                .Include(p => p.Privilegi)
                .Include(p => p.Alumne)
                .Where(p => p.Alumne.IdGrup == idGrup)
                .OrderByDescending(p => p.DataCreacio)
                .ToListAsync();
        }

        // ==================================================
        // LLISTA PER CLASSE
        // ==================================================
        public async Task<List<PrivilegiAssignat>> LlistaPerClasseAsync(long idClasse)
        {
            return await _context.PrivilegisAssignats
                .Include(p => p.Privilegi)
                .Include(p => p.Alumne)
                .Where(p => p.Alumne.IdClasse == idClasse)
                .OrderByDescending(p => p.DataCreacio)
                .ToListAsync();
        }
    }
}