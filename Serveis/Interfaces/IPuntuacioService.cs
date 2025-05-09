using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IPuntuacioService
    {
        public Task<ActionResult<Puntuacio>> CrearPuntuacioAsync(PuntuacioCrearDTO puntuacioDto, String? usuariCreacio);

        public Task<ActionResult<Puntuacio>> TCREARAsync(PuntuacioCrearDTO puntuacioDto, String? usuariCreacio);
    }
}
