using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs.DisplaySets;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Serveis
{


    public class ProfessorDeClasseService : IProfessorDeClasseService
    {
        private readonly DatabaseContext _context;

        public ProfessorDeClasseService(DatabaseContext context)
        {
            _context = context;
        }


        // ============================
        // CREAR (ASSIGNAR)
        // ============================
        public async Task<ProfessorDeClasse> AssignarAsync(string idProfessor, string idClasse, long idMateria)
        {
            // Ja existeix?
            var jaExisteix = await _context.ProfessorsDeClasse.AnyAsync(p =>
                p.IdProfessor == idProfessor &&
                p.IdClasse == idClasse &&
                p.IdMateria == idMateria);

            if (jaExisteix)
                throw new InvalidOperationException(
                    $"El professor amb Id {idProfessor} ja està assignat a la classe i matèria.");

            // Professor existeix?
            if (!await _context.Professor.AnyAsync(p => p.IdProfessor == idProfessor))
                throw new InvalidOperationException(
                    $"El professor amb Id {idProfessor} no existeix.");

            // Classe existeix?
            if (!await _context.Classes.AnyAsync(c => c.IdClasse == idClasse))
                throw new InvalidOperationException(
                    $"La classe amb Id {idClasse} no existeix.");

            // Matèria existeix?
            if (!await _context.Materia.AnyAsync(m => m.IdMateria == idMateria))
                throw new InvalidOperationException(
                    $"La matèria amb Id {idMateria} no existeix.");

            var relacio = new ProfessorDeClasse
            {
                IdProfessor = idProfessor,
                IdClasse = idClasse,
                IdMateria = idMateria
            };

            _context.ProfessorsDeClasse.Add(relacio);
            await _context.SaveChangesAsync();

            return relacio;
        }

        // ============================
        // ESBORRAR
        // ============================
        public async Task<bool> EsborrarAsync(string idProfessor,string idClasse, long idMateria)
        {
            var relacio = await _context.ProfessorsDeClasse
                .FirstOrDefaultAsync(p =>
                    p.IdProfessor == idProfessor &&
                    p.IdClasse == idClasse &&
                    p.IdMateria == idMateria);

            if (relacio == null)
                return false;

            _context.ProfessorsDeClasse.Remove(relacio);
            await _context.SaveChangesAsync();
            return true;
        }

        // ============================
        // INSTÀNCIA (per id)
        // ============================
        public async Task<ProfessorDeClasseDisplaySet?> InstanciaAsync(long idProfessorDeClasse)
        {
            return await _context.ProfessorsDeClasse
                .Include(p => p.Professor)
                .Include(p => p.Classe)
                .Include(p => p.Materia)
                .Where(p => p.IdProfessorDeClasse == idProfessorDeClasse)
                .Select(p => new ProfessorDeClasseDisplaySet
                {
                    IdProfessorDeClasse = p.IdProfessorDeClasse,

                    IdProfessor = p.IdProfessor,
                    NomICognomsProfessor =
                        (p.Professor.Nom ?? "") + " " + (p.Professor.Cognoms ?? ""),

                    IdClasse = p.IdClasse,
                    NomClasse = p.Classe.Nom,

                    IdMateria = p.IdMateria,
                    NomMateria = p.Materia.Nom
                })
                .FirstOrDefaultAsync();
        }

        // ============================
        // LLISTA COMPLETA
        // ============================
        public async Task<List<ProfessorDeClasseDisplaySet>> GetLlistaAsync()
        {
            return await _context.ProfessorsDeClasse
                .Include(p => p.Professor)
                .Include(p => p.Classe)
                .Include(p => p.Materia)
                .Select(p => new ProfessorDeClasseDisplaySet
                {
                    IdProfessorDeClasse = p.IdProfessorDeClasse,

                    IdProfessor = p.IdProfessor,
                    NomICognomsProfessor =
                        (p.Professor.Nom ?? "") + " " + (p.Professor.Cognoms ?? ""),

                    IdClasse = p.IdClasse,
                    NomClasse = p.Classe.Nom,

                    IdMateria = p.IdMateria,
                    NomMateria = p.Materia.Nom
                })
                .OrderBy(p => p.NomClasse)
                .ThenBy(p => p.NomMateria)
                .ThenBy(p => p.NomICognomsProfessor)
                .ToListAsync();
        }
        
        // ============================
        // VERIFICACIONS
        // ============================

        // Dona classe en eixa classe (independentment de la matèria)
        public async Task<bool> ImparteixClasseAsync(string idProfessor, string idClasse)
        {
            return await _context.ProfessorsDeClasse.AnyAsync(p =>
                p.IdProfessor == idProfessor &&
                p.IdClasse == idClasse);
        }

        // Dona eixa matèria en eixa classe
        public async Task<bool> ImparteixMateriaEnClasseAsync(string idProfessor, string idClasse, long idMateria)
        {
            return await _context.ProfessorsDeClasse.AnyAsync(p =>
                p.IdProfessor == idProfessor &&
                p.IdClasse == idClasse &&
                p.IdMateria == idMateria);
        }

    }
}
