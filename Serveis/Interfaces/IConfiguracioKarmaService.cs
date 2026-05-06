using KarmaWebAPI.DTOs;
//using KarmaWebAPI.DTOs.ConfiguracioKarmaDTO;
using KarmaWebAPI.Models;

public interface IConfiguracioKarmaService
{
    Task<List<ConfiguracioKarma>> GetPerAnyEscolarAsync(int idAnyEscolar);

    Task<ConfiguracioKarma> CrearAsync(ConfiguracioKarmaCrearDTO dto);
    Task<ConfiguracioKarma?> EditarAsync(ConfiguracioKarmaEditarDTO dto);
    Task<bool> EsborrarAsync(long idConfiguracioKarma);

    // validació forta (NO buits)
    Task ValidarConfiguracioCompletaAsync(int idAnyEscolar);
}