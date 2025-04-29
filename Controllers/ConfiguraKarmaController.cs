using KarmaWebAPI.Data;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/configurakarma")]
    public class ConfiguraKarmaController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ConfiguraKarmaController(DatabaseContext context)
        {
            _context = context;
        }

        #region Consultes

        // Instancia
        [HttpGet("{idConfiguraKarma}")]
        [Authorize]
        public async Task<IActionResult> Instancia(int idConfiguraKarma)
        {
            var configuraKarma = await _context.ConfiguracionsKarma
                .Include(c => c.AnyEscolar)
                .FirstOrDefaultAsync(m => m.IdConfiguraKarma == idConfiguraKarma);

            if (configuraKarma == null)
            {
                return NotFound();
            }

            return Ok(configuraKarma);
        }

        // Llistes
        [HttpGet]
        [Route("llista")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ConfiguraKarma>>> Llista()
        {
            var configuraKarmaList = await _context.ConfiguracionsKarma
                .Include(c => c.AnyEscolar)
                .ToListAsync();

            return Ok(configuraKarmaList);
        }

        // Consulta de relacionades
        [HttpGet]
        [Route("llista-per-anyescolar")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ConfiguraKarma>>> LlistaPerAnyEscolar(int idAnyEscolar)
        {
            var anyEscolar = await _context.AnysEscolar.FindAsync(idAnyEscolar);
            if (anyEscolar == null)
            {
                return NotFound($"Any escolar {idAnyEscolar} no trobat");
            }

            var configuraKarmaList = await _context.ConfiguracionsKarma
                .Include(c => c.AnyEscolar)
                .Where(c => c.IdAnyEscolar == idAnyEscolar)
                .ToListAsync();

            // Evitar el ciclo de referencias
            foreach (var configuraKarma in configuraKarmaList)
            {
                configuraKarma.AnyEscolar.ConfiguracionsKarma = null;
                configuraKarma.AnyEscolar.Privilegis = null;
                configuraKarma.AnyEscolar.Grups = null;
                configuraKarma.AnyEscolar.Periodes = null;
            }

            return Ok(configuraKarmaList);
        }

        #endregion Consultes

        #region Serveis

        // POST: api/ConfiguraKarma
        [HttpPost]
        [Route("crear")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<ActionResult<ConfiguraKarma>> Crear(ConfiguraKarma configuraKarma)
        {
            _context.ConfiguracionsKarma.Add(configuraKarma);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Instancia), new { idConfiguraKarma = configuraKarma.IdConfiguraKarma }, configuraKarma);
        }

        // PUT: api/ConfiguraKarma/editar
        [HttpPut("editar")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Editar(ConfiguraKarma configuraKarma)
        {
            var anyEscolar = await _context.AnysEscolar.FindAsync(configuraKarma.IdAnyEscolar);
            if (anyEscolar == null)
            {
                return NotFound($"Any escolar {configuraKarma.IdAnyEscolar} no trobat");
            }

            _context.Entry(configuraKarma).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConfiguraKarmaExists(configuraKarma.IdConfiguraKarma))
                {
                    return NotFound($"ConfiguraKarma {configuraKarma.IdConfiguraKarma} no trobat");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/ConfiguraKarma/eliminar
        [HttpDelete("eliminar")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Eliminar(int idConfiguraKarma)
        {
            var configuraKarma = await _context.ConfiguracionsKarma.FindAsync(idConfiguraKarma);
            if (configuraKarma == null)
            {
                return NotFound($"ConfiguraKarma {idConfiguraKarma} no trobat");
            }

            _context.ConfiguracionsKarma.Remove(configuraKarma);
            await _context.SaveChangesAsync();

            return Ok(idConfiguraKarma);
        }

        #endregion Serveis

        #region Auxiliars

        private bool ConfiguraKarmaExists(int idConfiguraKarma)
        {
            return _context.ConfiguracionsKarma.Any(e => e.IdConfiguraKarma == idConfiguraKarma);
        }

        #endregion Auxiliars
    }
}
