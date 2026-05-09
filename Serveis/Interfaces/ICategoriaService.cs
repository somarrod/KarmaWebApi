using KarmaWebAPI.DTOs;
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

        Task<Categoria> ActivarAsync(long idCategoria);
        Task<Categoria> DesactivarAsync(long idCategoria);

        Task<bool> EliminarAsync(long idCategoria);
    }
}
