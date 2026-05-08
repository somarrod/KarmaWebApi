using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IAlumneService
    {
        Task<Alumne> CrearAsync(AlumneDTO dto);
        Task<Alumne> EditarAsync(AlumneDTO dto);
        Task<Alumne> ActivarAsync(string nia);
        Task<Alumne> DesactivarAsync(string nia);
        Task<Alumne> InstanciaAsync(string nia, ClaimsPrincipal user);
        Task<List<Alumne>> LlistaAsync(ClaimsPrincipal user);
        Task<Alumne> AssignarClasseAsync(string nia, long idNovaClasse);
    }

}
