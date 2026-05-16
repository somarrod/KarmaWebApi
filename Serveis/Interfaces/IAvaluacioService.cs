using KarmaWebAPI.Models;
using KarmaWebAPI.DTOs.Avaluacio;
using KarmaWebAPI.DTOs.DisplaySets;

public interface IAvaluacioService
{
    Task<List<AvaluacioDisplaySet>> GetLlistaAsync(bool isAdmin);
    Task<List<AvaluacioDisplaySet>> GetLlistaPerAnyEscolarAsync(int idAnyEscolar, bool isAdmin);
    Task<AvaluacioDisplaySet?> GetByIdAsync(long idAvaluacio);

    Task<AvaluacioDisplaySet> CrearAsync(AvaluacioCrearDTO dto);
    Task<AvaluacioDisplaySet?> EditarAsync(AvaluacioEditarDTO dto);

    Task<AvaluacioDisplaySet?> IniciarAsync(long idAvaluacio);
    Task<AvaluacioDisplaySet?> FinalitzarAsync(long idAvaluacio);

    Task<bool> EsborrarAsync(long idAvaluacio);
}
