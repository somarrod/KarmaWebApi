using KarmaWebAPI.Data;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Serveis
{
    public class PrivilegiAssignatService : IPrivilegiAssignatService
    {
        private readonly DatabaseContext _context;
        private readonly IAlumneService _alumneService;

        public PrivilegiAssignatService(DatabaseContext context, IAlumneService alumneService)
        {
            _context = context;
            this._alumneService = alumneService;
        }

        // ==================================================
        // ASSIGNAR PRIVILEGI
        // ==================================================
        public async Task<List<PrivilegiAssignat>> AssignarAsync(string nia, long idPrivilegi)
        {
            try
            {
                var alumne = await _context.Alumnes
                    .Include(a => a.Grup)
                    .Include(a => a.Classe)
                    .FirstOrDefaultAsync(a => a.NIA == nia)
                    ?? throw new InvalidOperationException("Alumne no trobat");

                var privilegi = await _context.Privilegis
                    .FirstOrDefaultAsync(p => p.IdPrivilegi == idPrivilegi && p.Actiu)
                    ?? throw new InvalidOperationException("Privilegi no vàlid");

                var ara = DateTime.Now;
                var codiIntern = $"PA-{idPrivilegi}-{Guid.NewGuid().ToString("N")[..8]}";

                var assignats = new List<PrivilegiAssignat>();

                // ==============================
                // VALIDAR NIVELL DE KARMA
                // ==============================
                var nivellPermes = await _alumneService
                    .ObtenirNivellPrivilegiPermesAsync(alumne.NIA);

                if (nivellPermes == null || nivellPermes < privilegi.NivellPrivilegi)
                {
                    throw new InvalidOperationException(
                        "L'alumne no té nivell de karma suficient per a aquest privilegi");
                }

                // ==============================
                // ASSIGNACIÓ
                // ==============================
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
                        var nivellPermesGrup = await _alumneService
                            .ObtenirNivellPrivilegiPermesAsync(a.NIA);

                        if (nivellPermesGrup == null || nivellPermesGrup < privilegi.NivellPrivilegi)
                            throw new InvalidOperationException(
                                $"{a.Nom} no té nivell de karma suficient per a aquest privilegi");

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
            catch (DbUpdateException dbEx)
            {
                // errors típics de BD (FK, duplicates, nulls…)
                throw new InvalidOperationException(
                    dbEx.InnerException?.Message ?? "Error guardant dades en la base de dades");
            }
            catch (InvalidOperationException)
            {
                // errors de negoci (els teus)
                throw;
            }
            catch (Exception ex)
            {
                // 💥 qualsevol altre error inesperat
                throw new Exception(
                    $"Error inesperat en assignar privilegi: {ex.Message}");
            }
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
        // EXECUTAR (per IdPrivilegi Assignat)
        // ==================================================
        public async Task<List<PrivilegiAssignat>> ExecutarAsync(long idPrivilegiAssignat)
        {
            // ==============================
            // 1. Carregar assignació base
            // ==============================
            var privilegiAssignat = await _context.PrivilegisAssignats
                .FirstOrDefaultAsync(p =>
                    p.IdPrivilegiAssignat == idPrivilegiAssignat &&
                    p.DataExecucio == null);

            if (privilegiAssignat == null)
                throw new InvalidOperationException(
                    $"Privilegi assignat no trobat per Id {idPrivilegiAssignat}"); 

            // ==============================
            // 2. Carregar privilegi
            // ==============================
            var privilegi = await _context.Privilegis
                .FirstOrDefaultAsync(p =>
                    p.IdPrivilegi == privilegiAssignat.IdPrivilegi);

            if (privilegi == null)
                throw new InvalidOperationException(
                    $"Privilegi no trobat per Id {privilegiAssignat.IdPrivilegi}");

            var tipus = privilegi.Tipus;

            // ==============================
            // 3. Determinar conjunt
            // ==============================
            IQueryable<PrivilegiAssignat> query;

            if (tipus == "G")
            {
                query = _context.PrivilegisAssignats
                    .Where(p =>
                        p.CodiIntern == privilegiAssignat.CodiIntern &&
                        p.DataExecucio == null);
            }
            else
            {
                query = _context.PrivilegisAssignats
                    .Where(p =>
                        p.IdPrivilegiAssignat == idPrivilegiAssignat &&
                        p.DataExecucio == null);
            }

            // ==============================
            // 4. Executar
            // ==============================
            var assignacions = await query.ToListAsync();

            if (assignacions.Count == 0)
                throw new InvalidOperationException(
                     $"Privilegi no trobat per Id {privilegiAssignat.IdPrivilegi}");

            var ara = DateTime.Now;

            foreach (var p in assignacions)
                p.DataExecucio = ara;

            // ==============================
            // 5. Guardar
            // ==============================
            await _context.SaveChangesAsync();

            // IMPORTANT: l’objecte original ja està actualitzat (tracking EF)
            return assignacions;
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