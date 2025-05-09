using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;

namespace KarmaWebAPI.Controllers
{
	[Route("api/professordegrup")]
	[ApiController]
	public class ProfessorController : ControllerBase
	{
		private readonly DatabaseContext _context;

		public ProfessorController(DatabaseContext context)
		{
			_context = context;
		}

		// GET: api/Professor
		[HttpGet("llista")]
		public async Task<ActionResult<IEnumerable<Professor>>> Llista()
		{
			return await _context.Professor.ToListAsync();
		}

		// GET: api/Professor/5
		[HttpGet("{idProfessor}")]
		public async Task<ActionResult<Professor>> Instancia(string idProfessor)
		{
			var professor = await _context.Professor.FindAsync(idProfessor);

			if (professor == null)
			{
				return NotFound();
			}

			return professor;
		}

		// PUT: api/Professor/editar
		[HttpPut("editar")]
		public async Task<IActionResult> Editar(Professor professor)
		{
			_context.Entry(professor).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProfessorExisteix(idProfessor))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Ok(professor);
		}

		// POST: api/Professor/crear
		[HttpPost("crear")]
		public async Task<ActionResult<Professor>> Crear(Professor professor)
		{
			_context.Professor.Add(professor);
			await _context.SaveChangesAsync();

			return CreatedAtAction("Instancia", new { id = professor.IdProfessor }, professor);
		}

		// DELETE: api/Professor/eliminar
		[HttpDelete("eliminar")]
		public async Task<IActionResult> Eliminar(string idProfessor)
		{
			var professor = await _context.Professor.FindAsync(idProfessor);
			if (professor == null)
			{
				return NotFound();
			}

			_context.Professor.Remove(professor);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool ProfessorExisteix(string id)
		{
			return _context.Professor.Any(e => e.IdProfessor == id);
		}
	}
}
