using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IAnyEscolarService
    {
        public Task<AnyEscolar> CrearAnyEscolarAsync(AnyEscolarCrearDTO anyEscolarDto);
        public Task<AnyEscolar> EditarAnyEscolarAsync(AnyEscolarEditarDTO anyEscolarDto);

        Task<bool> EliminarAnyEscolarAsync(int  idAnyEscolar);

        Task<bool> ExistsAsync(int idAnyEscolar);
        
        Task<List<AnyEscolar>> GetLlistaAsync();


     
    }

}
