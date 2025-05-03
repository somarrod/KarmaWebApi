//using KarmaWebAPI.Controllers;
using KarmaWebAPI.Data;
using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using KarmaWebAPI.Serveis.Interfaces;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Serveis
{

    public class AlumneService : IAlumneService
    {
        private readonly DatabaseContext _context;

      //  private readonly AuthController _authController;
      //  private readonly UserManager<ApiUser> _userManager;


        public AlumneService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<Alumne>> CrearAlumneAsync(AlumneDTO alumneDto)
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

            return new OkObjectResult(alumne);
        }

        public async Task<ActionResult<Alumne>> ActivarAlumneAsync(String nia)
        {
            var alumne = await _context.Alumne.FindAsync(nia);

            if (alumne == null)
            {
                return new NotFoundResult();
            }

            alumne.Actiu = true;

            _context.Entry(alumne).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return new OkResult();
        }


        public async Task<ActionResult<Alumne>> DesactivarAlumneAsync(String nia)
        {
            var alumne = await _context.Alumne.FindAsync(nia);

            if (alumne == null)
            {
                return new NotFoundResult();
            }

            alumne.Actiu = false;

            _context.Entry(alumne).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return new OkResult();
        }


    }

}
