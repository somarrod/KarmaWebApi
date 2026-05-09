using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/tipus-categoria")]
    [Authorize(Roles = "AG_Admin")]
    public class TipusCategoriaController : ControllerBase
    {
        private readonly ITipusCategoriaService _service;

        public TipusCategoriaController(ITipusCategoriaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Llista()
            => Ok(await _service.LlistaAsync());

        [HttpPost]
        public async Task<IActionResult> Crear(string descripcio)
            => Ok(await _service.CrearAsync(descripcio));

        [HttpPut]
        public async Task<IActionResult> Editar(long idTipusCategoria, string descripcio, bool actiu)
            => Ok(await _service.EditarAsync(idTipusCategoria, descripcio, actiu));

        [HttpDelete("{idTipusCategoria}")]
        public async Task<IActionResult> Eliminar(long idTipusCategoria)
            => await _service.EliminarAsync(idTipusCategoria) ? Ok() : NotFound();
    }
}
