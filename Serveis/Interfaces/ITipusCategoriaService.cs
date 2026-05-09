namespace KarmaWebAPI.Serveis.Interfaces
{
    using System.Security.Claims;
    using KarmaWebAPI.Models;

    public interface ITipusCategoriaService
    {
        Task<TipusCategoria?> InstanciaAsync(long idTipusCategoria);
        Task<List<TipusCategoria>> LlistaAsync();

        Task<TipusCategoria> CrearAsync(string descripcio);
        Task<TipusCategoria> EditarAsync(long idTipusCategoria, string descripcio, bool actiu);

        Task<TipusCategoria> ActivarAsync(long idTipusCategoria);
        Task<TipusCategoria> DesactivarAsync(long idTipusCategoria);

        Task<bool> EliminarAsync(long idTipusCategoria);
    }
}
