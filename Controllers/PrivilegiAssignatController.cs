using Microsoft.AspNetCore.Mvc;
using KarmaWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace KarmaWebAPI.Controllers
{
    [Route("api/privilegiassignat")]
    [ApiController]
    public class PrivilegiAssignatController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public PrivilegiAssignatController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/PrivilegiAssignat/5
        [HttpGet("{idPrivilegiAssignat}")]
        [Authorize(Roles = "AG_Professor,AG_Admin,AG_Alumne")]
        public async Task<ActionResult<PrivilegiAssignat>> Instancia(int idPrivilegiAssignat)
        {
            var privilegiAssignat = await _context.PrivilegiAssignat.FindAsync(idPrivilegiAssignat);

            if (privilegiAssignat == null)
            {
                return NotFound();
            }

            return privilegiAssignat;
        }

        // GET: api/PrivilegiAssignat
        [HttpGet("llista")]
        [Authorize(Roles = "AG_Professor,AG_Admin,AG_Alumne")]
        public async Task<ActionResult<IEnumerable<PrivilegiAssignat>>> Llista()
        {
            return await _context.PrivilegiAssignat.ToListAsync();
        }


        // POST: api/PrivilegiAssignat/crear
        [HttpPost("crear")]
        [Authorize(Roles = "AG_Professor,AG_Admin")]
        public async Task<ActionResult<PrivilegiAssignat>> Crear(PrivilegiAssignatCrearDto privilegiAssignatDto)
        {
            // Comprovar les dades proporcionades
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Comprovar que el any escolar, grup, NIA i privilegi existeixen
            var anyEscolarExisteix = await _context.AnyEscolar.AnyAsync(a => a.IdAnyEscolar == privilegiAssignatDto.IdAnyEscolar);
            var grupExisteix = await _context.Grup.AnyAsync(g => g.IdGrup == privilegiAssignatDto.IdGrup);
            var alumneExisteix = await _context.Alumne.AnyAsync(a => a.NIA == privilegiAssignatDto.NIA);
            var privilegi = await _context.Privilegi.FirstOrDefaultAsync(p => p.IdPrivilegi == privilegiAssignatDto.IdPrivilegi);

            if (!anyEscolarExisteix || !grupExisteix || !alumneExisteix || privilegi==null)
            {
                return NotFound("Alguna de les entitats proporcionades no existeix.");
            }

            // Comprovar que existeix un alumneEnGrup per al grup, any escolar i alumne
            var alumneEnGrup = await _context.AlumneEnGrup.FirstOrDefaultAsync(ag => ag.IdGrup == privilegiAssignatDto.IdGrup &&
                                                                               ag.IdAnyEscolar == privilegiAssignatDto.IdAnyEscolar &&
                                                                               ag.NIA == privilegiAssignatDto.NIA);

            if (alumneEnGrup==null)
            {
                return NotFound("No existeix cap alumne amb el nia proporcionat en el grup i any escolar indicats.");
            }

            if (privilegi.EsIndividualGrup == "I")
            {
                var privilegiAssignat = new PrivilegiAssignat
                {
                    IdPrivilegi = privilegiAssignatDto.IdPrivilegi,
                    IdAlumneEnGrup = alumneEnGrup.IdAlumneEnGrup,
                    Nivell = privilegi.Nivell,
                    Descripcio = privilegi.Descripcio,
                    EsIndividualGrup = privilegi.EsIndividualGrup,
                    DataAssignacio = DateOnly.FromDateTime(DateTime.Now)
                };

                // Afegir i guardar l'entitat
                _context.PrivilegiAssignat.Add(privilegiAssignat);
            }
            else { 

                var alumnesDelGrup = await _context.AlumneEnGrup.Where(ag => ag.IdGrup == privilegiAssignatDto.IdGrup &&
                                                                       ag.IdAnyEscolar == privilegiAssignatDto.IdAnyEscolar)
                                                                .ToListAsync();
                foreach (var alumnedelgrup in alumnesDelGrup)
                {
                    // Mapear DTO a entitat
                    var privilegiAssignat = new PrivilegiAssignat
                    {
                        IdPrivilegi = privilegiAssignatDto.IdPrivilegi,
                        IdAlumneEnGrup = alumnedelgrup.IdAlumneEnGrup,
                        Nivell = privilegi.Nivell,
                        Descripcio = privilegi.Descripcio,
                        EsIndividualGrup = privilegi.EsIndividualGrup,
                        DataAssignacio = DateOnly.FromDateTime(DateTime.Now)
                    };

                    // Afegir i guardar l'entitat
                    _context.PrivilegiAssignat.Add(privilegiAssignat);
                }
            }

            await _context.SaveChangesAsync();

            return Ok();
        }


        // PUT: api/PrivilegiAssignat/marcar-realitzat
        [HttpPut("marcar-realitzat")]
        [Authorize(Roles = "AG_Professor,AG_Admin")]
        public async Task<IActionResult>MARCARREALITZAT(PrivilegiAssignatEditarDto privilegiAssignatDTO)
        {
            var privilegiAssignat = await _context.PrivilegiAssignat
                                     .Include(p => p.AlumneEnGrup)
                                     //.ThenInclude(aeg => aeg.Grup)
                                     .FirstOrDefaultAsync(p => p.IdPrivilegiAssignat == privilegiAssignatDTO.IdPrivilegiAssignat);


            if (privilegiAssignat == null)
            {
                return NotFound();
            }


            if (privilegiAssignat.DataExecucio!=null)
            {
                return new ConflictObjectResult($"El privilegi assignat seleccionat ja ha estat gaudit per l'alumne");
            }

            if (privilegiAssignat.DataAssignacio > privilegiAssignatDTO.DataExecucio) {
                return new ConflictObjectResult($"La data d'execució no pot ser superior a la data d'assignació: {privilegiAssignat.DataAssignacio}");
            }

            if (privilegiAssignat.EsIndividualGrup == "I")
            {
                privilegiAssignat.DataExecucio = privilegiAssignatDTO.DataExecucio;
                _context.Entry(privilegiAssignat).State = EntityState.Modified;
            }
            else 
            {
                //   FOR ALL AlumneEnGrup.Grup.AlumnesEnGrup.PrivilegisAssignats
                //   WHERE (AlumneEnGrup.Grup.AlumnesEnGrup.PrivilegisAssignats.Privilegi = Privilegi AND
                //          AlumneEnGrup.Grup.AlumnesEnGrup.PrivilegisAssignats.DataCreacio = DataCreacio) DO
                //          AlumneEnGrup.Grup.AlumnesEnGrup.PrivilegisAssignats.edit_instance(AlumneEnGrup.Grup.AlumnesEnGrup.PrivilegisAssignats, pDataExecucio)

                var privilegisDeGrup = _context.PrivilegiAssignat
                    .Where(p => p.IdPrivilegi == privilegiAssignat.IdPrivilegi &&
                                p.DataAssignacio == privilegiAssignat.DataAssignacio &&
                                p.AlumneEnGrup.IdGrup == privilegiAssignat.AlumneEnGrup.IdGrup &&
                                p.AlumneEnGrup.IdAnyEscolar == privilegiAssignat.AlumneEnGrup.IdAnyEscolar)
                    .ToList();


                privilegisDeGrup.ForEach(p =>
                {
                    p.DataExecucio = privilegiAssignatDTO.DataExecucio;
                    _context.Entry(p).State = EntityState.Modified;
                });
            }
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrivilegiAssignatExisteix(privilegiAssignat.IdPrivilegiAssignat))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/PrivilegiAssignat/eliminar
        [HttpDelete("eliminar")]
        [Authorize(Roles = "AG_Professor,AG_Admin")]
        public async Task<IActionResult> Eliminar(int idPrivilegiAssignat)
        {
            var privilegiAssignat = await _context.PrivilegiAssignat.FindAsync(idPrivilegiAssignat);
            if (privilegiAssignat == null)
            {
                return NotFound();
            }

            _context.PrivilegiAssignat.Remove(privilegiAssignat);
            await _context.SaveChangesAsync();

            return Ok($"El privilegi assignat amb Id {idPrivilegiAssignat} ha estat esborrat.");
        }

        private bool PrivilegiAssignatExisteix(int id)
        {
            return _context.PrivilegiAssignat.Any(e => e.IdPrivilegiAssignat == id);
        }
    }
}
