using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using KarmaWebAPI.Serveis.Interfaces;

namespace KarmaWebAPI.Controllers
{
    [Route("api/puntuacio")]
    [ApiController]
    public class PuntuacioController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private IPuntuacioService _puntuacioService;

        public PuntuacioController(DatabaseContext context, IPuntuacioService puntuacioService)
        {
            _context = context;
            _puntuacioService = puntuacioService;
        }

        // GET: api/Puntuacio
        [HttpGet("llista")]
        public async Task<ActionResult<IEnumerable<Puntuacio>>> Llista()
        {
            return await _context.Puntuacio.ToListAsync();
        }

        // GET: api/Puntuacio/5
        [HttpGet("{idPuntuacio}")]
        public async Task<ActionResult<Puntuacio>> Instancia(int idPuntuacio)
        {
            var puntuacio = await _context.Puntuacio.FindAsync(idPuntuacio);

            if (puntuacio == null)
            {
                return NotFound();
            }

            return puntuacio;
        }

        // POST: api/Puntuacio/crear
        [HttpPost("crear")]
        public async Task<ActionResult<Puntuacio>> Crear(PuntuacioCrearDTO puntuacioDto)
        {
            String? usuariCreacio  = User.Identity.Name;

            try
            { 
                var puntuacio = await _puntuacioService.CrearPuntuacioAsync(puntuacioDto, usuariCreacio);
                return Ok(puntuacio);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException!=null ? ex.InnerException.Message : ex.Message);
            }
           
        }

        [HttpPost("tcrear")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<ActionResult<Puntuacio>> TCREAR(PuntuacioCrearDTO puntuacioDto)
        {
            String? usuariCreacio = User.Identity.Name;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var puntuacio = await _puntuacioService.TCREARAsync(puntuacioDto, usuariCreacio);

                await transaction.CommitAsync();
                return Ok(puntuacio);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }


        // DELETE: api/Puntuacio/eliminar
        [HttpDelete("teliminar")]
        public async Task<IActionResult> TELIMINAR(int idPuntuacio)
        {

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var resultat = await _puntuacioService.TELIMINARAsync(idPuntuacio);

                await transaction.CommitAsync();
                return resultat.Result;// Convertir explícitamente a IActionResult
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }


        private bool PuntuacioExisteix(int id)
        {
            return _context.Puntuacio.Any(e => e.IdPuntuacio == id);
        }
    }
}
