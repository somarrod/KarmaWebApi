using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.DisplaySets;
using KarmaWebAPI.Models;
using System.Security.Claims;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface ICategoriaService
    {
        Task<CategoriaDisplaySet?> InstanciaAsync(long idCategoria, ClaimsPrincipal user);
        Task<List<CategoriaDisplaySet>> LlistaAsync(ClaimsPrincipal user);

        Task<Categoria> CrearAsync(CategoriaCrearDTO dto);
        Task<Categoria> EditarAsync(CategoriaEditarDTO dto);

        Task<Categoria> ActivarAsync(long idCategoria);
        Task<Categoria> DesactivarAsync(long idCategoria);

        Task<bool> EliminarAsync(long idCategoria);
    }
}
