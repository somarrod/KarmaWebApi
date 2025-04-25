using System.Drawing;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
//using KarmaWebAPI.Migrations;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/privilegi")]
    public class PrivilegiController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public PrivilegiController(DatabaseContext context)
        {
            _context = context;
        }


        #region Consultes

        //Instancia
        [HttpGet("{idPrivilegi}")]
        [Authorize]
        // GET: Privilegi/GetAnyEscolar/5
        public async Task<IActionResult> Instancia(int idPrivilegi)
        {
            var privilegi = await _context.Privilegi
                .Include(p => p.AnyEscolar)
                .FirstOrDefaultAsync(m => m.IdPrivilegi == idPrivilegi);
            if (privilegi == null)
            {
                return NotFound();
            }
            return Ok(privilegi);
        }

        //Llistes
        // GET: api/llista
        [HttpGet]
        [Route("llista")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Privilegi>>> Llista()
        {
            var privilegis = await _context.Privilegi
                                         .Include(p => p.AnyEscolar)
                                         .ToListAsync();

            // Evitar el ciclo de referencias
            foreach (var privilegi in privilegis)
            {
                privilegi.AnyEscolar.Privilegis = null;
            }

            return Ok(privilegis);
        }

        //Consulta de relacionades
        // GET: api/llista
        [HttpGet]
        [Route("llista-per-anyescolar")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Privilegi>>> LlistaPerAnyEscolar(int idAnyEscolar)
        {
            var anyEscolar = await _context.AnyEscolar.FindAsync(idAnyEscolar);
            if (anyEscolar == null)
            {
                return NotFound("Any escolar " + idAnyEscolar + " no trobat");
            }

            var privilegis = await _context.Privilegi
                                         .Include(p => p.AnyEscolar)
                                         .Where(p => p.IdAnyEscolar == idAnyEscolar)
                                         .ToListAsync();

            // Evitar el ciclo de referencias
            foreach (var privilegi in privilegis)
            {
                privilegi.AnyEscolar.Privilegis = null;
            }

            return Ok(privilegis);
        }
        #endregion Consultes

        #region Serveis
        // POST: api/Privilegi
        [HttpPost]
        [Route("crear")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<ActionResult<Privilegi>> Crear([FromBody] PrivilegiCrearDto privilegiDto)
        {
            var anyEscolar = await _context.AnyEscolar.FindAsync(privilegiDto.IdAnyEscolar);
            if (anyEscolar == null)
            {
                return NotFound("Any escolar " + privilegiDto.IdAnyEscolar + " no trobat");
            }

            // Verificar si ya existe un Privilegi con el mismo nombre
            var existePrivilegi = await _context.Privilegi
                    .AnyAsync(p => p.Descripcio == privilegiDto.Descripcio);

            if (existePrivilegi)
            {
                return Conflict("Ja existeix un privilegi amb la mateixa descripció.");
            }


            var privilegi = new Privilegi
            {
                Nivell = privilegiDto.Nivell,
                Descripcio = privilegiDto.Descripcio,
                EsIndividualGrup = privilegiDto.EsIndividualGrup,
                IdAnyEscolar = privilegiDto.IdAnyEscolar,
                AnyEscolar = anyEscolar // Asignar la instancia recuperada
            };

            await _context.Privilegi.AddAsync(privilegi);
            await _context.SaveChangesAsync();

            return Ok("Privilegi creat: " + privilegi.IdPrivilegi.ToString());
        }

       
        // PUT: api/Privilegi/5
        [Authorize(Roles = "AG_Admin")]
        [HttpPut("editar")]
        public async Task<IActionResult> Editar(PrivilegiEditarDto privilegiDto)
        {

            var anyEscolar = await _context.AnyEscolar.FindAsync(privilegiDto.IdAnyEscolar);
            if (anyEscolar == null)
            {
                return NotFound("Any escolar " + privilegiDto.IdAnyEscolar + " no trobat");
            }

            var privilegiAUX = await _context.Privilegi.FindAsync(privilegiDto.IdAnyEscolar);
            if (privilegiAUX == null)
            {
                return NotFound("Privilegi " + privilegiDto.IdPrivilegi + " no trobat");
            }

            Privilegi privilegi = new Privilegi
            {   
                IdPrivilegi = privilegiDto.IdPrivilegi,
                Nivell = privilegiDto.Nivell,
                Descripcio = privilegiDto.Descripcio,
                EsIndividualGrup = privilegiDto.EsIndividualGrup,
                IdAnyEscolar = privilegiDto.IdAnyEscolar,
                AnyEscolar = anyEscolar // Asignar la instancia recuperada
            };

            _context.Entry(privilegi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                 return StatusCode(500, e.InnerException != null ? e.InnerException.Message : e.Message);
            }

            return Ok();
        }


        // DELETE: api/Privilegi/5
        [HttpDelete("eliminar")]
        [Authorize(Roles = "AG_Admin")]
        public async Task<IActionResult> Eliminar(int idPrivilegi)
        {
            var privilegi = await _context.Privilegi.FindAsync(idPrivilegi);
            if (privilegi == null)
            {
                return NotFound("Privilegi " + idPrivilegi + " no trobat");
            }

            _context.Privilegi.Remove(privilegi);
            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion Serveis

        #region Auxiliars
        private bool PrivilegiExists(int idPrivilegi)
        {
            return _context.Privilegi.Any(e => e.IdPrivilegi == idPrivilegi);
        }
        #endregion Auxiliars
    }
}
