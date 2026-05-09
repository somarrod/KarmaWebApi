namespace KarmaWebAPI.Serveis
{
    using KarmaWebAPI.Data;
    using KarmaWebAPI.DTOs;
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

        public PuntuacioService(
            DatabaseContext context,
            IGrupService grupService)
        {
            _context = context;
            _grupService = grupService;
        }

        // ==================================================
        // ASSIGNAR PUNTS (S)
        // ==================================================
        public async Task<Puntuacio> AssignarPuntsAsync(
            string nia,
            long idAvaluacio,
            long idCategoria,
            double numPunts,
            string motiu,
            string? descripcioAdicional,
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
                user);
        }

        // ==================================================
        // REINICIAR PUNTS (I)
        // ==================================================
        public async Task<Puntuacio> ReiniciarPuntsAsync(
            string nia,
            long idAvaluacio,
            long idCategoria,
            double nouValor,
            string motiu,
            string? descripcioAdicional,
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
                user);
        }

        // ==================================================
        // IMPLEMENTACIÓ INTERNA COMUNA
        // ==================================================
        private async Task<Puntuacio> CrearPuntuacioAsync(
            string nia,
            long idAvaluacio,
            long idCategoria,
            string tipus,
            double numPunts,
            string motiu,
            string? descripcioAdicional,
            ClaimsPrincipal user)
        {
            var ara = DateTime.Now;
            var hui = DateOnly.FromDateTime(ara);

            // ===============================
            // Professor (usuari autenticat)
            // ===============================
            var idProfessor = user.Identity?.Name
                ?? throw new InvalidOperationException("Usuari no autenticat");

            var professorExisteix = await _context.Professors
                .AnyAsync(p => p.IdProfessor == idProfessor);

            if (!professorExisteix)
                throw new InvalidOperationException("El professor no existeix");

            // ===============================
            // Alumne + context snapshot
            // ===============================
            var alumne = await _context.Alumnes
                .Include(a => a.Classe)
                .Include(a => a.Grup)
                .FirstOrDefaultAsync(a => a.NIA == nia)
                ?? throw new InvalidOperationException("Alumne no trobat");

            if (!alumne.IdClasse.HasValue)
                throw new InvalidOperationException("L’alumne no té classe");

            // ===============================
            // Avaluació (validació forta)
            // ===============================
            Avaluacio avaluacio = await _context.Avaluacions
                .FirstOrDefaultAsync(a => a.IdAvaluacio == idAvaluacio)
                ?? throw new InvalidOperationException("Avaluació no trobada");

            if (avaluacio.IdAnyEscolar != alumne.Classe.IdAnyEscolar)
                throw new InvalidOperationException("Avaluació fora del curs de l’alumne");

            if (hui < avaluacio.DataInicial || hui > avaluacio.DataFinal)
                throw new InvalidOperationException("La data no cau dins de l’avaluació");

            // ===============================
            // Categoria
            // ===============================
            var categoriaExisteix = await _context.Categories
                .AnyAsync(c => c.IdCategoria == idCategoria);

            if (!categoriaExisteix)
                throw new InvalidOperationException("Categoria no existent");

            // ===============================
            // Crear puntuació
            // ===============================
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

                DataEvent = hui,
                DataCreacio = ara,

                IdAvaluacio = idAvaluacio
            };

            _context.Puntuacions.Add(puntuacio);
            await _context.SaveChangesAsync();

            // ===============================
            // KARMA ALUMNE
            // ===============================
            KarmaAlumne karma = await _context.KarmaAlumnes
                .FirstAsync(k => k.NIA == nia && k.IdAvaluacio == idAvaluacio);

            if (tipus == "S")
                karma.NumPuntsActuals += numPunts;
            else if (tipus == "I")
                karma.NumPuntsActuals = numPunts;
            else
                throw new InvalidOperationException("Tipus de puntuació no vàlid");

            ConfiguracioKarma configuracio = await _context.ConfiguracionsKarma
                .FirstAsync(c =>
                    c.IdAnyEscolar == avaluacio.IdAnyEscolar &&
                    karma.NumPuntsActuals >= c.NumPuntsMinim &&
                    karma.NumPuntsActuals < c.NumPuntsMaxim);

            karma.KarmaActual = configuracio.ColorKarma;

            // Karma actual derivat de l’alumne
            alumne.KarmaActualPunts = karma.NumPuntsActuals;
            alumne.KarmaActualColor = configuracio.ColorKarma;

            await _context.SaveChangesAsync();

            // ===============================
            // KARMA BASE DEL GRUP
            // ===============================
            if (alumne.IdGrup.HasValue)
                await _grupService.RecalcularKarmaBaseAsync(alumne.IdGrup.Value);

            return puntuacio;
        }

        // ==================================================
        // CONSULTES
        // ==================================================
        public async Task<Puntuacio?> InstanciaAsync(long idPuntuacio)
        {
            return await _context.Puntuacions
                .Include(p => p.Alumne)
                .Include(p => p.Professor)
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.IdPuntuacio == idPuntuacio);
        }

        public async Task<List<Puntuacio>> LlistaPerAlumneAsync(string nia, long? idAvaluacio = null)
        {
            var q = _context.Puntuacions.Where(p => p.NIA == nia);

            if (idAvaluacio.HasValue)
                q = q.Where(p => p.IdAvaluacio == idAvaluacio.Value);

            return await q.OrderByDescending(p => p.DataEvent).ToListAsync();
        }

        public async Task<List<Puntuacio>> LlistaPerClasseAsync(long idClasse, long? idAvaluacio = null)
        {
            var q = _context.Puntuacions.Where(p => p.IdClasse == idClasse);

            if (idAvaluacio.HasValue)
                q = q.Where(p => p.IdAvaluacio == idAvaluacio.Value);

            return await q.OrderByDescending(p => p.DataEvent).ToListAsync();
        }

        public async Task<List<Puntuacio>> LlistaPerGrupAsync(long idGrup, long? idAvaluacio = null)
        {
            var q = _context.Puntuacions.Where(p => p.IdGrup == idGrup);

            if (idAvaluacio.HasValue)
                q = q.Where(p => p.IdAvaluacio == idAvaluacio.Value);

            return await q.OrderByDescending(p => p.DataEvent).ToListAsync();
        }
    }


}