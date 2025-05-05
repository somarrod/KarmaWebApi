namespace KarmaWebAPI.Serveis
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using KarmaWebAPI.Data;
    using KarmaWebAPI.DTOs;
    using KarmaWebAPI.Models;
    using KarmaWebAPI.Serveis.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class PuntuacioService
    {
        private readonly DatabaseContext _context;
        private readonly IAlumneEnGrupService _alumneEnGrupService;

        public PuntuacioService(DatabaseContext context, IAlumneEnGrupService alumneEnGrupService)
        {
            _context = context;
            _alumneEnGrupService = alumneEnGrupService;
        }


        public async Task<Puntuacio> CrearPuntuacioAsync(PuntuacioCrearDTO puntuacioDto)
        {
            var puntuacio = new Puntuacio
            {
                Motiu = puntuacioDto.Motiu,
                Punts = puntuacioDto.Punts,
                IdCategoria = puntuacioDto.IdCategoria,
                IdAlumneEnGrup = puntuacioDto.IdAlumneEnGrup,
                DataEntrada = DateOnly.FromDateTime(DateTime.Now),
            };

            _context.Puntuacio.Add(puntuacio);
            await _context.SaveChangesAsync();

            return puntuacio;
        }


        public async Task TCREARAsync(PuntuacioCrearDTO puntuacioDto)
        {
            // Crear instancia de puntuación
            var puntuacio = new Puntuacio
            {
                Motiu = puntuacioDto.Motiu,
                Punts = puntuacioDto.Punts,
                IdCategoria = puntuacioDto.IdCategoria,
                IdAlumneEnGrup = puntuacioDto.IdAlumneEnGrup,
                DataEntrada = DateOnly.FromDateTime(DateTime.Now),
            };

            await _context.Puntuacio.AddAsync(puntuacio);
            await _context.SaveChangesAsync();

            // Editar puntuación del alumno en grupo
            var alumneEnGrup = await _context.AlumneEnGrup.FindAsync(puntuacioDto.IdAlumneEnGrup);

            if (alumneEnGrup != null)
            {
                await _alumneEnGrupService.EditPuntuacioAsync(puntuacioDto.IdAlumneEnGrup, alumneEnGrup.PuntuacioTotal + puntuacioDto.Punts);

                // Calcular karma base del grupo PENDENT ACÍ
                //alumneEnGrup.Grup.calculaKarmaBase();
                //await _context.SaveChangesAsync();
            }

        }
    }

}