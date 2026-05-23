using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.DisplaySets;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IAlumneService
    {
        Task<AlumneDisplaySet> CrearAsync(AlumneDTO dto);
        Task<AlumneDisplaySet> EditarAsync(AlumneDTO dto);
        Task<AlumneDisplaySet> ActivarAsync(string nia);
        Task<AlumneDisplaySet> DesactivarAsync(string nia);
        Task<AlumneDisplaySet> AssignarClasseIGrupAsync(AlumneAssignarClasseIGrupDTO dto);


        //CONSULTES   
        Task<AlumneDisplaySet> InstanciaAsync(string nia, ClaimsPrincipal user);
        Task<AlumneDisplaySet> InstanciaCoreAsync(string nia);
        Task<List<AlumneDisplaySet>> LlistaAsync(ClaimsPrincipal user);
        Task<List<AlumneDisplaySet>> LlistaPerClasseAsync(long idClasse, ClaimsPrincipal user);

        //Actualitza en BD tots els identity dels alumnes (si no estaven creats)
        Task SincronitzarIdentityAsync();

    }

}
