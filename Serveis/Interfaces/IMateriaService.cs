using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using System.Security.Claims;

namespace KarmaWebAPI.Serveis.Interfaces
{

    public interface IMateriaService
    {
        // =========================
        // CONSULTES
        // =========================
        Task<Materia?> InstanciaAsync(long idMateria, ClaimsPrincipal user);
        Task<List<Materia>> LlistaAsync(ClaimsPrincipal user);

        // =========================
        // SERVEIS
        // =========================
        Task<Materia> CrearAsync(MateriaCrearDTO dto);
        Task<Materia> EditarAsync(MateriaEditarDTO dto);

        Task<Materia> ActivarAsync(long idMateria);
        Task<Materia> DesactivarAsync(long idMateria);

        Task<bool> EliminarAsync(long idMateria);
    }

}
