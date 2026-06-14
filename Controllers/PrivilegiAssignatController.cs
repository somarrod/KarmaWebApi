using KarmaWebAPI.DTOs;
using KarmaWebAPI.Serveis;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/privilegi-assignat")]
    public class PrivilegiAssignatController : ControllerBase
    {
        private readonly IPrivilegiAssignatService _service;

        public PrivilegiAssignatController(IPrivilegiAssignatService service)
        {
            _service = service;
        }

        // ==================================================
        // POST: api/privilegi-assignat/assignar
        // Assigna privilegi (I o G segons Tipus)
        // ==================================================
        [HttpPost("assignar")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu")]
        public async Task<IActionResult> Assignar(AssignarPrivilegiDTO dto)
        {
            var result = await _service.AssignarAsync(dto.NIA, dto.IdPrivilegi);
            return Ok(result);
        }

        // ==================================================
        // POST: api/privilegi-assignat/executar
        // Executa privilegi per CodiIntern (afecta tots si és de grup)
        // ==================================================
        [HttpPost("gaudir/{idPrivilegiAssignat:long}")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu")]
        public async Task<IActionResult> Executar(long idPrivilegiAssignat)
        {
            var result = await _service.ExecutarAsync(idPrivilegiAssignat);
           
            return Ok(result);
        }

        // ==================================================
        // GET: api/privilegi-assignat/per-alumne/{nia}
        // ==================================================
        [HttpGet("per-alumne/{nia}")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu,AG_Alumne")]
        public async Task<IActionResult> LlistaPerAlumne(string nia)
        {
            // Un alumne només es pot veure a si mateix
            if (User.IsInRole("AG_Alumne") && User.Identity!.Name != nia)
                return Forbid();

            var llista = await _service.LlistaPerAlumneAsync(nia);
            return Ok(llista);
        }

        // ==================================================
        // GET: api/privilegi-assignat/per-grup/{idGrup}
        // ==================================================
        [HttpGet("per-grup/{idGrup:long}")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu")]
        public async Task<IActionResult> LlistaPerGrup(long idGrup)
        {
            var llista = await _service.LlistaPerGrupAsync(idGrup);
            return Ok(llista);
        }

        // ==================================================
        // GET: api/privilegi-assignat/per-classe/{idClasse}
        // ==================================================
        [HttpGet("per-classe/{idClasse:long}")]
        [Authorize(Roles = "AG_Admin,AG_Professor,AG_EquipDirectiu")]
        public async Task<IActionResult> LlistaPerClasse(long idClasse)
        {
            var llista = await _service.LlistaPerClasseAsync(idClasse);
            return Ok(llista);
        }
    }
}