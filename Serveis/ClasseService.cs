using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.DisplaySets;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Serveis
{
    public class ClasseService : IClasseService
    {
        private readonly DatabaseContext _context;

        public ClasseService(DatabaseContext context)
        {
            _context = context;
        }

        // =====================================================
        // CONSULTES
        // =====================================================

        public async Task<List<ClasseDisplaySet>> LlistaAsync(int idAnyEscolar)
        {
            return await _context.Classes
                .Where(c => c.IdAnyEscolar == idAnyEscolar)
                .OrderBy(c => c.Nom)
                .Select(c => new ClasseDisplaySet
                {
                    IdClasse = c.IdClasse,
                    Nom = c.Nom,
                    IdAnyEscolar = c.IdAnyEscolar
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ClasseDisplaySet> InstanciaAsync(long idClasse)
        {
            var result = await _context.Classes
                .Where(c => c.IdClasse == idClasse)
                .Select(c => new ClasseDisplaySet
                {
                    IdClasse = c.IdClasse,
                    Nom = c.Nom,
                    IdAnyEscolar = c.IdAnyEscolar
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result == null)
                throw new InvalidOperationException("La classe no existeix");

            return result;
        }

        // =====================================================
        // CREAR
        // =====================================================

        public async Task<ClasseDisplaySet> CrearAsync(ClasseCrearDTO dto)
        {
            bool anyExisteix = await _context.AnyEscolars
                .AnyAsync(a => a.IdAnyEscolar == dto.IdAnyEscolar);

            if (!anyExisteix)
                throw new InvalidOperationException("L'any escolar no existeix");

            bool existeixDuplicat = await _context.Classes
                .AnyAsync(c =>
                    c.IdAnyEscolar == dto.IdAnyEscolar &&
                    c.Nom == dto.Nom);

            if (existeixDuplicat)
                throw new InvalidOperationException(
                    "Ja existeix una classe amb aquest nom dins del mateix any escolar");

            var classe = new Classe
            {
                Nom = dto.Nom,
                IdAnyEscolar = dto.IdAnyEscolar
            };

            _context.Classes.Add(classe);
            await _context.SaveChangesAsync();

            return await InstanciaAsync(classe.IdClasse);
        }

        // =====================================================
        // EDITAR
        // =====================================================

        public async Task<ClasseDisplaySet> EditarAsync(ClasseEditarDTO dto)
        {
            var classe = await _context.Classes
                .FirstOrDefaultAsync(c => c.IdClasse == dto.IdClasse);

            if (classe == null)
                throw new InvalidOperationException("La classe no existeix");

            bool existeixDuplicat = await _context.Classes
                .AnyAsync(c =>
                    c.IdClasse != dto.IdClasse &&
                    c.IdAnyEscolar == classe.IdAnyEscolar &&
                    c.Nom == dto.Nom);

            if (existeixDuplicat)
                throw new InvalidOperationException(
                    "Ja existeix una altra classe amb aquest nom dins del mateix any escolar");

            classe.Nom = dto.Nom;

            await _context.SaveChangesAsync();

            return await InstanciaAsync(classe.IdClasse);
        }

        // =====================================================
        // ESBORRAR
        // =====================================================

        public async Task EsborrarAsync(long idClasse)
        {
            var classe = await _context.Classes
                .FirstOrDefaultAsync(c => c.IdClasse == idClasse);

            if (classe == null)
                throw new InvalidOperationException("La classe no existeix");

            bool teDependencies =
                await _context.Alumnes.AnyAsync(a => a.IdClasse == idClasse) ||
                await _context.Grups.AnyAsync(g => g.IdClasse == idClasse);

            if (teDependencies)
                throw new InvalidOperationException(
                    "No es pot eliminar la classe perquè té dependències associades");

            _context.Classes.Remove(classe);
            await _context.SaveChangesAsync();
        }
    }
}