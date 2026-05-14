using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.DisplaySets;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IAnyEscolarService
    {
        public Task<AnyEscolarDisplaySet> CrearAnyEscolarAsync(AnyEscolarCrearDTO anyEscolarDto);
        public Task<AnyEscolarDisplaySet> EditarAnyEscolarAsync(AnyEscolarEditarDTO anyEscolarDto);

        Task<bool> EliminarAnyEscolarAsync(int  idAnyEscolar);

        Task<bool> ExistsAsync(int idAnyEscolar);
        
        Task<List<AnyEscolarDisplaySet>> GetLlistaAsync();

        Task CopiarConfiguracioAsync(int idAnyOrigen, int idAnyDesti);

    }

}
