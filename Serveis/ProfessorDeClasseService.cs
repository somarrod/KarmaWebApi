using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
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
        public async Task<ProfessorDeClasseDisplaySet> AssignarProfessorAClasseAsync(ProfessorDeClasseCrearDTO dto)
        {
            // Ja existeix?
            var jaExisteix = await _context.ProfessorsDeClasse.AnyAsync(p =>
                p.IdProfessor == dto.IdProfessor &&
                p.IdClasse == dto.IdClasse &&
                p.IdMateria == dto.IdMateria);

            if (jaExisteix)
                throw new InvalidOperationException(
                    $"El professor amb Id {dto.IdProfessor} ja està assignat a la classe i matèria.");

            // Professor existeix?
            if (!await _context.Professors.AnyAsync(p => p.IdProfessor == dto.IdProfessor))
                throw new InvalidOperationException(
                    $"El professor amb Id {dto.IdProfessor} no existeix.");

            // Classe existeix?
            if (!await _context.Classes.AnyAsync(c => c.IdClasse == dto.IdClasse))
                throw new InvalidOperationException(
                    $"La classe amb Id {dto.IdClasse} no existeix.");

            // Matèria existeix?
            if (!await _context.Materies.AnyAsync(m => m.IdMateria == dto.IdMateria))
                throw new InvalidOperationException(
                    $"La matèria amb Id {dto.IdMateria} no existeix.");
            

            var relacio = new ProfessorDeClasse
            {
                IdProfessor = dto.IdProfessor,
                IdClasse = dto.IdClasse,
                IdMateria = dto.IdMateria
            };

            _context.ProfessorsDeClasse.Add(relacio);
            await _context.SaveChangesAsync();

            return await InstanciaAsync(relacio.IdProfessorDeClasse);
        }

        // ============================
        // ESBORRAR
        // ============================
        public async Task<bool> EsborrarAsync(long idProfessorDeClasse)
        {
            var relacio = await _context.ProfessorsDeClasse
                .FirstOrDefaultAsync(p =>
                    p.IdProfessorDeClasse == idProfessorDeClasse);

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

        // ================================
        // LLISTA COMPLETA per AnyEscolar
        // ================================
        public async Task<List<ProfessorDeClasseDisplaySet>> GetLlistaAsync(int idAnyEscolar)
        {
            return await _context.ProfessorsDeClasse
                .Where(p => p.Classe.IdAnyEscolar == idAnyEscolar)
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
                .AsNoTracking()
                .ToListAsync();
        }

        // ============================
        // VERIFICACIONS
        // ============================

        // Dona classe en eixa classe (independentment de la matèria)
        public async Task<bool> ImparteixClasseAsync(string idProfessor, long idClasse)
        {
            return await _context.ProfessorsDeClasse.AnyAsync(p =>
                p.IdProfessor == idProfessor &&
                p.IdClasse == idClasse);
        }

        // Dona eixa matèria en eixa classe
        public async Task<bool> ImparteixMateriaEnClasseAsync(string idProfessor, long idClasse, long idMateria)
        {
            return await _context.ProfessorsDeClasse.AnyAsync(p =>
                p.IdProfessor == idProfessor &&
                p.IdClasse == idClasse &&
                p.IdMateria == idMateria);
        }

    }
}
