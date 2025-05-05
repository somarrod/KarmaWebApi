using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs.DisplaySets;
using KarmaWebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace KarmaWebAPI.Controllers
{
    [Route("api/periode")]
    [ApiController]
    public class PeriodeController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public PeriodeController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Periode
        [HttpGet("llista")]
        [Authorize(Roles ="AG_Professor,AG_Alumne,AG_Admin")]
        public async Task<ActionResult<IEnumerable<Periode>>> Llista()
        {

            var PeriodeList = new List<Periode>();
            var userName = User.Identity.Name;

            if (User.IsInRole("AG_Admin"))
            {
                // Retornar tots els alumnes en del grup
                PeriodeList = await _context.Periode
                            .ToListAsync();
            }
            else
            {
               
                    PeriodeList = await _context.Periode
                            .Include(c => c.AnyEscolar)
                             .Where(c => c.AnyEscolar.Actiu == true)
                            .ToListAsync();
            }

            return Ok(PeriodeList);
            
        }

        // GET: api/Periode
        [HttpGet("llista-per-anyescolar")]
        [Authorize(Roles = "AG_Professor,AG_Alumne,AG_Admin")]
        public async Task<ActionResult<IEnumerable<Periode>>> LlistaPerAnyEscolar(int idAnyEscolar)
        {

            var PeriodeList = new List<Periode>();
            var userName = User.Identity.Name;

            if (User.IsInRole("AG_Admin"))
            {
                // Retornar tots els alumnes en del grup
                PeriodeList = await _context.Periode
                            .Where(c => c.IdAnyEscolar == idAnyEscolar)
                            .ToListAsync();
            }
            else
            {
                PeriodeList = await _context.Periode
                        .Include(c => c.AnyEscolar)
                         .Where(c => c.AnyEscolar.Actiu == true && c.IdAnyEscolar == idAnyEscolar)
                        .ToListAsync();
            }

            return Ok(PeriodeList);


        }

        // GET: api/Periode/5
        [HttpGet("{idPeriode}")]
        public async Task<ActionResult<Periode>> Instancia(int idPeriode)
        {
            var periode = await _context.Periode.FindAsync(idPeriode);

            if (periode == null)
            {
                return NotFound();
            }

            return periode;
        }

        // TRANSACCIÓN INTERNA - NO DISPONIBLE PARA INVOCAR DESDE INTERFAZ
        //// POST: api/Periode/tcrear
        //[HttpPost("crear")]
        //public async Task<ActionResult<Periode>> TCrear(PeriodeTCREARDTO periodeDto)
        //{
        //    var anyEscolar = await _context.AnyEscolar.FindAsync(periodeDto.IdAnyEscolar);
        //    if (anyEscolar == null)
        //    {
        //        return NotFound($"Any escolar amb Id {periodeDto.IdAnyEscolar} no trobat");
        //    }

        //    DateOnly dataFi;
        //    if (anyEscolar.DataFiCurs > periodeDto.DataInici.AddDays(anyEscolar.DiesPeriode))
        //    {
        //        dataFi = periodeDto.DataInici.AddDays(anyEscolar.DiesPeriode);
        //    }
        //    else
        //    {
        //        dataFi = anyEscolar.DataFiCurs;
        //    }

        //    var periode = new Periode
        //    {
        //        IdAnyEscolar = periodeDto.IdAnyEscolar,
        //        DataInici = periodeDto.DataInici,
        //        DataFi = dataFi
        //    };

        //    _context.Periode.Add(periode);
        //    await _context.SaveChangesAsync();

        //    if (periode.DataFi < anyEscolar.DataFiCurs)
        //    {
        //        var nextPeriodeDto = new PeriodeTCREARDTO
        //        {
        //            IdAnyEscolar = periode.IdAnyEscolar,
        //            DataInici = periode.DataFi.AddDays(1)
        //        };
        //        await TCrear(nextPeriodeDto);
        //    }

        //    return Ok(periode);
        //}
   

        // PUT: api/Periode/editar
        //[HttpPut("editar")]
        //public async Task<IActionResult> Editar(int idPeriode, Periode periode)
        //{
        //    if (idPeriode != periode.IdPeriode)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(periode).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PeriodeExisteix(idPeriode))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // DELETE: api/Periode/eliminar
        [HttpDelete("eliminar")]
        public async Task<IActionResult> Eliminar(int idPeriode)
        {
            var periode = await _context.Periode.FindAsync(idPeriode);
            if (periode == null)
            {
                return NotFound();
            }

            _context.Periode.Remove(periode);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool PeriodeExisteix(int id)
        {
            return _context.Periode.Any(e => e.IdPeriode == id);
        }
    }
}
