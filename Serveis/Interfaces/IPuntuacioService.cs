using KarmaWebAPI.Models;
using System.Security.Claims;

public interface IPuntuacioService
{
    // Crear / Assignar punts (S)
    Task<Puntuacio> AssignarPuntsAsync(
        string nia,
        long idAvaluacio,
        long idCategoria,
        double numPunts,
        string motiu,
        string? descripcioAdicional,
        ClaimsPrincipal user);

    // Reiniciar punts (I)
    Task<Puntuacio> ReiniciarPuntsAsync(
        string nia,
        long idAvaluacio,
        long idCategoria,
        double nouValor,
        string motiu,
        string? descripcioAdicional,
        ClaimsPrincipal user);

    // Consultes
    Task<Puntuacio?> InstanciaAsync(long idPuntuacio);

    Task<List<Puntuacio>> LlistaPerAlumneAsync(string nia, long? idAvaluacio = null);

    Task<List<Puntuacio>> LlistaPerClasseAsync(long idClasse, long? idAvaluacio = null);

    Task<List<Puntuacio>> LlistaPerGrupAsync(long idGrup, long? idAvaluacio = null);
}