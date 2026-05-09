using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
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

        public async Task<Categoria?> InstanciaAsync(long idCategoria, ClaimsPrincipal user)
        {
            var categoria = await _context.Categories
                .Include(c => c.TipusCategoria)
                .FirstOrDefaultAsync(c => c.IdCategoria == idCategoria);

            if (categoria == null) return null;

            if (user.IsInRole("AG_Professor") && !categoria.Activa)
                return null;

            return categoria;
        }

        public async Task<List<Categoria>> LlistaAsync(ClaimsPrincipal user)
        {
            var query = _context.Categories
                .Include(c => c.TipusCategoria)
                .AsQueryable();

            if (user.IsInRole("AG_Professor"))
                query = query.Where(c => c.Activa);

            return await query.ToListAsync();
        }

        public async Task<Categoria> CrearAsync(CategoriaCrearDTO dto)
        {
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
