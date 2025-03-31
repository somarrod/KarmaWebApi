using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Controllers
{
    [Route("api/AnyEscolar")]
    [ApiController]
    public class AnyEscolarController : ControllerBase
    {
        private readonly AnyEscolarContext _context;

        public AnyEscolarController(AnyEscolarContext context)
        {
            _context = context;
        }

        // GET: api/AnyEscolars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnyEscolar>>> GetAnyEscolar()
        {
            return await _context.AnyEscolar.ToListAsync();
        }

        // GET: api/AnyEscolars/2025
        [HttpGet("{id_anyEscolar}")]
        public async Task<ActionResult<AnyEscolar>> GetAnyEscolar(int id)
        {
            var anyEscolar = await _context.AnyEscolar.FindAsync(id);

            if (anyEscolar == null)
            {
                return NotFound();
            }

            return anyEscolar;
        }

        // PUT: api/AnyEscolars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id_anyEscolar}")]
        public async Task<IActionResult> PutAnyEscolar(int id, AnyEscolar anyEscolar)
        {
            if (id != anyEscolar.id_anyEscolar)
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
                if (!AnyEscolarExists(id))
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

        // POST: api/AnyEscolars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("crear")]
        public async Task<ActionResult<AnyEscolar>> crear(AnyEscolar anyEscolar)
        {
            await _context.AnyEscolar.AddAsync(anyEscolar);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetAnyEscolar), new { id = anyEscolar.id_anyEscolar }, anyEscolar);
            return Ok();
        }

        // DELETE: api/AnyEscolars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnyEscolar(int id)
        {
            var anyEscolar = await _context.AnyEscolar.FindAsync(id);
            if (anyEscolar == null)
            {
                return NotFound();
            }

            _context.AnyEscolar.Remove(anyEscolar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnyEscolarExists(int id)
        {
            return _context.AnyEscolar.Any(e => e.id_anyEscolar == id);
        }
    }
}
