using System.Drawing;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
//using KarmaWebAPI.Migrations;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/privilegiperiode")]
    public class VPrivilegiPeriodeController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IPrivilegiService _privilegiService;
        private readonly IPeriodeService _periodeService;
        private readonly IAlumneEnGrupService _alumneEnGrupService;
        public VPrivilegiPeriodeController(DatabaseContext context)
        {
            _context = context;
        }

        #region Consultes

        //Instancia
        [HttpGet("instancia")]
        [Authorize]
        // GET: Privilegi/GetAnyEscolar/5
        public async Task<IActionResult> Instancia(int idAlumneEnGrup, int idPeriode, int idPrivilegi)
        {
            var vPrivilegiPeriode = await _context.VPrivilegiPeriode
                .Include(p => p.AlumneEnGrup)
                .Include(p => p.Periode)
                .Include(p => p.Privilegi)
                .FirstOrDefaultAsync(m => m.IdPrivilegi == idPrivilegi && 
                                     m.IdPeriode == idPeriode && 
                                     m.IdAlumneEnGrup == idAlumneEnGrup);
            if (vPrivilegiPeriode == null)
            {
                return NotFound();
            }
            return Ok(vPrivilegiPeriode);
        }

        //Llistes
        // GET: api/llista
        [HttpGet]
        [Route("llista")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<VPrivilegiPeriode>>> Llista()
        {
            var privilegis = await _context.VPrivilegiPeriode
                                            .Include(p => p.AlumneEnGrup)
                                            .Include(p => p.Periode)
                                            .Include(p => p.Privilegi)
                                            .ToListAsync();

            // Evitar el ciclo de referencias
            foreach (var privilegi in privilegis)
            {
                privilegi.AlumneEnGrup.PrivilegisPeriode = null;
                privilegi.Periode.PrivilegisPeriode = null;
                privilegi.Privilegi.PrivilegisPeriode = null;
            }

            return Ok(privilegis);
        }



        [Authorize]
        [HttpGet("llista-per-privilegi")]
        public IActionResult GetPrivilegisPeriodePerPrivilegi(int idPrivilegi)
        {
            var result = _privilegiService.GetPrivilegisPeriode(idPrivilegi);
            if (result == null)
            {
                return NotFound($"No s'ha trobat privilegis-periode per al privilegi amb ID {idPrivilegi}.");
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("llista-per-periode")]
        public IActionResult GetPrivilegisPeriodePerPeriode(int idPeriode)
        {
            var result = _periodeService.GetPrivilegisPeriode(idPeriode);
            if (result == null)
            {
                return NotFound($"No s'han trobat privilegis-periode para al periode amb ID {idPeriode}.");
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [HttpGet("llista-per-alumneengrup")]
        public IActionResult GetPrivilegisPeriodePerAlumneEnGrup(int idAlumneEnGrup)
        {
            var result = _alumneEnGrupService.GetPrivilegisPeriode(idAlumneEnGrup);
            if (result == null)
            {
                return NotFound($"No s'han trobat privilegis-periode para a l'alumne en grup amb ID {idAlumneEnGrup}.");
            }

            return Ok(result);
        }
        #endregion Consultes

    }
}
