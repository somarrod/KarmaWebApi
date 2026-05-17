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

        public AlumneService(
            DatabaseContext context,
            AccountService accountService,
            IKarmaAlumneService karmaAlumneService)
        {
            _context = context;
            _accountService = accountService;
            _karmaAlumneService = karmaAlumneService;
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
                IdClasse = dto.IdClasse,
                IdGrup = dto.IdGrup
            };


            try
            {
                _context.Alumnes.Add(alumne);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                await tx.RollbackAsync();
                throw new InvalidOperationException(
                    "Ja existeix un alumne amb aquest NIA o les dades no són vàlides."
                );
            }


            // Crear usuari AG_Alumne
            var password = FuncionsAuxiliars.ConstruirPasswordAlumne(dto);
            var identityResult = await _accountService.CreateUserAsync(
                dto.NIA, null, "AG_Alumne", password);

            if (!identityResult.Succeeded)
            {
                await tx.RollbackAsync();
                throw new InvalidOperationException(
                    identityResult.Errors.First().Description);
            }

            // Crear Karma només si té classe assignada
            if (alumne.IdClasse.HasValue)
            {
                var idAnyEscolar = await _context.Classes
                    .Where(c => c.IdClasse == alumne.IdClasse)
                    .Select(c => c.IdAnyEscolar)
                    .FirstAsync();

                await _karmaAlumneService
                    .CrearPerAlumneDesdeAvaluacioEnCursAsync(
                        alumne.NIA, idAnyEscolar);
            }

            await tx.CommitAsync();
            return alumne;
        }

        // ==================================================
        // EDITAR
        // ==================================================
        public async Task<Alumne> EditarAsync(AlumneDTO dto)
        {
            var alumne = await _context.Alumnes
                .FirstOrDefaultAsync(a => a.NIA == dto.NIA);

            if (alumne == null)
                throw new InvalidOperationException("Alumne no trobat");

            alumne.Nom = dto.Nom;
            alumne.Cognoms = dto.Cognoms;

            await _context.SaveChangesAsync();
            return alumne; // objecte modificat
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


        // ==================================================
        // ASSIGNAR CLASSE I ASSIGNAR GRUP
        // ==================================================
        public async Task<Alumne> AssignarClasseAsync(AlumneAssignarClasseDTO dto)
        {
            var alumne = await _context.Alumnes
                .FirstOrDefaultAsync(a => a.NIA == dto.NIA);

            if (alumne == null)
                throw new InvalidOperationException("Alumne no trobat");

            var classeExisteix = await _context.Classes
                .AnyAsync(c => c.IdClasse == dto.IdClasse && c.IdAnyEscolar == dto.IdAnyEscolar);

            if (!classeExisteix)
                throw new InvalidOperationException("La classe no existeix");

            // Si no canvia, retornem igualment l’objecte
            if (alumne.IdClasse == dto.IdClasse )
                return alumne;

            // Assignar nova classe
            alumne.IdClasse = dto.IdClasse;
            

            // Desvincular del grup anterior
            alumne.IdGrup = null; // o 0 si el mantens no nullable

            await _context.SaveChangesAsync();

            return alumne; // objecte modificat
        }

        public async Task<Alumne> AssignarGrupAsync(string nia, long idGrup)
        {
            // 1. Alumne
            var alumne = await _context.Alumnes
                .FirstOrDefaultAsync(a => a.NIA == nia)
                ?? throw new InvalidOperationException("Alumne no trobat");

            if (!alumne.IdClasse.HasValue)
                throw new InvalidOperationException("L'alumne no té cap classe assignada");

            // 2. Grup
            var grup = await _context.Grups
                .FirstOrDefaultAsync(g => g.IdGrup == idGrup)
                ?? throw new InvalidOperationException("Grup no trobat");

            // 3. Validar que el grup pertany a la mateixa classe
            if (grup.IdClasse != alumne.IdClasse)
                throw new InvalidOperationException(
                    "El grup no pertany a la mateixa classe que l'alumne");

            // 4. Desvincular grup actual i assignar el nou
            alumne.IdGrup = idGrup;

            await _context.SaveChangesAsync();
            return alumne;
        }


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
