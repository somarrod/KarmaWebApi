using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis
{
    public interface IAlumneService
    {
        public Task<ActionResult<AnyEscolar>> CrearAlumneAsync(AlumneDTO alumneDto);
    }

}
