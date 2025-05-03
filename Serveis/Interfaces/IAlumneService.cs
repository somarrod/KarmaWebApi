using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IAlumneService
    {
        public Task<ActionResult<Alumne>> CrearAlumneAsync(AlumneDTO alumneDto);

        public Task<ActionResult<Alumne>> ActivarAlumneAsync(string nia);

        public Task<ActionResult<Alumne>> DesactivarAlumneAsync(string nia);
    }

}
