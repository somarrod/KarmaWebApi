namespace KarmaWebAPI.Serveis
{
    using KarmaWebAPI.Data;
    using KarmaWebAPI.DTOs;
    using KarmaWebAPI.DTOs.DisplaySets;
    using KarmaWebAPI.Models;
    using KarmaWebAPI.Serveis.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    public class PuntuacioService : IPuntuacioService
    {
        private readonly DatabaseContext _context;
        private readonly IGrupService _grupService;
        private readonly IKarmaAlumneService _karmaAlumneService;

        public PuntuacioService(
            DatabaseContext context,
            IGrupService grupService,
            IKarmaAlumneService karmaAlumneService)
        {
            _context = context;
            _grupService = grupService;
            _karmaAlumneService = karmaAlumneService;
        }

        // ==================================================
        // ASSIGNAR PUNTS (S)
        // ==================================================
        public async Task<PuntuacioDisplaySet> AssignarPuntsAsync(
            string nia,
            long idAvaluacio,
            long idCategoria,
            double numPunts,
            string motiu,
            string? descripcioAdicional,
            DateOnly dataEvent,
            ClaimsPrincipal user)
        {
            return await CrearPuntuacioAsync(
                nia,
                idAvaluacio,
                idCategoria,
                "S",
                numPunts,
                motiu,
                descripcioAdicional,
                dataEvent,
                user);
        }

        // ==================================================
        // REINICIAR PUNTS (I)
        // ==================================================

        public async Task<PuntuacioDisplaySet> ReiniciarPuntsAsync(
            string nia,
            long idAvaluacio,
            long idCategoria,
            double nouValor,
            string motiu,
            string? descripcioAdicional,
            DateOnly dataEvent,
            ClaimsPrincipal user)
        {
            return await CrearPuntuacioAsync(
                nia,
                idAvaluacio,
                idCategoria,
                "I",
                nouValor,
                motiu,
                descripcioAdicional,
                dataEvent,
                user);
        }

        private async Task<PuntuacioDisplaySet> CrearPuntuacioAsync(
            string nia,
            long idAvaluacio,
            long idCategoria,
            string tipus,
            double numPunts,
            string motiu,
            string? descripcioAdicional,
            DateOnly dataEvent,
            ClaimsPrincipal user)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var tx = await _context.Database.BeginTransactionAsync();

                try
                {
                    var puntuacio = await CrearPuntuacioCoreAsync(
                        nia,
                        idAvaluacio,
                        idCategoria,
                        tipus,
                        numPunts,
                        motiu,
                        descripcioAdicional,
                        dataEvent,
                        user);

                    await _context.SaveChangesAsync();

                    await tx.CommitAsync();

                    // retornar DisplaySet (no entity)
                    return await QueryPuntuacioDisplaySet()
                        .AsNoTracking()
                        .FirstAsync(p => p.IdPuntuacio == puntuacio.IdPuntuacio);
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
        // IMPLEMENTACIÓ INTERNA COMUNA
        // ==================================================
        private async Task<Puntuacio> CrearPuntuacioCoreAsync(
            string nia,
            long idAvaluacio,
            long idCategoria,
            string tipus,
            double numPunts,
            string motiu,
            string? descripcioAdicional,
            DateOnly dataEvent,
            ClaimsPrincipal user)
        {
            var ara = DateTime.Now;

            var idProfessor = user.Identity?.Name
                ?? throw new InvalidOperationException("Usuari no autenticat");

            var professor = await _context.Professors
                .FirstOrDefaultAsync(p => p.IdProfessor == idProfessor)
                ?? throw new InvalidOperationException("És necessari que es connecte com a professor");

            if (!professor.Actiu)
                throw new InvalidOperationException("El professor connectat no està actiu");

            var alumne = await _context.Alumnes
                .Include(a => a.Classe)
                .Include(a => a.Grup)
                .FirstOrDefaultAsync(a => a.NIA == nia)
                ?? throw new InvalidOperationException("Alumne no trobat");

            if (!alumne.IdClasse.HasValue)
                throw new InvalidOperationException("L’alumne no té classe");

            var avaluacio = await _context.Avaluacions
                .FirstOrDefaultAsync(a => a.IdAvaluacio == idAvaluacio)
                ?? throw new InvalidOperationException("Avaluació no trobada");

            if (avaluacio.IdAnyEscolar != alumne.Classe.IdAnyEscolar)
                throw new InvalidOperationException("Avaluació fora del curs de l’alumne");

            if (dataEvent < avaluacio.DataInicial || dataEvent > avaluacio.DataFinal)
                throw new InvalidOperationException(
                    $"La data ha d'estar entre {avaluacio.DataInicial:dd/MM/yyyy} i {avaluacio.DataFinal:dd/MM/yyyy}");

            var categoria = await _context.Categories
                .FirstOrDefaultAsync(c => c.IdCategoria == idCategoria)
                ?? throw new InvalidOperationException("Categoria no existent");

            if (!categoria.Editable && numPunts != categoria.NumPunts)
                throw new InvalidOperationException("Aquesta categoria no permet modificar el nombre de punts");

            if (!categoria.Editable)
                numPunts = categoria.NumPunts;

            var puntuacio = new Puntuacio
            {
                NIA = nia,
                IdProfessor = idProfessor,
                IdCategoria = idCategoria,

                IdClasse = alumne.IdClasse.Value,
                NomClasse = alumne.Classe.Nom,

                IdGrup = alumne.IdGrup,
                NomGrup = alumne.Grup?.Nom,

                NumPunts = numPunts,
                Tipus = tipus,
                Motiu = motiu,
                DescripcioAdicional = descripcioAdicional,

                DataEvent = dataEvent,
                DataCreacio = ara,

                IdAvaluacio = idAvaluacio
            };

            _context.Puntuacions.Add(puntuacio);

            var karma = await _context.KarmaAlumnes
                .FirstOrDefaultAsync(k => k.NIA == nia && k.IdAvaluacio == idAvaluacio);

            if (karma == null)
            {
                // crea el karma amb el servei (sense SaveChanges)
                await _karmaAlumneService.CrearPerAlumneCoreAsync(
                    nia,
                    idAvaluacio,
                    0);

                // torna a carregar-lo ja creat (tracking actiu)
                karma = _context.KarmaAlumnes
                    .Local
                    .First(k => k.NIA == nia && k.IdAvaluacio == idAvaluacio);
            }


            if (tipus == "S")
                karma.NumPuntsActuals += numPunts;
            else if (tipus == "I")
                karma.NumPuntsActuals = numPunts;
            else
                throw new InvalidOperationException("Tipus de puntuació no vàlid");

            var configuracio = await _context.ConfiguracionsKarma
                .FirstAsync(c =>
                    c.IdAnyEscolar == avaluacio.IdAnyEscolar &&
                    karma.NumPuntsActuals >= c.NumPuntsMinim &&
                    karma.NumPuntsActuals < c.NumPuntsMaxim);

            karma.KarmaActual = configuracio.ColorKarma;

            alumne.KarmaActualPunts = karma.NumPuntsActuals;
            alumne.KarmaActualColor = configuracio.ColorKarma;


            if (alumne.IdGrup.HasValue)
                await _grupService.CalcularKarmaBaseCoreAsync(alumne.IdGrup.Value, false);

            return puntuacio;
        }

        // ==================================================
        // CONSULTES
        // ==================================================
        public async Task<PuntuacioDisplaySet?> InstanciaAsync(
            long idPuntuacio,
            ClaimsPrincipal user)
        {
            var query = QueryPuntuacioDisplaySet()
                .Where(p => p.IdPuntuacio == idPuntuacio);

            if (user.IsInRole("AG_Alumne"))
            {
                var nia = user.Identity!.Name;
                query = query.Where(p => p.NIA == nia);
            }

            return await query
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        
        public async Task<List<PuntuacioDisplaySet>> LlistaPerAlumneAsync(string nia, ClaimsPrincipal user, long? idAvaluacio = null)
        {
            var query = QueryPuntuacioDisplaySet();

            //El alumne conectat sols pot vore les seues propies puntuacions
            if (user.IsInRole("AG_Alumne"))
            {
                var niaUser = user.Identity!.Name;
                query = query.Where(p => p.NIA == niaUser);
            }
            else
            {
                query = query.Where(p => p.NIA == nia);
            }

            if (idAvaluacio.HasValue)
                query = query.Where(p => p.IdAvaluacio == idAvaluacio.Value);

            return await query
                .OrderByDescending(p => p.DataEvent)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<PuntuacioDisplaySet>> LlistaPerClasseAsync(long idClasse, ClaimsPrincipal user, long? idAvaluacio = null)
        {
            var query = QueryPuntuacioDisplaySet()
                .Where(p => p.IdClasse == idClasse);

            if (user.IsInRole("AG_Alumne"))
            {
                var nia = user.Identity!.Name;
                query = query.Where(p => p.NIA == nia);
            }

            if (idAvaluacio.HasValue)
                query = query.Where(p => p.IdAvaluacio == idAvaluacio.Value);

            return await query
                .OrderByDescending(p => p.DataEvent)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<List<PuntuacioDisplaySet>> LlistaPerGrupAsync(long idGrup,ClaimsPrincipal user, long? idAvaluacio = null)
        {
            var query = QueryPuntuacioDisplaySet()
                .Where(p => p.IdGrup == idGrup);

            if (user.IsInRole("AG_Alumne"))
            {
                var nia = user.Identity!.Name;
                query = query.Where(p => p.NIA == nia);
            }

            if (idAvaluacio.HasValue)
                query = query.Where(p => p.IdAvaluacio == idAvaluacio.Value);

            return await query
                .OrderByDescending(p => p.DataEvent)
                .AsNoTracking()
                .ToListAsync();
        }

        private IQueryable<PuntuacioDisplaySet> QueryPuntuacioDisplaySet()
        {
            return _context.Puntuacions
                .Select(p => new PuntuacioDisplaySet
                {
                    IdPuntuacio = p.IdPuntuacio,

                    NIA = p.NIA,
                    NomAlumne = p.Alumne.Nom + " " + p.Alumne.Cognoms,

                    IdProfessor = p.IdProfessor,
                    NomProfessor = p.Professor.Nom,

                    IdCategoria = p.IdCategoria,
                    DescripcioCategoria = p.Categoria.Descripcio,

                    IdClasse = p.IdClasse,
                    NomClasse = p.NomClasse,

                    IdGrup = p.IdGrup,
                    NomGrup = p.NomGrup,

                    NumPunts = p.NumPunts,
                    Tipus = p.Tipus,
                    Motiu = p.Motiu,
                    DescripcioAdicional = p.DescripcioAdicional,

                    DataEvent = p.DataEvent,
                    DataCreacio = p.DataCreacio,

                    IdAvaluacio = p.IdAvaluacio,
                    NomAvaluacio = p.Avaluacio.Nom,
                    IdAnyEscolar = p.Avaluacio.IdAnyEscolar,

                    //  KARMA DINÀMIC PER DATA
                    KarmaActualPunts = _context.KarmaAlumnes
                        .Where(k =>
                            k.NIA == p.NIA &&
                            k.Avaluacio.DataInicial <= p.DataEvent &&
                            k.Avaluacio.DataFinal >= p.DataEvent)
                        .Select(k => (double?)k.NumPuntsActuals)
                        .FirstOrDefault() ?? 0,

                    KarmaActualColor = _context.KarmaAlumnes
                        .Where(k =>
                            k.NIA == p.NIA &&
                            k.Avaluacio.DataInicial <= p.DataEvent &&
                            k.Avaluacio.DataFinal >= p.DataEvent)
                        .Select(k => k.KarmaActual)
                        .FirstOrDefault() ?? "No definit"
                });
        }
    }


}