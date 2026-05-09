using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using System.Security.Claims;

namespace KarmaWebAPI.Serveis.Interfaces
{

    public interface IMateriaService
    {
        Task<Materia?> InstanciaAsync(int idMateria, ClaimsPrincipal user);
        Task<List<Materia>> LlistaAsync(ClaimsPrincipal user);

        Task<Materia> CrearAsync(MateriaCrearDTO dto);
        Task<Materia> EditarAsync(MateriaEditarDTO dto);

        Task<Materia> ActivarAsync(int idMateria);
        Task<Materia> DesactivarAsync(int idMateria);

        Task<bool> EliminarAsync(int idMateria);
    }

}
