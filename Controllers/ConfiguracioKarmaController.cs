using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/configuraciokarma")]
    public class ConfiguracioKarmaController : ControllerBase
    {
        private readonly DatabaseContext _context;


        private readonly ConfiguracioKarmaService _configuracioKarmaService;


        public ConfiguracioKarmaController(DatabaseContext context, ConfiguracioKarmaService configuracioKarmaService)
        {
            _context = context;
            _configuracioKarmaService = configuracioKarmaService;
        }

        #region Consultes

        // Instancia
        [HttpGet("{idConfiguracioKarma}")]
        [Authorize(Roles = "AG_Admin, AG_Professor")]
        public async Task<IActionResult> Instancia(int idConfiguracioKarma)
        {
            var ConfiguracioKarma = await _context.ConfiguracioKarma
                .Include(c => c.AnyEscolar)
                .FirstOrDefaultAsync(m => m.IdConfiguracioKarma == idConfiguracioKarma);

            if (ConfiguracioKarma == null)
            {
                return NotFound();
            }
            else 
            { 
                if (User.IsInRole("AG_Professor") && !ConfiguracioKarma.AnyEscolar.Actiu)
                {
                    return NotFound($"ConfiguracioKarma {idConfiguracioKarma} no trobat");
                }

            }
            // Evitar el ciclo de referencias
            ConfiguracioKarma.AnyEscolar.ConfiguracionsKarma = null;
            ConfiguracioKarma.AnyEscolar.Privilegis = null;
            ConfiguracioKarma.AnyEscolar.Grups = null;
            ConfiguracioKarma.AnyEscolar.Periodes = null;

            return Ok(ConfiguracioKarma);
        }

        // Llistes
        [HttpGet]
        [Route("llista")]
        [Authorize (Roles = "AG_Admin,AG_Professor")]
        public async Task<ActionResult<IEnumerable<ConfiguracioKarma>>> Llista()
        {
            // Comprovar si l'usuari té el rol AG_Adminque li permet accedir a totes les configuracions
            var ConfiguracioKarmaList = new List<ConfiguracioKarma>();

            if (User.IsInRole("AG_Admin"))
            {
                // Retornar totes les configuracions
                ConfiguracioKarmaList = await _context.ConfiguracioKarma
                    .Include(c => c.AnyEscolar)
                    .ToListAsync();
            } 
            else
            {   //AG_Professor
                //EXIST(AnyEscolar) WHERE(AnyEscolar.Actiu = TRUE) = TRUE
                ConfiguracioKarmaList = await _context.ConfiguracioKarma
                    .Include(c => c.AnyEscolar)
                    .Where(c => c.AnyEscolar.Actiu == true)
                    .ToListAsync();
            }
            return Ok(ConfiguracioKarmaList);
        }

        // Consulta de relacionades
        [HttpGet]
        [Route("llista-per-anyescolar")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ConfiguracioKarma>>> LlistaPerAnyEscolar(int idAnyEscolar)
        {
            var anyEscolar = await _context.AnyEscolar.FindAsync(idAnyEscolar);
            if (anyEscolar == null)
            {
                return NotFound($"Any escolar {idAnyEscolar} no trobat");
            }

            var ConfiguracioKarmaList = await _context.ConfiguracioKarma
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

        // POST: api/ConfiguracioKarma
        [HttpPost]
        [Route("crear")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<ActionResult<ConfiguracioKarma>> Crear([Required] ConfiguracioKarmaCrearDTO configuracioKarmaDTO)
        {
            if (configuracioKarmaDTO == null)
            {
                return BadRequest("ConfiguracioKarmaDTO no pot ser null");
            }

            var anyEscolar = await _context.AnyEscolar.FindAsync(configuracioKarmaDTO.IdAnyEscolar);
            if (anyEscolar == null)
            {
                return NotFound($"Any escolar {configuracioKarmaDTO.IdAnyEscolar} no trobat");
            }

            var configuracioKarma = new ConfiguracioKarmaCrearDTO
            {
                IdAnyEscolar = configuracioKarmaDTO.IdAnyEscolar,
                KarmaMinim = configuracioKarmaDTO.KarmaMinim,
                KarmaMaxim = configuracioKarmaDTO.KarmaMaxim,
                ColorNivell = configuracioKarmaDTO.ColorNivell,
                NivellPrivilegis = configuracioKarmaDTO.NivellPrivilegis
            };

            try
            {
                _configuracioKarmaService.CrearConfiguracioKarma(configuracioKarmaDTO);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(configuracioKarma);
        }
    

        //public async Task<ActionResult<ConfiguracioKarma>> Crear(ConfiguracioKarmaCrearDTO configuracioKarmaDTO)
        //{
        //    var anyEscolar = await _context.AnyEscolar.FindAsync(configuracioKarmaDTO.IdAnyEscolar);
        //    if (anyEscolar == null)
        //    {
        //        return NotFound($"Any escolar {configuracioKarmaDTO.IdAnyEscolar} no trobat");
        //    }

        //    var configuracioKarma = new ConfiguracioKarma
        //    {
        //        IdAnyEscolar = configuracioKarmaDTO.IdAnyEscolar,
        //        KarmaMinim = configuracioKarmaDTO.KarmaMinim,
        //        KarmaMaxim = configuracioKarmaDTO.KarmaMaxim,
        //        ColorNivell = configuracioKarmaDTO.ColorNivell,
        //        NivellPrivilegis = configuracioKarmaDTO.NivellPrivilegis
        //    };


        //    _context.ConfiguracioKarma.Add(configuracioKarma);
        //    await _context.SaveChangesAsync();

        //    return Ok (configuracioKarma);
        //}

        // PUT: api/ConfiguracioKarma/editar
        [HttpPut("editar")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Editar(ConfiguracioKarmaEditarDTO configuracioKarmaDto)
        {

            var configuracioKarma = await _context.ConfiguracioKarma.FindAsync(configuracioKarmaDto.IdConfiguracioKarma);

            if (configuracioKarma == null)
            {
                return NotFound($"ConfiguracioKarma {configuracioKarmaDto.IdConfiguracioKarma} no trobat");
            }

            var anyEscolar = await _context.AnyEscolar.FindAsync(configuracioKarmaDto.IdAnyEscolar);
            if (anyEscolar == null)
            {
                return NotFound($"Any escolar {configuracioKarmaDto.IdAnyEscolar} no trobat");
            }

            configuracioKarma.IdAnyEscolar = configuracioKarmaDto.IdAnyEscolar;
            configuracioKarma.KarmaMinim = configuracioKarmaDto.KarmaMinim;
            configuracioKarma.KarmaMaxim = configuracioKarmaDto.KarmaMaxim;
            configuracioKarma.ColorNivell = configuracioKarmaDto.ColorNivell;
            configuracioKarma.NivellPrivilegis = configuracioKarmaDto.NivellPrivilegis;
            

            _context.Entry(configuracioKarma).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
               return StatusCode(500, ex.InnerException != null ? ex.InnerException.Message : ex.Message);

            }

            return Ok(configuracioKarma);
        }

        // DELETE: api/ConfiguracioKarma/eliminar
        [HttpDelete("eliminar")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Eliminar(int idConfiguracioKarma)
        {
            var ConfiguracioKarma = await _context.ConfiguracioKarma.FindAsync(idConfiguracioKarma);
            if (ConfiguracioKarma == null)
            {
                return NotFound($"ConfiguracioKarma {idConfiguracioKarma} no trobat");
            }

            _context.ConfiguracioKarma.Remove(ConfiguracioKarma);
            await _context.SaveChangesAsync();

            return Ok(idConfiguracioKarma);
        }

        #endregion Serveis

        #region Auxiliars

        private bool ConfiguracioKarmaExists(int idConfiguracioKarma)
        {
            return _context.ConfiguracioKarma.Any(e => e.IdConfiguracioKarma == idConfiguracioKarma);
        }

        #endregion Auxiliars
    }
}
