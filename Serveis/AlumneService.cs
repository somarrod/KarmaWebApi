using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis
{

    public class AlumneService : IAlumneService
    {
        private readonly DatabaseContext _context;

        public AlumneService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<AnyEscolar>> CrearAlumneAsync(AlumneDTO alumneDto)
        {
            var alumne = new Alumne
            {
                NIA = alumneDto.NIA,
                Nom = alumneDto.Nom,
                Cognoms = alumneDto.Cognoms,
                Actiu = true,
                Email = alumneDto.Email
            };

            _context.Alumne.Add(alumne);
            await _context.SaveChangesAsync();

            // Fix: Use the correct method `CreatedAtAction` instead of `CreatedAtActionResult`
            return new CreatedAtActionResult("crear", null, new { nia = alumne.NIA }, alumne);
            //return new OkObjectResult(alumne);
        }

     
    }

}
