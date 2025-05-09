using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IPuntuacioService
    {
        public Task<Puntuacio> CrearPuntuacioAsync(PuntuacioCrearDTO puntuacioDto, String? usuariCreacio);

        public Task<Puntuacio> TCREARAsync(PuntuacioCrearDTO puntuacioDto, String? usuariCreacio);
    }
}
