using KarmaWebAPI.Data;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/AnyEscolar")]
    
    public class AnyEscolarController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public AnyEscolarController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/AnyEscolars
        [HttpGet]
        [Route("llista")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AnyEscolar>>> GetAnyEscolar()
        {
            var anyEscolar = await _context.AnyEscolar.ToListAsync();
            return Ok(anyEscolar);
        }

        // GET: api/AnyEscolars/2025
        [HttpGet("{id_anyEscolar}")]
        [Authorize]
        public async Task<ActionResult<AnyEscolar>> GetAnyEscolar(int id_anyEscolar)
        {
            var anyEscolar = await _context.AnyEscolar.FindAsync(id_anyEscolar);

            if (anyEscolar == null)
            {
                return NotFound();
            }

            return Ok(anyEscolar);
        }

        // PUT: api/AnyEscolars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize (Roles = "AG_Admin")]
        [HttpPut("editar")]
        public async Task<IActionResult> PutAnyEscolar(int id_anyEscolar, AnyEscolar anyEscolar)
        {
            if (id_anyEscolar != anyEscolar.id_anyEscolar)
            {
                return BadRequest();
            }

            _context.Entry(anyEscolar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnyEscolarExists(id_anyEscolar))
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

        // POST: api/AnyEscolars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("crear")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<ActionResult<AnyEscolar>> crear(AnyEscolar anyEscolar)
        {
            await _context.AnyEscolar.AddAsync(anyEscolar);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetAnyEscolar), new { id = anyEscolar.id_anyEscolar }, anyEscolar);
            return Ok();
        }

        // DELETE: api/AnyEscolars/5
        [HttpDelete("eliminar")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> DeleteAnyEscolar(int id)
        {
            var anyEscolar = await _context.AnyEscolar.FindAsync(id);
            if (anyEscolar == null)
            {
                return NotFound();
            }

            _context.AnyEscolar.Remove(anyEscolar);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool AnyEscolarExists(int id_AnyEscolar)
        {
            return _context.AnyEscolar.Any(e => e.id_anyEscolar == id_AnyEscolar);
        }
    }
}
