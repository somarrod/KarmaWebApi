namespace KarmaWebAPI.Serveis
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using KarmaWebAPI.Data;
    using KarmaWebAPI.DTOs;
    using KarmaWebAPI.Models;
    using KarmaWebAPI.Serveis.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class PuntuacioService : IPuntuacioService
    {
        private readonly DatabaseContext _context;
        private readonly IAlumneEnGrupService _alumneEnGrupService;
        private readonly IGrupService _grupService;

        public PuntuacioService(DatabaseContext context, IAlumneEnGrupService alumneEnGrupService, IGrupService grupService)
        {
            _context = context;
            _alumneEnGrupService = alumneEnGrupService;
            _grupService = grupService;
        }

        public async Task<ActionResult<Puntuacio>> CrearPuntuacioAsync(PuntuacioCrearDTO puntuacioDto, String? usuariCreacio)
        {
            var puntuacio = new Puntuacio
            {
                Motiu = puntuacioDto.Motiu,
                Punts = puntuacioDto.Punts,
                IdCategoria = puntuacioDto.IdCategoria,
                IdPeriode = puntuacioDto.IdPeriode,
                IdAlumneEnGrup = puntuacioDto.IdAlumneEnGrup,
                DataEntrada = DateOnly.FromDateTime(DateTime.Now),
                UsuariCreacio = usuariCreacio
            };

            _context.Puntuacio.Add(puntuacio);
            await _context.SaveChangesAsync();

            return new OkObjectResult(puntuacio);
        }

        public async Task<ActionResult<Puntuacio>> TCREARAsync(PuntuacioCrearDTO puntuacioDto, String? usuariCreacio)
        {

            var puntuacio = await CrearPuntuacioAsync(puntuacioDto, usuariCreacio);

            if (puntuacio == null)
            {
                return new ObjectResult("No s'ha pogut crear la puntuació")
                {
                    StatusCode = 500
                };
            }

            var alumneEnGrup = await _context.AlumneEnGrup.FindAsync(puntuacioDto.IdAlumneEnGrup);

            if (alumneEnGrup != null)
            {
                int total = alumneEnGrup.PuntuacioTotal + puntuacioDto.Punts;
                int idAlumneEnGrup = puntuacioDto.IdAlumneEnGrup;
                // Specify the interface explicitly to resolve ambiguity
                var resultado = await _alumneEnGrupService.EditPuntuacioAsync(idAlumneEnGrup, total);

                await _grupService.calculaKarmaBaseAsync(alumneEnGrup.IdAnyEscolar, alumneEnGrup.IdGrup);
            }
            await _context.SaveChangesAsync();

            return puntuacio;
        }
    

        public async Task<ActionResult<String>> TELIMINARAsync(int idPuntuacio)
        {
            try { 
            var puntuacio = await _context.Puntuacio.FindAsync(idPuntuacio);

            if (puntuacio == null)
            {
                return new ObjectResult($"No existeix la puntuació amb id {idPuntuacio}")
                {
                    StatusCode = 500
                };
            }

            var alumneEnGrup = await _context.AlumneEnGrup.FindAsync(puntuacio.IdAlumneEnGrup);

            if (alumneEnGrup != null)
            {
                int total = alumneEnGrup.PuntuacioTotal - puntuacio.Punts;
                int idAlumneEnGrup = puntuacio.IdAlumneEnGrup;

                var resultado = await _alumneEnGrupService.EditPuntuacioAsync(idAlumneEnGrup, total);

                await _grupService.calculaKarmaBaseAsync(alumneEnGrup.IdAnyEscolar, alumneEnGrup.IdGrup);
            }

            _context.Puntuacio.Remove(puntuacio);

            await _context.SaveChangesAsync();

            return new OkObjectResult($"La puntuació amb Id {idPuntuacio} ha estat esborrada");
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Error: {ex.Message}")
                {
                    StatusCode = 500
                };
            }
        }
    }


}