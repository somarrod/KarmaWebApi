using System;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace KarmaWebAPI.Serveis
{

    public class GrupService : IGrupService
    {
        private readonly DatabaseContext _context;

        public GrupService(DatabaseContext context)
        {
            _context = context;
        }


        public async Task<string?> RecalcularKarmaBaseAsync(long idGrup)
        {
            var grup = await _context.Grups
                .Include(g => g.Classe)
                .FirstAsync(g => g.IdGrup == idGrup);

            long idAnyEscolar = grup.Classe.IdAnyEscolar;
            DateOnly hui = DateOnly.FromDateTime(DateTime.Now);

            // 🔑 Avaluació en curs PER ANY ESCOLAR
            var avaluacio = await _context.Avaluacions
                .Where(a =>
                    a.IdAnyEscolar == idAnyEscolar &&
                    a.DataInicial <= hui &&
                     a.DataFinal >= hui)
                .FirstOrDefaultAsync();

            if (avaluacio == null)
                return null;

            // alumnes del grup
            var puntsMinims = await _context.KarmaAlumnes
                .Where(k =>
                    k.IdAvaluacio == avaluacio.IdAvaluacio &&
                    _context.Alumnes.Any(a =>
                        a.NIA == k.NIA &&
                        a.IdGrup == idGrup))
                .MinAsync(k => (double?)k.NumPuntsActuals);

            if (puntsMinims == null)
                return null;

            var configuracio = await _context.ConfiguracionsKarma
                .Where(c =>
                    c.IdAnyEscolar == idAnyEscolar &&
                    puntsMinims >= c.NumPuntsMinim &&
                    puntsMinims < c.NumPuntsMaxim)
                .FirstAsync();

            grup.KarmaBase = configuracio.ColorKarma;
            grup.DataUltimaActualitzacioKarma = DateTime.Now;

            await _context.SaveChangesAsync();
            return grup.KarmaBase;
        }


        public async Task<Grup> CrearAsync(long idClasse, string nom)
        {
            var classeExisteix = await _context.Classes
                .AnyAsync(c => c.IdClasse == idClasse);

            if (!classeExisteix)
                throw new InvalidOperationException("Classe inexistent");

            var grup = new Grup
            {
                IdClasse = idClasse,
                Nom = nom
            };

            _context.Grups.Add(grup);
            await _context.SaveChangesAsync();
            return grup;
        }

        public async Task<bool> EsborrarAsync(long idGrup)
        {
            var grup = await _context.Grups.FindAsync(idGrup);
            if (grup == null) return false;

            // alumnes del grup → grup a null
            var alumnes = await _context.Alumnes
                .Where(a => a.IdGrup == idGrup)
                .ToListAsync();

            foreach (var a in alumnes)
                a.IdGrup = null;

            _context.Grups.Remove(grup);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Alumne> AfegirAlumneAsync(long idGrup, string nia)
        {
            var alumne = await _context.Alumnes
                .FirstOrDefaultAsync(a => a.NIA == nia)
                ?? throw new InvalidOperationException("Alumne no trobat");

            var grup = await _context.Grups
                .FirstOrDefaultAsync(g => g.IdGrup == idGrup)
                ?? throw new InvalidOperationException("Grup no trobat");

            // coherència Classe
            if (alumne.IdClasse != grup.IdClasse)
                throw new InvalidOperationException(
                    "L'alumne no pertany a la classe del grup");

            alumne.IdGrup = idGrup;
            await _context.SaveChangesAsync();

            await RecalcularKarmaBaseAsync(idGrup);
            return alumne;
        }

        public async Task<Alumne> LlevarAlumneAsync(string nia)
        {
            var alumne = await _context.Alumnes
                .FirstOrDefaultAsync(a => a.NIA == nia)
                ?? throw new InvalidOperationException("Alumne no trobat");

            var idGrupAnterior = alumne.IdGrup;
            alumne.IdGrup = null;

            await _context.SaveChangesAsync();

            if (idGrupAnterior.HasValue)
                await RecalcularKarmaBaseAsync(idGrupAnterior.Value);

            return alumne;
        }

        // ==================================================
        // INSTÀNCIA
        // ==================================================
        public async Task<Grup?> InstanciaAsync(long idGrup)
        {
            return await _context.Grups
                .Include(g => g.Classe)
                .FirstOrDefaultAsync(g => g.IdGrup == idGrup);
        }

        // ==================================================
        // LLISTA PER CLASSE
        // ==================================================
        public async Task<List<Grup>> LlistaPerClasseAsync(long idClasse)
        {
            return await _context.Grups
                .Where(g => g.IdClasse == idClasse)
                .OrderBy(g => g.Nom)
                .ToListAsync();
        }

    }

}
