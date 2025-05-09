using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Serveis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KarmaWebAPI.Controllers
{
    [Route("api/puntuacio")]
    [ApiController]
    public class PuntuacioController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private PuntuacioService _puntuacioService;

        public PuntuacioController(DatabaseContext context, PuntuacioService puntuacioService)
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
        [Authorize(Roles = "AG_Admin,AG_Professor")]

       public async Task<ActionResult<Puntuacio>> Crear(PuntuacioCrearDTO puntuacioDto)
        {
            String? UsuariCreacio = null;

            var user = User.Identity.Name;

            try
            { 
                var puntuacio = await _puntuacioService.CrearPuntuacioAsync(puntuacioDto, UsuariCreacio);
                return Ok(puntuacio);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException!=null ? ex.InnerException.Message : ex.Message);
            }
           
        }


        // PUT: api/Puntuacio/editar
        [HttpPut("editar")]
        public async Task<IActionResult> Editar(int idPuntuacio, Puntuacio puntuacio)
        {
            if (idPuntuacio != puntuacio.IdPuntuacio)
            {
                return BadRequest();
            }

            _context.Entry(puntuacio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuntuacioExisteix(idPuntuacio))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/Puntuacio/eliminar
        [HttpDelete("eliminar")]
        public async Task<IActionResult> Eliminar(int idPuntuacio)
        {
            var puntuacio = await _context.Puntuacio.FindAsync(idPuntuacio);
            if (puntuacio == null)
            {
                return NotFound();
            }

            _context.Puntuacio.Remove(puntuacio);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool PuntuacioExisteix(int id)
        {
            return _context.Puntuacio.Any(e => e.IdPuntuacio == id);
        }
    }
}
