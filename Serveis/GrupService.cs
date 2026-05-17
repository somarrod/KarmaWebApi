using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Security.Cryptography;

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

            int idAnyEscolar = grup.Classe.IdAnyEscolar;
            DateOnly hui = DateOnly.FromDateTime(DateTime.Now);

            // Avaluació en curs PER ANY ESCOLAR
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

        public async Task<Grup> CrearAsync(GrupCrearDTO dto)
        {
            var classeExisteix = await _context.Classes
                .AnyAsync(c => c.IdClasse == dto.IdClasse);

            if (!classeExisteix)
                throw new InvalidOperationException("Classe inexistent");

            // ✅ Validar duplicat dins de la mateixa classe
            bool existeixDuplicat = await _context.Grups
                .AnyAsync(g =>
                    g.IdClasse == dto.IdClasse &&
                    g.Nom == dto.Nom);

            if (existeixDuplicat)
                throw new InvalidOperationException(
                    "Ja existeix un grup amb aquest nom dins de la mateixa classe");

            var grup = new Grup
            {
                IdClasse = dto.IdClasse,
                Nom = dto.Nom
            };

            _context.Grups.Add(grup);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException(
                    ex.InnerException?.Message ?? ex.Message);
            }

            return grup;
        }

        public async Task<Grup> EditarAsync(GrupEditarDTO dto)
        {
            var grup = await _context.Grups
                .FirstOrDefaultAsync(g => g.IdGrup == dto.IdGrup);

            if (grup == null)
                throw new InvalidOperationException("El grup no existeix");

            // Validar duplicat dins de la mateixa classe
            bool existeixDuplicat = await _context.Grups
                .AnyAsync(g =>
                    g.IdGrup != dto.IdGrup &&
                    g.IdClasse == grup.IdClasse &&
                    g.Nom == dto.Nom);

            if (existeixDuplicat)
                throw new InvalidOperationException(
                    "Ja existeix un grup amb aquest nom dins de la mateixa classe");

            //  Actualitzar només el nom
            grup.Nom = dto.Nom;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException(
                    ex.InnerException?.Message ?? ex.Message);
            }

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
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {

                throw new InvalidOperationException(
                    ex.InnerException?.Message ?? ex.Message);

            }
            return true;
        }

        public async Task<Alumne> AfegirAlumneAsync(AssignarAlumneAGrupDTO dto)
        {
            var alumne = await _context.Alumnes
                .FirstOrDefaultAsync(a => a.NIA == dto.NIA)
                ?? throw new InvalidOperationException("Alumne no trobat");

            var grup = await _context.Grups
                .FirstOrDefaultAsync(g => g.IdGrup == dto.IdGrup)
                ?? throw new InvalidOperationException("Grup no trobat");

            // coherència Classe
            if (alumne.IdClasse != grup.IdClasse)
                throw new InvalidOperationException(
                    "L'alumne no pertany a la classe del grup");

            // assignar (substitueix si estava en un altre grup)
            alumne.IdGrup = dto.IdGrup;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException(
                    ex.InnerException?.Message ?? ex.Message);
            }

            // recalcular només aquest grup
            await RecalcularKarmaBaseAsync(dto.IdGrup);

            return alumne;
        }

        public async Task<Alumne> LlevarAlumneAsync(string nia)
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
                await RecalcularKarmaBaseAsync(idGrupAnterior.Value);

            return alumne;
        }

        // ==================================================
        // INSTÀNCIA
        // ==================================================
        public async Task<Grup?> InstanciaAsync(long idGrup, string rolUsuari, string? niaUsuari)
        {
                    var grup = await _context.Grups
                        .Include(g => g.Classe)
                        .FirstOrDefaultAsync(g => g.IdGrup == idGrup);

                    if (grup == null)
                           return null;

            // Si és alumne → només pot veure el seu grup
            if (rolUsuari == "AG_Alumne")
                    {
                        var alumne = await _context.Alumnes
                            .FirstOrDefaultAsync(a => a.NIA == niaUsuari);

                        if (alumne == null || alumne.IdGrup != idGrup)
                           return null;
                    }

                    return grup;
        }

        // ==================================================
        // LLISTA PER CLASSE
        // ==================================================
        public async Task<List<Grup>> LlistaPerClasseAsync(long idClasse, string rolUsuari, string? niaUsuari)
        {

            // Si NO és alumne → llista completa
            if (rolUsuari != "AG_Alumne")
            {
                return await _context.Grups
                    .Where(g => g.IdClasse == idClasse)
                    .OrderBy(g => g.Nom)
                    .ToListAsync();
            }

            // Si és alumne → només el seu grup
            var alumne = await _context.Alumnes
                .FirstOrDefaultAsync(a => a.NIA == niaUsuari);

            if (alumne == null || alumne.IdClasse != idClasse || alumne.IdGrup == null)
                return new List<Grup>(); // buida (no accés)

            var grup = await _context.Grups
                .Where(g => g.IdGrup == alumne.IdGrup)
                .ToListAsync();

            return grup;
        }
    }

}
