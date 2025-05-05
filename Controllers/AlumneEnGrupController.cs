using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace KarmaWebAPI.Controllers
{
    [Route("api/alumneengrup")]
    [ApiController]
    public class AlumneEnGrupController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public AlumneEnGrupController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/AlumneEnGrup/5
        [HttpGet("{idAlumneEnGrup}")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_Alumne")]
        public async Task<ActionResult<AlumneEnGrup>> Instancia(int idAlumneEnGrup)
        {
            var alumneEnGrup = await _context.AlumneEnGrup.FindAsync(idAlumneEnGrup);

            if (alumneEnGrup == null)
            {
                return NotFound();
            }

            return alumneEnGrup;
        }

        // GET: api/AlumneEnGrup
        [HttpGet("llista")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_Alumne")]
        public async Task<ActionResult<IEnumerable<AlumneEnGrup>>> Llista()
        {
            var AlumneEnGrupList = new List<AlumneEnGrup>();
            var userName = User.Identity.Name;

            if (User.IsInRole("AG_Admin"))
            {
                // Retornar tots els alumnes en del grup
                AlumneEnGrupList = await _context.AlumneEnGrup
                            .Include(c => c.Grup)
                            .Include(c=> c.Alumne)
                            .ToListAsync();
            }
            else
            {
                if (User.IsInRole("AG_Professor"))
                    //AG_Professor
                    //EXIST(Grup.AnyEscolar) WHERE(Grup.AnyEscolar.Actiu = true) = true AND
                    //EXIST(Grup.ProfessorsDeGrup ) WHERE(Grup.ProfessorsDeGrup.Professor = Agent.Professor) = true
                    AlumneEnGrupList = await _context.AlumneEnGrup
                            .Include(c => c.Grup)
                            .Include(c => c.Alumne)
                            .Include(c => c.AnyEscolar)
                             .Where(c => c.AnyEscolar.Actiu == true
                                            && _context.ProfessorDeGrup.Any(pg => pg.IdProfessor == userName && pg.IdGrup == c.IdGrup))
                            .ToListAsync();
                else
                {
                    AlumneEnGrupList = await _context.AlumneEnGrup
                            .Include(c => c.Grup)
                            .Include(a => a.Alumne)
                            .Include(c => c.AnyEscolar)
                            .Where(c => c.AnyEscolar.Actiu == true && c.Alumne.NIA == userName)
                            .ToListAsync();
                }
            }

            return AlumneEnGrupList;
        }


        // POST: api/AlumneEnGrup
        [HttpPost("crear")]
        public async Task<ActionResult<AlumneEnGrup>> Crear(AlumneEnGrup alumneEnGrup)
        {
            _context.AlumneEnGrup.Add(alumneEnGrup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("ObtenerAlumneEnGrup", new { id = alumneEnGrup.IdAlumneEnGrup }, alumneEnGrup);
        }

        // PUT: api/AlumneEnGrup/5
        [HttpPut("editar")]
        public async Task<IActionResult> Editar(int idAlumneEnGrup, AlumneEnGrup alumneEnGrup)
        {
            if (idAlumneEnGrup != alumneEnGrup.IdAlumneEnGrup)
            {
                return BadRequest();
            }

            _context.Entry(alumneEnGrup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlumneEnGrupExisteix(idAlumneEnGrup))
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


        [HttpDelete("eliminar")]
        public async Task<IActionResult> Eliminar(int IdAlumneEnGrup)
        {
            var alumneEnGrup = await _context.AlumneEnGrup.FindAsync(IdAlumneEnGrup);
            if (alumneEnGrup == null)
            {
                return NotFound();
            }

            _context.AlumneEnGrup.Remove(alumneEnGrup);
            await _context.SaveChangesAsync();

            return NoContent();
                }

        private bool AlumneEnGrupExisteix(int id)
        {
            return _context.AlumneEnGrup.Any(e => e.IdAlumneEnGrup == id);
        }
    }
}
    