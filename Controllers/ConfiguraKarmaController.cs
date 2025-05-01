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
    [Route("api/configuraciokarma")]
    public class ConfiguracioKarmaController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ConfiguracioKarmaController(DatabaseContext context)
        {
            _context = context;
        }

        #region Consultes

        // Instancia
        [HttpGet("{idConfiguracioKarma}")]
        [Authorize]
        public async Task<IActionResult> Instancia(int idConfiguracioKarma)
        {
            var ConfiguracioKarma = await _context.ConfiguracionsKarma
                .Include(c => c.AnyEscolar)
                .FirstOrDefaultAsync(m => m.IdConfiguracioKarma == idConfiguracioKarma);

            if (ConfiguracioKarma == null)
            {
                return NotFound();
            }

            return Ok(ConfiguracioKarma);
        }

        // Llistes
        [HttpGet]
        [Route("llista")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ConfiguracioKarma>>> Llista()
        {
            var ConfiguracioKarmaList = await _context.ConfiguracionsKarma
                .Include(c => c.AnyEscolar)
                .ToListAsync();

            return Ok(ConfiguracioKarmaList);
        }

        // Consulta de relacionades
        [HttpGet]
        [Route("llista-per-anyescolar")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ConfiguracioKarma>>> LlistaPerAnyEscolar(int idAnyEscolar)
        {
            var anyEscolar = await _context.AnysEscolar.FindAsync(idAnyEscolar);
            if (anyEscolar == null)
            {
                return NotFound($"Any escolar {idAnyEscolar} no trobat");
            }

            var ConfiguracioKarmaList = await _context.ConfiguracionsKarma
                .Include(c => c.AnyEscolar)
                .Where(c => c.IdAnyEscolar == idAnyEscolar)
                .ToListAsync();

            // Evitar el ciclo de referencias
            foreach (var ConfiguracioKarma in ConfiguracioKarmaList)
            {
                ConfiguracioKarma.AnyEscolar.ConfiguracionsKarma = null;
                ConfiguracioKarma.AnyEscolar.Privilegis = null;
                ConfiguracioKarma.AnyEscolar.Grups = null;
                ConfiguracioKarma.AnyEscolar.Periodes = null;
            }

            return Ok(ConfiguracioKarmaList);
        }

        #endregion Consultes

        #region Serveis

        // POST: api/ConfiguracioKarma
        [HttpPost]
        [Route("crear")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<ActionResult<ConfiguracioKarma>> Crear(ConfiguracioKarma ConfiguracioKarma)
        {
            _context.ConfiguracionsKarma.Add(ConfiguracioKarma);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Instancia), new { idConfiguracioKarma = ConfiguracioKarma.IdConfiguracioKarma }, ConfiguracioKarma);
        }

        // PUT: api/ConfiguracioKarma/editar
        [HttpPut("editar")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Editar(ConfiguracioKarma ConfiguracioKarma)
        {
            var anyEscolar = await _context.AnysEscolar.FindAsync(ConfiguracioKarma.IdAnyEscolar);
            if (anyEscolar == null)
            {
                return NotFound($"Any escolar {ConfiguracioKarma.IdAnyEscolar} no trobat");
            }

            _context.Entry(ConfiguracioKarma).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConfiguracioKarmaExists(ConfiguracioKarma.IdConfiguracioKarma))
                {
                    return NotFound($"ConfiguracioKarma {ConfiguracioKarma.IdConfiguracioKarma} no trobat");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/ConfiguracioKarma/eliminar
        [HttpDelete("eliminar")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Eliminar(int idConfiguracioKarma)
        {
            var ConfiguracioKarma = await _context.ConfiguracionsKarma.FindAsync(idConfiguracioKarma);
            if (ConfiguracioKarma == null)
            {
                return NotFound($"ConfiguracioKarma {idConfiguracioKarma} no trobat");
            }

            _context.ConfiguracionsKarma.Remove(ConfiguracioKarma);
            await _context.SaveChangesAsync();

            return Ok(idConfiguracioKarma);
        }

        #endregion Serveis

        #region Auxiliars

        private bool ConfiguracioKarmaExists(int idConfiguracioKarma)
        {
            return _context.ConfiguracionsKarma.Any(e => e.IdConfiguracioKarma == idConfiguracioKarma);
        }

        #endregion Auxiliars
    }
}
