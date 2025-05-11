using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using KarmaWebAPI.Serveis.Interfaces;
using KarmaWebAPI.DTOs.DisplaySets;

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
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_Alumne")]
        public async Task<ActionResult<ProfessorDeGrup>> Instancia(int idProfessorDeGrup)
        {
            var professorDeGrup = await _context.ProfessorDeGrup
                .Include(p => p.Professor)
                .Include(p => p.Materia)
                .Include(p => p.AnyEscolar)
                .Include(p => p.Grup)
                .FirstOrDefaultAsync(p => p.IdProfessorDeGrup == idProfessorDeGrup);

            if (professorDeGrup == null)
            {
                return NotFound();
            }

            var displaySet = new ProfessorDeGrupDisplaySet
            {
                IdProfessorDeGrup = professorDeGrup.IdProfessorDeGrup,
                IdProfessor = professorDeGrup.IdProfessor,
                IdMateria = professorDeGrup.IdMateria,
                IdAnyEscolar = professorDeGrup.IdAnyEscolar,
                IdGrup = professorDeGrup.IdGrup,
                NomICognomsProfessor = (professorDeGrup.Professor?.Nom ?? "") + " " + (professorDeGrup.Professor?.Cognoms ?? ""),
                NomMateria = professorDeGrup.Materia?.Nom ?? "",
                DescripcioGrup = professorDeGrup.Grup?.Descripcio ?? ""
            };

            return Ok(displaySet);

        }

        // GET: api/professordegrup/llista
        [HttpGet("llista")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_Alumne")]
        public async Task<ActionResult<IEnumerable<ProfessorDeGrupDisplaySet>>> Llista()
        {

            var professorDeGrupList = await _context.ProfessorDeGrup
            .Include(p => p.Professor)
            .Include(p => p.Materia)
            .Include(p => p.AnyEscolar)
            .Include(p => p.Grup)
            .ToListAsync();


            var displaySetList = professorDeGrupList.Select(p => new ProfessorDeGrupDisplaySet
            {
                IdProfessorDeGrup = p.IdProfessorDeGrup,
                IdProfessor = p.IdProfessor,
                IdMateria = p.IdMateria,
                IdAnyEscolar = p.IdAnyEscolar,
                IdGrup = p.IdGrup,
                NomICognomsProfessor = (p.Professor?.Nom ?? "") + " " + (p.Professor?.Cognoms ?? ""),
                NomMateria = p.Materia?.Nom ?? "",
                DescripcioGrup = p.Grup?.Descripcio ?? ""
            }).ToList();

            return Ok(displaySetList);

        }


        // POST: api/professordegrup/crear
        [HttpPost("crear")]
        [Authorize(Roles = "AG_Admin,AG_Professor")]
        public async Task<ActionResult<ProfessorDeGrup>> Crear(ProfessorDeGrupCrearDTO professorDeGrupDto)
        {
            var jaExisteix = await _context.ProfessorDeGrup.AnyAsync(p => p.IdProfessor == professorDeGrupDto.IdProfessor &&
                                                                          p.IdAnyEscolar == professorDeGrupDto.IdAnyEscolar &&
                                                                          p.IdGrup == professorDeGrupDto.IdGrup &&
                                                                          p.IdMateria == professorDeGrupDto.IdMateria);
            if (jaExisteix)
            {
                return BadRequest($"El professor amb Id {professorDeGrupDto.IdProfessor} ja està assignat al grup i materia introduit.");
            }

            // Verificar que IdProfessor exista
            var professorExisteix = await _context.Professor.AnyAsync(p => p.IdProfessor == professorDeGrupDto.IdProfessor);
            if (!professorExisteix)
            {
                return BadRequest($"El professor amb Id {professorDeGrupDto.IdProfessor} no existeix.");
            }

            // Verificar que IdMateria exista
            var materiaExisteix = await _context.Materia.AnyAsync(m => m.IdMateria == professorDeGrupDto.IdMateria);
            if (!materiaExisteix)
            {
                return BadRequest($"La materia amb Id {professorDeGrupDto.IdMateria} no existeix.");
            }

            // Verificar que IdAnyEscolar exista
            var anyEscolarExisteix = await _context.AnyEscolar.AnyAsync(a => a.IdAnyEscolar == professorDeGrupDto.IdAnyEscolar);
            if (!anyEscolarExisteix)
            {
                return BadRequest($"L'any escolar amb Id {professorDeGrupDto.IdAnyEscolar} no existeix.");
            }

            // Verificar que IdGrup exista
            var grupExisteix = await _context.Grup.AnyAsync(g => g.IdGrup == professorDeGrupDto.IdGrup);
            if (!grupExisteix)
            {
                return BadRequest($"El grup amb Id {professorDeGrupDto.IdGrup} no existeix.");
            }

            var professorDeGrup = new ProfessorDeGrup
            {
                IdProfessor = professorDeGrupDto.IdProfessor,
                IdMateria = professorDeGrupDto.IdMateria,
                IdAnyEscolar = professorDeGrupDto.IdAnyEscolar,
                IdGrup = professorDeGrupDto.IdGrup
            };

            _context.ProfessorDeGrup.Add(professorDeGrup);
            await _context.SaveChangesAsync();

            return Ok(professorDeGrup);
        }


        // DELETE: api/professordegrup/eliminar/{id}
        [HttpDelete("eliminar")]
        public async Task<IActionResult> Eliminar(int idProfessorDeGrup)
        {
            var professorDeGrup = await _context.ProfessorDeGrup.FindAsync(idProfessorDeGrup);
            if (professorDeGrup == null)
            {
                return NotFound();
            }

            _context.ProfessorDeGrup.Remove(professorDeGrup);
            await _context.SaveChangesAsync();

            return Ok($"ProfessorDeGrup amb Id {idProfessorDeGrup} esborrat");
        }

        private bool ProfessorDeGrupExisteix(int id)
        {
            return _context.ProfessorDeGrup.Any(e => e.IdProfessorDeGrup == id);
        }
    }
}
