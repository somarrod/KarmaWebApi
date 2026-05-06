using KarmaWebAPI.Models;
using KarmaWebAPI.DTOs.Avaluacio;

public interface IAvaluacioService
{
    Task<List<Avaluacio>> GetLlistaAsync(bool isAdmin);
    Task<List<Avaluacio>> GetLlistaPerAnyEscolarAsync(int idAnyEscolar, bool isAdmin);
    Task<Avaluacio?> GetByIdAsync(int idAvaluacio);

    Task<Avaluacio> TCrearAsync(AvaluacioTCrearDTO dto);
    Task<Avaluacio?> TEditarAsync(AvaluacioTEditarDTO dto);

    Task<Avaluacio?> TIniciarAsync(int idAvaluacio);
    Task<Avaluacio?> TFinalitzarAsync(int idAvaluacio);

    Task<bool> EsborrarAsync(int idAvaluacio);
}