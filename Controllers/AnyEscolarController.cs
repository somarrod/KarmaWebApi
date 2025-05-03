using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/anyescolar")]
    
    public class AnyEscolarController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IAnyEscolarService _anyEscolarService;
        //private readonly IPrivilegiService _privilegiService;

        public AnyEscolarController(DatabaseContext context, IAnyEscolarService anyEscolarService, IPrivilegiService privilegiService)
        {
            _context = context;
            _anyEscolarService = anyEscolarService;
            //_privilegiService = privilegiService;


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
        [HttpGet]
        [Route("llista")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AnyEscolar>>> Llista()
        {
            var anyEscolar = await _context.AnyEscolar.ToListAsync();
            return Ok(anyEscolar);
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
                    var result = await _anyEscolarService.CrearAnyEscolarAsync(anyEscolarDto);

                    if (result.Result is OkObjectResult)
                    {
                        await transaction.CommitAsync();
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }

        #region Comentat - Editar no ha d'estar disponible
        // PUT: api/AnyEscolars/5
        /*
        [Authorize(Roles = "AG_Admin")]
        [HttpPut("editar")]
        public async Task<IActionResult> Editar(AnyEscolarEditarDto anyEscolarDto)
        {
            if (anyEscolarDto == null) {
                return BadRequest();
            }

            var anyEscolar = new AnyEscolar
            {
                IdAnyEscolar = anyEscolarDto.IdAnyEscolar,
                DataIniciCurs = anyEscolarDto.DataIniciCurs,
                DataFiCurs = anyEscolarDto.DataFiCurs,
                Actiu = anyEscolarDto.Actiu,
                DiesPeriode = anyEscolarDto.DiesPeriode
            };

            _context.Entry(anyEscolar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnyEscolarExists(anyEscolarDto.IdAnyEscolar))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }
        */
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

                return Ok(idAnyEscolar);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }
        #endregion Serveis

        #region Transaccions
        // POST: api/AnyEscolar/TCREAR
        [HttpPost]
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
        }
        #endregion Transaccions

        #region Auxiliars
        private bool AnyEscolarExists(int id_AnyEscolar)
        {
            return _context.AnyEscolar.Any(e => e.IdAnyEscolar == id_AnyEscolar);
        }
        #endregion Auxiliars
    }
}
