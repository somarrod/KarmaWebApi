using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using KarmaWebAPI.Serveis.Interfaces;

namespace KarmaWebAPI.Controllers
{
    [Route("api/professordegrup")]
    [ApiController]

    public class ProfessorDeGrupController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ProfessorDeGrupController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/professordegrup/{id}
        [HttpGet("{id}")]
        [Authoritze(Roles = "AG_Admin,AG_Professor,AG_Alumne")]
        public async Task<ActionResult<ProfessorDeGrup>> Instancia(int id)
        {
            var professorDeGrup = await _context.ProfessorDeGrup
                .Include(p => p.Professor)
                .Include(p => p.Materia)
                .Include(p => p.AnyEscolar)
                .Include(p => p.Grup)
                .FirstOrDefaultAsync(p => p.IdProfessorDeGrup == id);

            if (professorDeGrup == null)
            {
                return NotFound();
            }

            return professorDeGrup;
        }

        // GET: api/professordegrup/llista
        [HttpGet("llista")]
        [Authoritze(Roles = "AG_Admin,AG_Professor,AG_Alumne")]
        public async Task<ActionResult<IEnumerable<ProfessorDeGrup>>> Llista()
        {
            return await _context.ProfessorDeGrup
                .Include(p => p.Professor)
                .Include(p => p.Materia)
                .Include(p => p.AnyEscolar)
                .Include(p => p.Grup)
                .ToListAsync();
        }


        // POST: api/professordegrup/crear
        [HttpPost("crear")]
        [Authoritze(Roles = "AG_Admin,AG_Professor,AG_Alumne")]
        public async Task<ActionResult<ProfessorDeGrup>> Crear(ProfessorDeGrup professorDeGrup)
        {
            _context.ProfessorDeGrup.Add(professorDeGrup);
            await _context.SaveChangesAsync();

            return Ok(professorDeGrup);
        }


        // PUT: api/professordegrup/editar
        [HttpPut("editar")]
        public async Task<IActionResult> Editar(ProfessorDeGrup professorDeGrup)
        {
            _context.Entry(professorDeGrup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfessorDeGrupExisteix(professorDeGrup.IdProfessorDeGrup))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(professorDeGrup);
        }

        // DELETE: api/professordegrup/eliminar/{id}
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var professorDeGrup = await _context.ProfessorDeGrup.FindAsync(id);
            if (professorDeGrup == null)
            {
                return NotFound();
            }

            _context.ProfessorDeGrup.Remove(professorDeGrup);
            await _context.SaveChangesAsync();

            return Ok($"ProfessorDeGrup amb Id {id} esborrat");
        }

        //private bool ProfessorDeGrupExisteix(int id)
        //{
        //    return _context.ProfessorDeGrup.Any(e => e.IdProfessorDeGrup == id);
        //}
    }
}
