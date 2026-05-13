namespace KarmaWebAPI.Serveis.Interfaces
{
    using System.Security.Claims;
    using KarmaWebAPI.DTOs;
    using KarmaWebAPI.Models;

    public interface ITipusCategoriaService
    {
        Task<TipusCategoria?> InstanciaAsync(long idTipusCategoria);
        Task<List<TipusCategoria>> LlistaAsync();
        Task<List<TipusCategoria>> LlistaActiusAsync();

        Task<TipusCategoria> CrearAsync(TipusCategoriaCrearDTO dto);
        Task<TipusCategoria> EditarAsync(TipusCategoriaEditarDTO dto);

        Task<TipusCategoria> ActivarAsync(long idTipusCategoria);
        Task<TipusCategoria> DesactivarAsync(long idTipusCategoria);

        Task<bool> EliminarAsync(long idTipusCategoria);

        Task<bool> ExisteixAsync(long idTipusCategoria);
    }
}
