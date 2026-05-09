using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace KarmaWebAPI.Serveis
{
    public class MateriaService : IMateriaService
    {
        private readonly DatabaseContext _context;

        public MateriaService(DatabaseContext context)
        {
            _context = context;
        }

        // =========================
        // INSTÀNCIA
        // =========================
        public async Task<Materia?> InstanciaAsync(long idMateria, ClaimsPrincipal user)
        {
            var materia = await _context.Materies.FindAsync(idMateria);
            if (materia == null) return null;

            // Professor → només pot veure actives
            if (user.IsInRole("AG_Professor") && !materia.Activa)
                return null;

            return materia;
        }

        // =========================
        // LLISTA
        // =========================
        public async Task<List<Materia>> LlistaAsync(ClaimsPrincipal user)
        {
            if (user.IsInRole("AG_Admin"))
            {
                return await _context.Materies.ToListAsync();
            }

            // Professor → només actives
            return await _context.Materies
                .Where(m => m.Activa)
                .ToListAsync();
        }

        // =========================
        // CREAR
        // =========================
        public async Task<Materia> CrearAsync(MateriaCrearDTO dto)
        {
            var existeix = await _context.Materies
                .AnyAsync(m => m.Nom.ToLower() == dto.Nom.ToLower());

            if (existeix)
                throw new InvalidOperationException("Ja existeix una matèria amb aquest nom.");

            var materia = new Materia
            {
                Nom = dto.Nom,
                Activa = true
            };

            _context.Materies.Add(materia);
            await _context.SaveChangesAsync();

            return materia;
        }

        // =========================
        // EDITAR
        // =========================
        public async Task<Materia> EditarAsync(MateriaEditarDTO dto)
        {
            var existeix = await _context.Materies.AnyAsync(m =>
                m.Nom.ToLower() == dto.Nom.ToLower() &&
                m.IdMateria != dto.IdMateria);

            if (existeix)
                throw new InvalidOperationException("Ja existeix una matèria amb aquest nom.");

            var materia = await _context.Materies.FindAsync(dto.IdMateria)
                ?? throw new InvalidOperationException("Matèria no trobada");

            materia.Nom = dto.Nom;
            materia.Activa = dto.Activa;

            await _context.SaveChangesAsync();
            return materia;
        }

        // =========================
        // ACTIVAR / DESACTIVAR
        // =========================
        public async Task<Materia> ActivarAsync(long idMateria)
        {
            var materia = await _context.Materies.FindAsync(idMateria)
                ?? throw new InvalidOperationException("La matèria indicada no existeix");

            materia.Activa = true;
            await _context.SaveChangesAsync();
            return materia;
        }

        public async Task<Materia> DesactivarAsync(long idMateria)
        {
            var materia = await _context.Materies.FindAsync(idMateria)
                ?? throw new InvalidOperationException("La matèria indicada no existeix");

            materia.Activa = false;
            await _context.SaveChangesAsync();
            return materia;
        }

        // =========================
        // ELIMINAR
        // =========================
        public async Task<bool> EliminarAsync(long idMateria)
        {
            var materia = await _context.Materies.FindAsync(idMateria);
            if (materia == null) return false;

            _context.Materies.Remove(materia);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
