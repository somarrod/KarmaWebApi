using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.DisplaySets;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace KarmaWebAPI.Serveis
{
   public class CategoriaService : ICategoriaService
    {
        private readonly DatabaseContext _context;

        public CategoriaService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<CategoriaDisplaySet?> InstanciaAsync(long idCategoria, ClaimsPrincipal user)
        {
            var query = _context.Categories
                .AsQueryable();

            // Professors i alumnes només veuen actives
            if (user.IsInRole("AG_Professor") || user.IsInRole("AG_Alumne"))
            {
                query = query.Where(c => c.Activa);
            }

            return await query
                .Where(c => c.IdCategoria == idCategoria)
                .Select(c => new CategoriaDisplaySet
                {
                    IdCategoria = c.IdCategoria,
                    Descripcio = c.Descripcio,
                    NumPunts = c.NumPunts,
                    Editable = c.Editable,
                    Comentaris = c.Comentaris,
                    Activa = c.Activa,

                    IdTipusCategoria = c.IdTipusCategoria,
                    DescripcioTipusCategoria = c.TipusCategoria.Descripcio
                })
                .FirstOrDefaultAsync();
        }


        public async Task<List<CategoriaDisplaySet>> LlistaAsync(ClaimsPrincipal user)
        {
            var query = _context.Categories
                .AsQueryable();

            // Professors i alumnes només veuen actives
            if (user.IsInRole("AG_Professor") || user.IsInRole("AG_Alumne"))
            {
                query = query.Where(c => c.Activa);
            }

            return await query
                .Select(c => new CategoriaDisplaySet
                {
                    IdCategoria = c.IdCategoria,
                    Descripcio = c.Descripcio,
                    NumPunts = c.NumPunts,
                    Editable = c.Editable,
                    Comentaris = c.Comentaris,
                    Activa = c.Activa,

                    IdTipusCategoria = c.IdTipusCategoria,
                    DescripcioTipusCategoria = c.TipusCategoria.Descripcio
                })
                .ToListAsync();
        }

        public async Task<Categoria> CrearAsync(CategoriaCrearDTO dto)
        {
            // ===============================
            // Validar TipusCategoria
            // ===============================
            var tipusCategoria = await _context.TipusCategories
                .FirstOrDefaultAsync(t => t.IdTipusCategoria == dto.IdTipusCategoria);

            if (tipusCategoria == null)
                throw new InvalidOperationException("El tipus de categoria no existeix");

            if (!tipusCategoria.Actiu)
                throw new InvalidOperationException("El tipus de categoria no està actiu");

            // ===============================
            // Crear categoria
            // ===============================
            var categoria = new Categoria
            {
                Descripcio = dto.Descripcio,
                NumPunts = dto.NumPunts,
                Editable = dto.Editable,
                Comentaris = dto.Comentaris,
                IdTipusCategoria = dto.IdTipusCategoria,
                Activa = true
            };

            _context.Categories.Add(categoria);
            await _context.SaveChangesAsync();

            return categoria;
        }

        public async Task<Categoria> EditarAsync(CategoriaEditarDTO dto)
        {
            var categoria = await _context.Categories.FindAsync(dto.IdCategoria)
                ?? throw new InvalidOperationException("Categoria no trobada");

            categoria.Descripcio = dto.Descripcio;
            categoria.NumPunts = dto.NumPunts;
            categoria.Editable = dto.Editable;
            categoria.Comentaris = dto.Comentaris;
            categoria.IdTipusCategoria = dto.IdTipusCategoria;
            categoria.Activa = dto.Activa;

            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<Categoria> ActivarAsync(long idCategoria)
        {
            var categoria = await _context.Categories.FindAsync(idCategoria)
                ?? throw new InvalidOperationException("Categoria no trobada");

            categoria.Activa = true;
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<Categoria> DesactivarAsync(long idCategoria)
        {
            var categoria = await _context.Categories.FindAsync(idCategoria)
                ?? throw new InvalidOperationException("Categoria no trobada");

            categoria.Activa = false;
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<bool> EliminarAsync(long idCategoria)
        {
            var categoria = await _context.Categories.FindAsync(idCategoria);
            if (categoria == null) return false;

            _context.Categories.Remove(categoria);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
