using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.DisplaySets;
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

        // ==================================================
        // CALCULAR KARMA BASE 
        // ==================================================
        //Wrapper Orquestrador de transaccions
        public async Task<string?> CalcularKarmaBaseAsync(
            long idGrup,
            long? idAvaluacio = null)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var tx = await _context.Database.BeginTransactionAsync();

                try
                {
                    var res = await CalcularKarmaBaseCoreAsync(
                        idGrup,
                        idAvaluacio,
                        true);

                    await tx.CommitAsync();
                    return res;
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            });
        }

        //Core
        public async Task<string?> CalcularKarmaBaseCoreAsync(
            long idGrup,
            long? idAvaluacio = null,
            bool saveChanges = true)
        {
            var grup = await _context.Grups
                .Include(g => g.Classe)
                .FirstAsync(g => g.IdGrup == idGrup);

            int idAnyEscolar = grup.Classe.IdAnyEscolar;

            // ==============================
            // OBTENIR AVALUACIÓ
            // ==============================
            Avaluacio? avaluacio;

            if (idAvaluacio.HasValue)
            {
                // cas controlat
                avaluacio = await _context.Avaluacions
                    .FirstOrDefaultAsync(a => a.IdAvaluacio == idAvaluacio.Value);
            }
            else
            {
                // avaluació actual
                DateOnly hui = DateOnly.FromDateTime(DateTime.Now);

                avaluacio = await _context.Avaluacions
                    .Where(a =>
                        a.IdAnyEscolar == idAnyEscolar &&
                        a.DataInicial <= hui &&
                        a.DataFinal >= hui)
                    .FirstOrDefaultAsync();

                // fallback: última
                if (avaluacio == null)
                {
                    avaluacio = await _context.Avaluacions
                        .Where(a => a.IdAnyEscolar == idAnyEscolar)
                        .OrderByDescending(a => a.DataFinal)
                        .FirstOrDefaultAsync();
                }
            }

            if (avaluacio == null)
                return null;

            // ==============================
            // CALCULAR PUNTS MINIMS
            // ==============================
            var puntsMinims = await _context.KarmaAlumnes
                .Where(k =>
                    k.IdAvaluacio == avaluacio.IdAvaluacio &&
                    _context.Alumnes.Any(a =>
                        a.NIA == k.NIA &&
                        a.IdGrup == idGrup))
                .MinAsync(k => (double?)k.NumPuntsActuals);

            if (puntsMinims == null)
                return null;

            // ==============================
            // CONFIGURACIÓ
            // ==============================
            var configuracio = await _context.ConfiguracionsKarma
                .Where(c =>
                    c.IdAnyEscolar == idAnyEscolar &&
                    puntsMinims >= c.NumPuntsMinim &&
                    puntsMinims < c.NumPuntsMaxim)
                .FirstAsync();

            // ==============================
            // ACTUALITZAR GRUP
            // ==============================
            grup.KarmaBase = configuracio.ColorKarma;
            grup.DataUltimaActualitzacioKarma = DateTime.Now;

            if (saveChanges)
                await _context.SaveChangesAsync();

            return grup.KarmaBase;
        }

        public async Task<GrupDisplaySet> CrearAsync(GrupCrearDTO dto)
        {
            var classe = await _context.Classes
                .FirstOrDefaultAsync(c => c.IdClasse == dto.IdClasse);

            if (classe==null)
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

            return await InstanciaAsync(grup.IdGrup,"AG_Admin", null);
        }

        public async Task<GrupDisplaySet> EditarAsync(GrupEditarDTO dto)
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

            return await InstanciaAsync(grup.IdGrup, "AG_Admin", null);
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

        public async Task<GrupDisplaySet> AfegirAlumneAsync(AssignarAlumneAGrupDTO dto)
        {
            var alumne = await _context.Alumnes
                .FirstOrDefaultAsync(a => a.NIA == dto.NIA)
                ?? throw new InvalidOperationException("Alumne no trobat");

            var grup = await _context.Grups
                .Include(g => g.Classe)
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

            // recalcular karma del grup
            await CalcularKarmaBaseAsync(dto.IdGrup);

            // tornar a carregar grup actualitzat
            grup = await _context.Grups
                .Include(g => g.Classe)
                .FirstAsync(g => g.IdGrup == dto.IdGrup);

            // obtenir alumnes del grup
            List<string> alumnes = await _context.Alumnes
                .Where(a => a.IdGrup == dto.IdGrup)
                .Select(a => a.Nom + " " + a.Cognoms)
                .ToListAsync();

            // construir display
            GrupDisplaySet result = new GrupDisplaySet
            {
                IdGrup = grup.IdGrup,
                Nom = grup.Nom,
                IdClasse = grup.IdClasse,
                NomClasse = grup.Classe.Nom,
                IdAnyEscolar = grup.Classe.IdAnyEscolar,
                Alumnes = alumnes,
                KarmaBase = grup.KarmaBase ?? "No definit"
            };

            return result;
        }

        // ==================================================
        // INSTÀNCIA
        // ==================================================
        public async Task<GrupDisplaySet?> InstanciaAsync(
            long idGrup,
            string rolUsuari,
            string? niaUsuari)
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

            // Obtenir alumnes del grup
            List<string> alumnes = await _context.Alumnes
                .Where(a => a.IdGrup == idGrup)
                .Select(a => a.Nom + " " + a.Cognoms) 
                .ToListAsync();

            // Construir el DisplaySet
            GrupDisplaySet result = new GrupDisplaySet
            {
                IdGrup = grup.IdGrup,
                Nom = grup.Nom,
                IdClasse = grup.IdClasse,
                NomClasse = grup.Classe.Nom,
                IdAnyEscolar = grup.Classe.IdAnyEscolar,
                Alumnes = alumnes,
                KarmaBase = grup.KarmaBase ?? "No definit"
            };

            return result;
        }


        // ==================================================
        // LLISTA PER CLASSE
        // ==================================================
        public async Task<List<GrupDisplaySet>> LlistaPerClasseAsync(
            long idClasse,
            string rolUsuari,
            string? niaUsuari)
        {
            List<Grup> grups;

            // Si és alumne → només el seu grup
            if (rolUsuari == "AG_Alumne")
            {
                var alumne = await _context.Alumnes
                    .FirstOrDefaultAsync(a => a.NIA == niaUsuari);

                if (alumne == null || alumne.IdClasse != idClasse || alumne.IdGrup == null)
                    return new List<GrupDisplaySet>();

                grups = await _context.Grups
                    .Include(g => g.Classe)
                    .Where(g => g.IdGrup == alumne.IdGrup)
                    .ToListAsync();
            }
            else
            {
                // No és alumne → tots els grups de la classe
                grups = await _context.Grups
                    .Include(g => g.Classe)
                    .Where(g => g.IdClasse == idClasse)
                    .OrderBy(g => g.Nom)
                    .ToListAsync();
            }

            //  Construir DisplaySet per a cada grup
            List<GrupDisplaySet> result = new List<GrupDisplaySet>();

            foreach (Grup grup in grups)
            {
                List<string> alumnes = await _context.Alumnes
                    .Where(a => a.IdGrup == grup.IdGrup)
                    .Select(a => a.Nom + " " + a.Cognoms)
                    .ToListAsync();

                result.Add(new GrupDisplaySet
                {
                    IdGrup = grup.IdGrup,
                    Nom = grup.Nom,
                    IdClasse = grup.IdClasse,
                    NomClasse = grup.Classe.Nom,
                    IdAnyEscolar = grup.Classe.IdAnyEscolar,
                    Alumnes = alumnes,
                    KarmaBase = grup.KarmaBase ?? "No definit"
                });
            }

            return result;
        }
    }

}
