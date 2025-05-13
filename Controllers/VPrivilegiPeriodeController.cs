using System.Drawing;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.DisplaySets;

//using KarmaWebAPI.Migrations;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/vprivilegiperiode")]
    public class VPrivilegiPeriodeController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IPrivilegiService _privilegiService;
        private readonly IPeriodeService _periodeService;
        private readonly IAlumneEnGrupService _alumneEnGrupService;
        public VPrivilegiPeriodeController(DatabaseContext context, IPrivilegiService privilegiService, IPeriodeService periodeService, IAlumneEnGrupService alumneEnGrupService)
        {
            _context = context;
            _privilegiService = privilegiService;
            _periodeService = periodeService;
            _alumneEnGrupService = alumneEnGrupService;
        }

        #region Consultes

        //Instancia
        [HttpGet("instancia-per-camps")]
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
        [Authorize(Roles ="AG_Professor,AG_Alumne,AG_Admin")]
        public async Task<ActionResult<IEnumerable<VPrivilegiPeriode>>> Llista()
        {

            var userId = User.Identity.Name;

            var privilegis = new List<VPrivilegiPeriode>();

            if (User.IsInRole("AG_Admin"))
            {
                privilegis = await _context.VPrivilegiPeriode
                                    .Include(p => p.AlumneEnGrup)
                                    .Include(p => p.Periode)
                                    .Include(p => p.Privilegi)
                                    .ToListAsync();
            }
            else 
            {
                if (User.IsInRole("AG_Professor"))
                {
                    //EXIST( AlumneEnGrup.Grup.ProfessorsDeGrup ) WHERE (AlumneEnGrup.Grup.ProfessorsDeGrup.Professor = Agent.Professor) = true
                    // Filtrar privilegis basados en el rol de profesor
                    privilegis = await _context.VPrivilegiPeriode
                                 .Include(p => p.AlumneEnGrup)
                                 .Include(p => p.Periode)
                                 .Include(p => p.Privilegi)
                                 .Where(p => p.AlumneEnGrup.Grup.ProfessorsDeGrup.Any(pg => pg.Professor.IdProfessor == userId))
                                 .ToListAsync();
                }
                else 
                {
                    if (User.IsInRole("AG_Alumne")) 
                    {
                        privilegis = await _context.VPrivilegiPeriode
                                     .Include(p => p.AlumneEnGrup)
                                     .Include(p => p.Periode)
                                     .Include(p => p.Privilegi)
                                     .Where(p => p.AlumneEnGrup.NIA == userId)
                                     .ToListAsync();
                    }
                }
            }

             // Evitar el ciclo de referencias
            foreach (var privilegi in privilegis)
            {
                privilegi.AlumneEnGrup.PrivilegisPeriode = null;
                privilegi.Periode.PrivilegisPeriode = null;
                privilegi.Privilegi.PrivilegisPeriode = null;
            }

            return Ok(privilegis);
        }

       
        [HttpGet("llista-condicionada")]
        [Authorize(Roles = "AG_Professor,AG_Alumne,AG_Admin")]
        public async Task<ActionResult<IEnumerable<VPrivilegiPeriodeDisplaySet>>> Llista(int? idPrivilegi, int? idPeriode, string? idGrup, int? idAnyEscolar, string? nia)
        {
            var userId = User.Identity.Name;

            var query = _context.VPrivilegiPeriode
                                .Include(p => p.AlumneEnGrup)
                                    .ThenInclude(aeg => aeg.Grup)
                                .Include(p => p.AlumneEnGrup)
                                    .ThenInclude(aeg => aeg.Alumne)
                                .Include(p => p.Periode)
                                .Include(p => p.Privilegi)
                                .AsQueryable();

            if (idPrivilegi.HasValue)
            {
                query = query.Where(p => p.IdPrivilegi == idPrivilegi.Value);
            }

            if (idPeriode.HasValue)
            {
                query = query.Where(p => p.IdPeriode == idPeriode.Value);
            }

            if (!string.IsNullOrEmpty(idGrup))
            {
                query = query.Where(p => p.AlumneEnGrup.IdGrup == idGrup);
            }

            if (idAnyEscolar.HasValue)
            {
                query = query.Where(p => p.AlumneEnGrup.IdAnyEscolar == idAnyEscolar.Value);
            }

            if (!string.IsNullOrEmpty(nia))
            {
                query = query.Where(p => p.AlumneEnGrup.NIA == nia);
            }

            var privilegisPeriode = new List<VPrivilegiPeriode>();

            if (User.IsInRole("AG_Admin"))
            {
                privilegisPeriode = await query.ToListAsync();
            }
            else if (User.IsInRole("AG_Professor"))
            {
                privilegisPeriode = await query
                                 .Where(p => p.AlumneEnGrup.Grup.ProfessorsDeGrup.Any(pg => pg.Professor.IdProfessor == userId))
                                 .ToListAsync();
            }
            else if (User.IsInRole("AG_Alumne"))
            {
                privilegisPeriode = await query
                                 .Where(p => p.AlumneEnGrup.NIA == userId)
                                 .ToListAsync();
            }

            // Convertir els resultats a VPrivilegiPeriodeDisplaySet
            var result = privilegisPeriode.Select(p => new VPrivilegiPeriodeDisplaySet
            {
                IdAnyEscolar = p.AlumneEnGrup.IdAnyEscolar,
                IdGrup = p.AlumneEnGrup.IdGrup,
                DescripcioGrup = p.AlumneEnGrup.Grup.Descripcio,
                IdPeriode = p.Periode.IdPeriode,
                DataInici = p.Periode.DataInici,
                DataFi = p.Periode.DataFi,
                NIA = p.AlumneEnGrup.NIA,
                NomCompletAlumne = p.AlumneEnGrup.Alumne.Nom + " " + p.AlumneEnGrup.Alumne.Cognoms,
                IdPrivilegi = p.IdPrivilegi,
                DescripcioPrivilegi = p.Privilegi.Descripcio
            }).ToList();

            return Ok(result);
        }


        [Authorize(Roles = "AG_Professor,AG_Alumne,AG_Admin")]
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

        [Authorize(Roles = "AG_Professor,AG_Alumne,AG_Admin")]
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
