using KarmaWebAPI.Models;
using KarmaWebAPI.DTOs.Avaluacio;

public interface IAvaluacioService
{
    Task<List<Avaluacio>> GetLlistaAsync(bool isAdmin);
    Task<List<Avaluacio>> GetLlistaPerAnyEscolarAsync(int idAnyEscolar, bool isAdmin);
    Task<Avaluacio?> GetByIdAsync(long idAvaluacio);

    Task<Avaluacio> TCrearAsync(AvaluacioTCrearDTO dto);
    Task<Avaluacio?> TEditarAsync(AvaluacioTEditarDTO dto);

    Task<Avaluacio?> TIniciarAsync(long idAvaluacio);
    Task<Avaluacio?> TFinalitzarAsync(long idAvaluacio);

    Task<bool> EsborrarAsync(long idAvaluacio);
}
