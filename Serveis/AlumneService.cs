//using KarmaWebAPI.Controllers;
using Humanizer;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.DisplaySets;
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
        public async Task<AlumneDisplaySet> CrearAsync(AlumneDTO dto)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var tx = await _context.Database.BeginTransactionAsync();

                var alumne = new Alumne
                {
                    NIA = dto.NIA,
                    Nom = dto.Nom,
                    Cognoms = dto.Cognoms,
                    Actiu = true,
                    IdClasse = null,
                    IdGrup = null
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

                // Crear usuari Identity
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
                    // Assignació de classe i grup via CORE (sense nova transacció)
                    if (dto.IdClasse.HasValue)
                    {
                        alumne = await AssignarClasseIGrupCoreAsync(new AlumneAssignarClasseIGrupDTO
                        {
                            NIA = dto.NIA,
                            IdClasse = dto.IdClasse.Value,
                            IdGrup = dto.IdGrup   // pot ser null 
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

                return await InstanciaCoreAsync(dto.NIA);
            });
        }


        // ==================================================
        // EDITAR
        // ==================================================
        public async Task<AlumneDisplaySet> EditarAsync(AlumneDTO dto)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var tx = await _context.Database.BeginTransactionAsync();

                var alumne = await _context.Alumnes
                    .FirstOrDefaultAsync(a => a.NIA == dto.NIA)
                    ?? throw new InvalidOperationException("Alumne no trobat");

                // ✅ Guardem estat anterior per comparar
                var idClasseAnterior = alumne.IdClasse;
                var idGrupAnterior = alumne.IdGrup;

                // ✅ Actualitzar dades bàsiques
                alumne.Nom = dto.Nom;
                alumne.Cognoms = dto.Cognoms;

                try
                {
                    await _context.SaveChangesAsync();

                    // ✅ Si canvia classe o grup → usar CORE (sense transacció nova)
                    if (dto.IdClasse.HasValue &&
                        (idClasseAnterior != dto.IdClasse || idGrupAnterior != dto.IdGrup))
                    {
                        alumne = await AssignarClasseIGrupCoreAsync(new AlumneAssignarClasseIGrupDTO
                        {
                            NIA = dto.NIA,
                            IdClasse = dto.IdClasse.Value,
                            IdGrup = dto.IdGrup
                        });
                    }

                    await tx.CommitAsync();

                    return await InstanciaCoreAsync(dto.NIA);
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    throw new InvalidOperationException(
                        ex.InnerException?.Message ?? ex.Message);
                }
            });
        }


        // ==================================================
        // ACTIVAR / DESACTIVAR
        // ==================================================
        public async Task<AlumneDisplaySet> ActivarAsync(string nia)
        {
            var alumne = await _context.Alumnes.FindAsync(nia)
                ?? throw new InvalidOperationException("Alumne no trobat");

            alumne.Actiu = true;
            await _context.SaveChangesAsync();
            return await InstanciaCoreAsync(nia);
        }

        public async Task<AlumneDisplaySet> DesactivarAsync(string nia)
        {
            var alumne = await _context.Alumnes.FindAsync(nia)
                ?? throw new InvalidOperationException("Alumne no trobat");

            alumne.Actiu = false;
            await _context.SaveChangesAsync();
            return await InstanciaCoreAsync(nia);
        }

        // ==================================================
        // INSTÀNCIA
        // ==================================================
        //wrapper
        public async Task<AlumneDisplaySet> InstanciaAsync(string nia, ClaimsPrincipal user)
        {
            if (user.IsInRole("AG_Alumne") && user.Identity!.Name != nia)
                throw new UnauthorizedAccessException();

            return await InstanciaCoreAsync(nia);
        }

        //Core
        public async Task<AlumneDisplaySet> InstanciaCoreAsync(string nia)
        {
            var result = await _context.Alumnes
                .Where(a => a.NIA == nia)
                .Select(a => new AlumneDisplaySet
                {
                    NIA = a.NIA,
                    Nom = a.Nom,
                    Cognoms = a.Cognoms,

                    IdAnyEscolar = a.Classe != null ? a.Classe.IdAnyEscolar : 0,

                    IdClasse = a.IdClasse != null ? a.IdClasse.ToString() : null,
                    NomClasse = a.Classe != null ? a.Classe.Nom : null,

                    IdGrup = a.IdGrup != null ? a.IdGrup.ToString() : null,
                    NomGrup = a.Grup != null ? a.Grup.Nom : null,
                    KarmaBaseGrup = a.Grup != null ? a.Grup.KarmaBase : null,

                    AlumnesEnGrup = a.IdGrup != null
                        ? _context.Alumnes
                            .Where(x => x.IdGrup == a.IdGrup)
                            .Select(x => x.Nom + " " + x.Cognoms)                        
                            .ToList()
                        : new List<string>()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return result ?? throw new InvalidOperationException("Alumne no trobat");
        }

        // ==================================================
        // LLISTA
        // ==================================================
        public async Task<List<AlumneDisplaySet>> LlistaAsync(ClaimsPrincipal user)
        {
            var query = _context.Alumnes.AsQueryable();

            if (user.IsInRole("AG_Alumne"))
            {
                var nia = user.Identity!.Name;
                query = query.Where(a => a.NIA == nia);
            }

            return await query
                .Select(a => new AlumneDisplaySet
                {
                    NIA = a.NIA,
                    Nom = a.Nom,
                    Cognoms = a.Cognoms,

                    IdAnyEscolar = a.Classe != null ? a.Classe.IdAnyEscolar : 0,

                    IdClasse = a.IdClasse != null ? a.IdClasse.ToString() : null,
                    NomClasse = a.Classe != null ? a.Classe.Nom : null,

                    IdGrup = a.IdGrup != null ? a.IdGrup.ToString() : null,
                    NomGrup = a.Grup != null ? a.Grup.Nom : null,
                    KarmaBaseGrup = a.Grup != null ? a.Grup.KarmaBase : null,

                    AlumnesEnGrup = a.IdGrup != null
                        ? _context.Alumnes
                            .Where(x => x.IdGrup == a.IdGrup)
                            .Select(x => x.Nom + " " + x.Cognoms)
                            .ToList()
                        : new List<string>()
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<AlumneDisplaySet>> LlistaPerClasseAsync(
            long idClasse,
            ClaimsPrincipal user)
        {
            var query = _context.Alumnes
                .Where(a => a.IdClasse == idClasse)
                .AsQueryable();

            // ✅ Si és alumne → només ell mateix
            if (user.IsInRole("AG_Alumne"))
            {
                var nia = user.Identity!.Name;
                query = query.Where(a => a.NIA == nia);
            }

            return await query
                .Select(a => new AlumneDisplaySet
                {
                    NIA = a.NIA,
                    Nom = a.Nom,
                    Cognoms = a.Cognoms,

                    IdAnyEscolar = a.Classe != null ? a.Classe.IdAnyEscolar : 0,

                    IdClasse = a.IdClasse != null ? a.IdClasse.ToString() : null,
                    NomClasse = a.Classe != null ? a.Classe.Nom : null,
                    KarmaBaseGrup = a.Grup != null ? a.Grup.KarmaBase : null,

                    IdGrup = a.IdGrup != null ? a.IdGrup.ToString() : null,
                    NomGrup = a.Grup != null ? a.Grup.Nom : null,

                    AlumnesEnGrup = a.IdGrup != null
                        ? _context.Alumnes
                            .Where(x => x.IdGrup == a.IdGrup)
                            .Select(x => x.Nom + " " + x.Cognoms)
                            .ToList()
                        : new List<string>()
                })
                .AsNoTracking()
                .ToListAsync();
        }



        // ==================================================
        // ASSIGNAR CLASSE I ASSIGNAR GRUP
        // ==================================================
        //Orquestrador
        public async Task<AlumneDisplaySet> AssignarClasseIGrupAsync(AlumneAssignarClasseIGrupDTO dto)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var tx = await _context.Database.BeginTransactionAsync();

                try
                {
                    var alumne = await AssignarClasseIGrupCoreAsync(dto);

                    await tx.CommitAsync();
                    return await InstanciaCoreAsync(dto.NIA);
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            });
        }
        //Core
        public async Task<Alumne> AssignarClasseIGrupCoreAsync(AlumneAssignarClasseIGrupDTO dto)
        {
            var alumne = await _context.Alumnes
                .FirstOrDefaultAsync(a => a.NIA == dto.NIA)
                ?? throw new InvalidOperationException("Alumne no trobat");

            var classe = await _context.Classes
                .FirstOrDefaultAsync(c => c.IdClasse == dto.IdClasse)
                ?? throw new InvalidOperationException("La classe no existeix");

            Grup? grup = null;

            if (dto.IdGrup.HasValue)
            {
                grup = await _context.Grups
                    .FirstOrDefaultAsync(g => g.IdGrup == dto.IdGrup)
                    ?? throw new InvalidOperationException("El grup no existeix");

                if (grup.IdClasse != dto.IdClasse)
                    throw new InvalidOperationException("El grup no pertany a la classe indicada");
            }

            var idGrupAnterior = alumne.IdGrup;

            alumne.IdClasse = dto.IdClasse;
            alumne.IdGrup = dto.IdGrup;

            await _context.SaveChangesAsync();

            // també sense transacció
            if (idGrupAnterior.HasValue && idGrupAnterior != dto.IdGrup)
                await _grupService.CalcularKarmaBaseCoreAsync(idGrupAnterior.Value, null);

            if (dto.IdGrup.HasValue)
                await _grupService.CalcularKarmaBaseCoreAsync(dto.IdGrup.Value, null);

            return alumne;
        }


        public async Task<AlumneDisplaySet> LlevarAlumneDeGrupAsync(string nia)
        {
            var alumne = await _context.Alumnes
                .FirstOrDefaultAsync(a => a.NIA == nia)
                ?? throw new InvalidOperationException("Alumne no trobat");

            var idGrupAnterior = alumne.IdGrup;

            // desassignar
            alumne.IdGrup = null;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException(
                    ex.InnerException?.Message ?? ex.Message);
            }

            // si tenia grup → recalcular
            if (idGrupAnterior.HasValue)
                await _grupService.CalcularKarmaBaseAsync(idGrupAnterior.Value, null);

            return await InstanciaCoreAsync(alumne.NIA);
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
