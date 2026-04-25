using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.Models;
using KarmaWebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace KarmaWebAPI.Controllers
{
    [Route("api/tipuscategoria")]
    [ApiController]
    public class TipusCategoriaController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public TipusCategoriaController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("llista")]
        public async Task<ActionResult<IEnumerable<TipusCategoria>>> Llista()
        {
            return await _context.TipusCategoria
                .Include(t => t.Categories)
                .ToListAsync();
        }

        [HttpPost("crear")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<ActionResult<TipusCategoria>> Crear(TipusCategoriaCrearDTO dto)
        {
            var tipus = new TipusCategoria
            {
                Descripcio = dto.Descripcio
            };

            _context.TipusCategoria.Add(tipus);
            await _context.SaveChangesAsync();

            return Ok(tipus);
        }

        [HttpDelete("eliminar")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Eliminar(int idTipusCategoria)
        {
            var tipus = await _context.TipusCategoria.FindAsync(idTipusCategoria);

            if (tipus == null)
                return NotFound();

            _context.TipusCategoria.Remove(tipus);
            await _context.SaveChangesAsync();

            return Ok("Tipus de categoria eliminat");
        }
    }
}