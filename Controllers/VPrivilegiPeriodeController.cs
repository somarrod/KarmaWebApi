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
    [Route("api/vprivilegiperiodo")]
    public class VPrivilegiPeriodeController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IPrivilegiService _privilegiService;
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
            var vPrivilegiPeriode = await _context.VPrivilegisPeriode
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
            var privilegis = await _context.VPrivilegisPeriode
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
        #endregion Consultes

    }
}
