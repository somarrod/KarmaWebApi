using System.Threading.Tasks;
using KarmaWebAPI.DTOs;


namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IConfiguracioKarmaService
    {
        bool ValidateKarmaRange(int idConfiguracioKarma, int karmaMinim, int karmaMaxim);
        Task CrearConfiguracioKarmaAsync(ConfiguracioKarmaCrearDTO dto);
        Task EditarConfiguracioKarmaAsync(ConfiguracioKarmaEditarDTO dto);
    }
}
