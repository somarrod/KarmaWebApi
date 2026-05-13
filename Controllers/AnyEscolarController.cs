using Humanizer;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public AnyEscolarController(DatabaseContext context, IAnyEscolarService anyEscolarService, IPrivilegiService privilegiService)
        {
            _context = context;
            _anyEscolarService = anyEscolarService;
        }

        #region Consultes
        // GET: api/AnyEscolar/2025
        [HttpGet("{idAnyEscolar}")]
        [Authorize]
        public async Task<ActionResult<AnyEscolar>> Instancia(int idAnyEscolar)
        {
            var anyEscolar = await _context.AnyEscolars.FindAsync(idAnyEscolar);

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
        public async Task<ActionResult<AnyEscolar>> Crear(AnyEscolarCrearDTO anyEscolarDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var anyEscolar = await _anyEscolarService.CrearAnyEscolarAsync(anyEscolarDto);

                await transaction.CommitAsync(); // COMMIT explícit
                return Ok(anyEscolar);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // ROLLBACK explícit
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // DELETE: api/AnyEscolars/5
        [HttpDelete("eliminar/{idAnyEscolar}")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Eliminar(int idAnyEscolar)
        {
            var anyEscolar = await _context.AnyEscolars.FindAsync(idAnyEscolar);
            if (anyEscolar == null)
            {
                return NotFound($"L'any escolar {idAnyEscolar} no s'ha trobat");
            }

            try
            {
                bool ok = await _anyEscolarService.EliminarAnyEscolarAsync(idAnyEscolar);
                if (!ok) return NotFound();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }


        // ==================================================
        // PUT: api/any-escolar/editar
        // ==================================================
        [HttpPut("editar")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Editar([FromBody] AnyEscolarEditarDTO dto)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var anyEscolar = await _anyEscolarService.EditarAnyEscolarAsync(dto);

                await tx.CommitAsync();
                return Ok(anyEscolar);
            }
            catch (InvalidOperationException ex)
            {
                await tx.RollbackAsync();
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return StatusCode(500, new
                {
                    error = "S'ha produït un error intern en editar l'any escolar",
                    detail = ex.Message
                });
            }
        }

        #endregion Serveis


        #region Auxiliars

        [HttpGet("exists/{idAnyEscolar}")]
        public async Task<ActionResult<bool>> Exists(int idAnyEscolar)
        {
            var exists = await _anyEscolarService.ExistsAsync(idAnyEscolar);
            return Ok(exists);
        }

        #endregion Auxiliars
    }
}
