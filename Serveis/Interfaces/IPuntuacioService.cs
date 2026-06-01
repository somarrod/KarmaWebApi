using KarmaWebAPI.DTOs.DisplaySets;
using KarmaWebAPI.Models;
using System.Security.Claims;

public interface IPuntuacioService
{
    // Crear / Assignar punts (S)
    Task<PuntuacioDisplaySet> AssignarPuntsAsync(
        string nia,
        long idAvaluacio,
        long idCategoria,
        double numPunts,
        string motiu,
        string? descripcioAdicional,
        DateOnly dataEvent,
        ClaimsPrincipal user);

    // Reiniciar punts (I)
    Task<PuntuacioDisplaySet> ReiniciarPuntsAsync(
        string nia,
        long idAvaluacio,
        long idCategoria,
        double nouValor,
        string motiu,
        string? descripcioAdicional,
        DateOnly dataEvent,
        ClaimsPrincipal user);


    // ==================================================
    // CONSULTES
    // ==================================================
    Task<PuntuacioDisplaySet?> InstanciaAsync(
        long idPuntuacio,
        ClaimsPrincipal user);

    Task<List<PuntuacioDisplaySet>> LlistaPerAlumneAsync(
        string nia,
        ClaimsPrincipal user,
        long? idAvaluacio = null);

    Task<List<PuntuacioDisplaySet>> LlistaPerClasseAsync(
        long idClasse,
        ClaimsPrincipal user,
        long? idAvaluacio = null);

    Task<List<PuntuacioDisplaySet>> LlistaPerGrupAsync(
        long idGrup,
        ClaimsPrincipal user,
        long? idAvaluacio = null);
}