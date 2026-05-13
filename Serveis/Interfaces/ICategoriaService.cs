using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.DisplaySets;
using KarmaWebAPI.Models;
using System.Security.Claims;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface ICategoriaService
    {
        Task<Categoria?> InstanciaAsync(long idCategoria, ClaimsPrincipal user);
        Task<List<Categoria>> LlistaAsync(ClaimsPrincipal user);

        Task<Categoria> CrearAsync(CategoriaCrearDTO dto);
        Task<Categoria> EditarAsync(CategoriaEditarDTO dto);

        Task<CategoriaDisplaySet> ActivarAsync(long idCategoria);
        Task<CategoriaDisplaySet> DesactivarAsync(long idCategoria);

        Task<bool> EliminarAsync(long idCategoria);
    }
}
