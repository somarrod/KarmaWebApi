using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KarmaWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PuntuacioController : ControllerBase
    {
        private readonly IPuntuacioService _puntuacioService;

        public PuntuacioController(IPuntuacioService puntuacioService)
        {
            _puntuacioService = puntuacioService;
        }

        // ==================================================
        // ASSIGNAR PUNTS (S)
        // ==================================================
        [HttpPost("assignar")]
        public async Task<IActionResult> AssignarPunts(
            [FromBody] PuntuacioCrearDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Puntuacio p = await _puntuacioService.AssignarPuntsAsync(
                dto.NIA,
                dto.IdAvaluacio,
                dto.IdCategoria,
                dto.NumPunts,
                dto.Motiu,
                dto.DescripcioAdicional,
                User);

            return Ok(p);
        }

        // ==================================================
        // REINICIAR PUNTS (I)
        // ==================================================
        [HttpPost("reiniciar")]
        public async Task<IActionResult> ReiniciarPunts(
            [FromBody] PuntuacioCrearDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Puntuacio p = await _puntuacioService.ReiniciarPuntsAsync(
                dto.NIA,
                dto.IdAvaluacio,
                dto.IdCategoria,
                dto.NumPunts,
                dto.Motiu,
                dto.DescripcioAdicional,
                User);

            return Ok(p);
        }

        // ==================================================
        // OBTINDRE UNA PUNTUACIÓ (INSTÀNCIA)
        // ==================================================
        [HttpGet("{idPuntuacio:long}")]
        public async Task<IActionResult> Instancia(long idPuntuacio)
        {
            Puntuacio? p = await _puntuacioService.InstanciaAsync(idPuntuacio);

            if (p == null)
                return NotFound();

            return Ok(p);
        }

        // ==================================================
        // LLISTA PER ALUMNE
        // ==================================================
        [HttpGet("alumne/{nia}")]
        public async Task<IActionResult> LlistaPerAlumne(
            string nia,
            [FromQuery] long? idAvaluacio)
        {
            var llista = await _puntuacioService
                .LlistaPerAlumneAsync(nia, idAvaluacio);

            return Ok(llista);
        }

        // ==================================================
        // LLISTA PER CLASSE
        // ==================================================
        [HttpGet("classe/{idClasse:long}")]
        public async Task<IActionResult> LlistaPerClasse(
            long idClasse,
            [FromQuery] long? idAvaluacio)
        {
            var llista = await _puntuacioService
                .LlistaPerClasseAsync(idClasse, idAvaluacio);

            return Ok(llista);
        }

        // ==================================================
        // LLISTA PER GRUP
        // ==================================================
        [HttpGet("grup/{idGrup:long}")]
        public async Task<IActionResult> LlistaPerGrup(
            long idGrup,
            [FromQuery] long? idAvaluacio)
        {
            var llista = await _puntuacioService
                .LlistaPerGrupAsync(idGrup, idAvaluacio);

            return Ok(llista);
        }
    }
}