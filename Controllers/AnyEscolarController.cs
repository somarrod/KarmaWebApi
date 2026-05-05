using Humanizer;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/anyescolar")]
    
    public class AnyEscolarController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IAnyEscolarService _anyEscolarService;
        private readonly IAlumneEnGrupService _alumneEnGrupService;

        public AnyEscolarController(DatabaseContext context, IAnyEscolarService anyEscolarService, IPrivilegiService privilegiService, IAlumneEnGrupService alumneEnGrupService)
        {
            _context = context;
            _anyEscolarService = anyEscolarService;
            _alumneEnGrupService = alumneEnGrupService;
        }

        #region Consultes
        // GET: api/AnyEscolar/2025
        [HttpGet("{idAnyEscolar}")]
        [Authorize]
        public async Task<ActionResult<AnyEscolar>> Instancia(int idAnyEscolar)
        {
            var anyEscolar = await _context.AnyEscolar.FindAsync(idAnyEscolar);

            if (anyEscolar == null)
            {
                return NotFound();
            }

            return Ok(anyEscolar);
        }

        // GET: api/AnyEscolar

        // GET: api/AnyEscolar/llista
        [HttpGet]
        [Route("llista")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AnyEscolar>>> Llista()
        {
            try
            {
                var anysEscolars = await _anyEscolarService.GetLlistaAsync();
                return Ok(anysEscolars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion Consultes


        #region Serveis
        // POST: api/AnyEscolar/crear
        [HttpPost]
        [Route("crear")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(Roles = "AG_Admin")]
        public async Task<ActionResult<AnyEscolar>> Crear(AnyEscolarCrearDto anyEscolarDto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                try
                {
                    var anyEscolar = await _anyEscolarService.CrearAnyEscolarAsync(anyEscolarDto);
                    return Ok(anyEscolar);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }

        #region Comentat - Editar no ha d'estar disponible
        // PUT: api/AnyEscolars/5
        
        [Authorize(Roles = "AG_Admin")]
        [HttpPut]
        public async Task<ActionResult<AnyEscolar>> Editar([FromBody] AnyEscolarEditarDto dto)
        {
            try
            {
                var anyEscolar = await _anyEscolarService.EditarAnyEscolarAsync(dto);

                if (anyEscolar == null)
                    return NotFound();

                return Ok(anyEscolar);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion Comentat - Editar no ha d'estar disponible      

        // DELETE: api/AnyEscolars/5
        [HttpDelete("eliminar")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Eliminar(int idAnyEscolar)
        {
            var anyEscolar = await _context.AnyEscolar.FindAsync(idAnyEscolar);
            if (anyEscolar == null)
            {
                return NotFound($"L'any escolar {idAnyEscolar} no s'ha trobat");
            }

            try
            {
                _context.AnyEscolar.Remove(anyEscolar);
                await _context.SaveChangesAsync();

                return Ok($"L'any escolar {idAnyEscolar} ha estat esborrat.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }
        #endregion Serveis

        #region Transaccions
        // POST: api/AnyEscolar/TCREAR
        /* [HttpPost]
         [Route("tcrear")]
         [Authorize(Roles = "AG_Admin")]
         public async Task<ActionResult<AnyEscolar>> TCREAR(AnyEscolarCrearDto anyEscolarDto)
         {
             using (var transaction = await _context.Database.BeginTransactionAsync())
             {
                 try
                 {
                     var result = await _anyEscolarService.TCREARAsync(anyEscolarDto);

                     if (result.Result is not OkObjectResult)
                     {
                         await transaction.RollbackAsync();
                         return result;
                     }

                     await transaction.CommitAsync();
                     return result;
                 }
                 catch (Exception ex)
                 {
                     await transaction.RollbackAsync();
                     return StatusCode(500, $"Internal server error: {ex.Message}");

                 }
             }
         }*/



        /*[HttpPut("actualitza-karma")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<IActionResult> ActualitzaKarma(int idAnyEscolar)
        {
            try
            {
                var result = await _anyEscolarService.ActualitzaKarmaAsync(idAnyEscolar);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }*/
        #endregion Transaccions

        #region Auxiliars

        [HttpGet("{idAnyEscolar}/exists")]
        public async Task<ActionResult<bool>> Exists(int idAnyEscolar)
        {
            var exists = await _anyEscolarService.ExistsAsync(idAnyEscolar);
            return Ok(exists);
        }

        #endregion Auxiliars
    }
}
