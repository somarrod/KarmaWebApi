using KarmaWebAPI.Data;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Serveis
{
    using KarmaWebAPI.Data;
    using KarmaWebAPI.Models;
    using KarmaWebAPI.Serveis.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class TipusCategoriaService : ITipusCategoriaService
    {
        private readonly DatabaseContext _context;

        public TipusCategoriaService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<TipusCategoria?> InstanciaAsync(long idTipusCategoria)
        {
            return await _context.TipusCategories.FindAsync(idTipusCategoria);
        }

        public async Task<List<TipusCategoria>> LlistaAsync()
        {
            return await _context.TipusCategories.ToListAsync();
        }

        public async Task<TipusCategoria> CrearAsync(string descripcio)
        {
            var existeix = await _context.TipusCategories
                .AnyAsync(t => t.Descripcio.ToLower() == descripcio.ToLower());

            if (existeix)
                throw new InvalidOperationException("Ja existeix un tipus de categoria amb aquesta descripció");

            var tipus = new TipusCategoria
            {
                Descripcio = descripcio,
                Actiu = true
            };

            _context.TipusCategories.Add(tipus);
            await _context.SaveChangesAsync();
            return tipus;
        }

        public async Task<TipusCategoria> EditarAsync(long idTipusCategoria, string descripcio, bool actiu)
        {
            var tipus = await _context.TipusCategories.FindAsync(idTipusCategoria)
                ?? throw new InvalidOperationException("Tipus de categoria no trobat");

            tipus.Descripcio = descripcio;
            tipus.Actiu = actiu;

            await _context.SaveChangesAsync();
            return tipus;
        }

        public async Task<TipusCategoria> ActivarAsync(long idTipusCategoria)
        {
            var tipus = await _context.TipusCategories.FindAsync(idTipusCategoria)
                ?? throw new InvalidOperationException("Tipus de categoria no trobat");

            tipus.Actiu = true;
            await _context.SaveChangesAsync();
            return tipus;
        }

        public async Task<TipusCategoria> DesactivarAsync(long idTipusCategoria)
        {
            var tipus = await _context.TipusCategories.FindAsync(idTipusCategoria)
                ?? throw new InvalidOperationException("Tipus de categoria no trobat");

            tipus.Actiu = false;
            await _context.SaveChangesAsync();
            return tipus;
        }

        public async Task<bool> EliminarAsync(long idTipusCategoria)
        {
            var tipus = await _context.TipusCategories.FindAsync(idTipusCategoria);
            if (tipus == null) return false;

            _context.TipusCategories.Remove(tipus);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
