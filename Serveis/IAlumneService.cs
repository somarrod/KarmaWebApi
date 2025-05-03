using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis
{
    public interface IAlumneService
    {
        public Task<ActionResult<Alumne>> CrearAlumneAsync(AlumneDTO alumneDto);

        public Task<ActionResult<Alumne>> ActivarAlumneAsync(String nia);

        public Task<ActionResult<Alumne>> DesactivarAlumneAsync(String nia);
    }

}
