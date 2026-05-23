//using KarmaWebAPI.Controllers;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;

//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KarmaWebAPI.Serveis
{

    public class AlumneService : IAlumneService
    {

        private readonly DatabaseContext _context;
        private readonly AccountService _accountService;
        private readonly IKarmaAlumneService _karmaAlumneService;
        private readonly IGrupService _grupService;

        public AlumneService(
            DatabaseContext context,
            AccountService accountService,
            IKarmaAlumneService karmaAlumneService,
            IGrupService grupService)
        {
            _context = context;
            _accountService = accountService;
            _karmaAlumneService = karmaAlumneService;
            _grupService = grupService;
        }


        // ==================================================
        // CREAR ALUMNE (+ usuari Identity)
        // ==================================================
        public async Task<Alumne> CrearAsync(AlumneDTO dto)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            var alumne = new Alumne
            {
                NIA = dto.NIA,
                Nom = dto.Nom,
                Cognoms = dto.Cognoms,
                Actiu = true,
                IdClasse = null,   // ✅ NO assignar ací
                IdGrup = null      // ✅ NO assignar ací
            };

            try
            {
                _context.Alumnes.Add(alumne);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                await tx.RollbackAsync();
                throw new InvalidOperationException(
                    "Ja existeix un alumne amb aquest NIA o les dades no són vàlides.");
            }

            // ✅ Crear usuari
            var password = FuncionsAuxiliars.ConstruirPasswordAlumne(dto);

            var identityResult = await _accountService.CreateUserAsync(
                dto.NIA, null, "AG_Alumne", password);

            if (!identityResult.Succeeded)
            {
                await tx.RollbackAsync();
                throw new InvalidOperationException(
                    identityResult.Errors.First().Description);
            }

            try
            {
                // Si ve classe i grup → usar el servei que ja tens
                if (dto.IdClasse.HasValue)
                {
                    await AssignarClasseIGrupAsync(new AlumneAssignarClasseIGrupDTO
                    {
                        NIA = dto.NIA,
                        IdClasse = dto.IdClasse.Value,
                        IdGrup = dto.IdGrup
                    });
                }

                await tx.CommitAsync();
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                throw new InvalidOperationException(
                    ex.InnerException?.Message ?? ex.Message);
            }

            return alumne;
        }

        // ==================================================
        // EDITAR
        // ==================================================
        public async Task<Alumne> EditarAsync(AlumneDTO dto)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            var alumne = await _context.Alumnes
                .FirstOrDefaultAsync(a => a.NIA == dto.NIA);

            if (alumne == null)
                throw new InvalidOperationException("Alumne no trobat");

            // Actualitzar dades bàsiques
            alumne.Nom = dto.Nom;
            alumne.Cognoms = dto.Cognoms;

            try
            {
                await _context.SaveChangesAsync();

                // Si canvia classe o grup → reutilitzar servei
                if (dto.IdClasse.HasValue &&
                    (alumne.IdClasse != dto.IdClasse || alumne.IdGrup != dto.IdGrup))
                {
                    alumne = await AssignarClasseIGrupAsync(new AlumneAssignarClasseIGrupDTO
                    {
                        NIA = dto.NIA,
                        IdClasse = dto.IdClasse.Value,
                        IdGrup = dto.IdGrup
                    });
                }

                await tx.CommitAsync();
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                throw new InvalidOperationException(
                    ex.InnerException?.Message ?? ex.Message);
            }

            return alumne;
        }



        // ==================================================
        // ACTIVAR / DESACTIVAR
        // ==================================================
        public async Task<Alumne> ActivarAsync(string nia)
        {
            var alumne = await _context.Alumnes.FindAsync(nia)
                ?? throw new InvalidOperationException("Alumne no trobat");

            alumne.Actiu = true;
            await _context.SaveChangesAsync();
            return alumne;
        }

        public async Task<Alumne> DesactivarAsync(string nia)
        {
            var alumne = await _context.Alumnes.FindAsync(nia)
                ?? throw new InvalidOperationException("Alumne no trobat");

            alumne.Actiu = false;
            await _context.SaveChangesAsync();
            return alumne;
        }

        // ==================================================
        // INSTÀNCIA
        // ==================================================
        public async Task<Alumne> InstanciaAsync(string nia, ClaimsPrincipal user)
        {
            if (user.IsInRole("AG_Alumne") && user.Identity!.Name != nia)
                throw new UnauthorizedAccessException();

            var alumne = await _context.Alumnes
                .Include(a => a.Classe)
                .Include(a => a.Grup)
                .FirstOrDefaultAsync(a => a.NIA == nia);

            return alumne ?? throw new InvalidOperationException("Alumne no trobat");
        }

        // ==================================================
        // LLISTA
        // ==================================================
        public async Task<List<Alumne>> LlistaAsync(ClaimsPrincipal user)
        {
            if (user.IsInRole("AG_Alumne"))
            {
                var nia = user.Identity!.Name;
                return await _context.Alumnes
                    .Where(a => a.NIA == nia)
                    .ToListAsync();
            }

            return await _context.Alumnes.ToListAsync();
        }

        public async Task<List<Alumne>> LlistaPerClasseAsync(long idClasse, ClaimsPrincipal user)
        {
            // Si és alumne → només es veu a si mateix (i només si pertany a eixa classe)
            if (user.IsInRole("AG_Alumne"))
            {
                var nia = user.Identity!.Name;

                return await _context.Alumnes
                    .Where(a => a.NIA == nia && a.IdClasse == idClasse)
                    .ToListAsync();
            }

            // Resta d’usuaris → tots els alumnes de la classe
            return await _context.Alumnes
                .Where(a => a.IdClasse == idClasse)
                .ToListAsync();
        }



        // ==================================================
        // ASSIGNAR CLASSE I ASSIGNAR GRUP
        // ==================================================
        public async Task<Alumne> AssignarClasseIGrupAsync(AlumneAssignarClasseIGrupDTO dto)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            var alumne = await _context.Alumnes
                .FirstOrDefaultAsync(a => a.NIA == dto.NIA)
                ?? throw new InvalidOperationException("Alumne no trobat");

            var classe = await _context.Classes
                .FirstOrDefaultAsync(c => c.IdClasse == dto.IdClasse)
                ?? throw new InvalidOperationException("La classe no existeix");

            Grup? grup = null;

            // Només validar grup si ve informat
            if (dto.IdGrup.HasValue)
            {
                grup = await _context.Grups
                    .FirstOrDefaultAsync(g => g.IdGrup == dto.IdGrup)
                    ?? throw new InvalidOperationException("El grup no existeix");

                // Validació coherència classe-grup
                if (grup.IdClasse != dto.IdClasse)
                    throw new InvalidOperationException(
                        "El grup no pertany a la classe indicada");
            }

            // Si no hi ha canvi real
            if (alumne.IdClasse == dto.IdClasse && alumne.IdGrup == dto.IdGrup)
                return alumne;

            var idGrupAnterior = alumne.IdGrup;

            // Assignació
            alumne.IdClasse = dto.IdClasse;
            alumne.IdGrup = dto.IdGrup; // pot ser null 
            try
            {
                await _context.SaveChangesAsync();

                // Recalcular grup anterior si existia i canvia
                if (idGrupAnterior.HasValue && idGrupAnterior != dto.IdGrup)
                {
                    await _grupService.CalcularKarmaBaseAsync(idGrupAnterior.Value);
                }

                // Recalcular nou grup només si existeix
                if (dto.IdGrup.HasValue)
                {
                    await _grupService.CalcularKarmaBaseAsync(dto.IdGrup.Value);
                }

                await tx.CommitAsync();
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                throw new InvalidOperationException(
                    ex.InnerException?.Message ?? ex.Message);
            }

            return alumne;
        }

        //public async Task<Alumne> AssignarGrupAsync(string nia, long idGrup)
        //{
        //    // 1. Alumne
        //    var alumne = await _context.Alumnes
        //        .FirstOrDefaultAsync(a => a.NIA == nia)
        //        ?? throw new InvalidOperationException("Alumne no trobat");

        //    if (!alumne.IdClasse.HasValue)
        //        throw new InvalidOperationException("L'alumne no té cap classe assignada");

        //    // 2. Grup
        //    var grup = await _context.Grups
        //        .FirstOrDefaultAsync(g => g.IdGrup == idGrup)
        //        ?? throw new InvalidOperationException("Grup no trobat");

        //    // 3. Validar que el grup pertany a la mateixa classe
        //    if (grup.IdClasse != alumne.IdClasse)
        //        throw new InvalidOperationException(
        //            "El grup no pertany a la mateixa classe que l'alumne");

        //    // 4. Desvincular grup actual i assignar el nou
        //    alumne.IdGrup = idGrup;

        //    await _context.SaveChangesAsync();
        //    return alumne;
        //}


        // Servei que sincronitza tots els alumnes de la BD amb Identity.
        // - Crea AspNetUsers si no existeixen
        // - Assigna el rol AG_Alumne si cal
        // - Pensat per a imports massius (Excel / SQL)
        public async Task SincronitzarIdentityAsync()
        {
            var alumnes = await _context.Alumnes.ToListAsync();

            foreach (var alumne in alumnes)
            {
                var emailFictici = $"{alumne.NIA}@alumnat.val";

                AlumneDTO dto = new AlumneDTO() { 
                    Nom = alumne.Nom,
                    Cognoms = alumne.Cognoms,
                    NIA = alumne.NIA
                };
                string password = FuncionsAuxiliars.ConstruirPasswordAlumne(dto);

                await _accountService.EnsureUserWithRoleAsync(
                    alumne.NIA,
                    emailFictici,
                    "AG_Alumne", 
                    password);

                // opcional: si està actiu, desbloqueja
                if (alumne.Actiu)
                {
                    await _accountService.ReactivateUserAsync(alumne.NIA);
                }
            }
        }

    }

}
