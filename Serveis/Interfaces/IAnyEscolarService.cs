using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IAnyEscolarService
    {
        public Task<AnyEscolar> CrearAnyEscolarAsync(AnyEscolarCrearDto anyEscolarDto);
        public Task<AnyEscolar> EditarAnyEscolarAsync(AnyEscolarEditarDto anyEscolarDto);

        Task<bool> ExistsAsync(int idAnyEscolar);
        
        Task<List<AnyEscolar>> GetLlistaAsync();


        //public Task<IActionResult> ActualitzaKarmaAsync(int anyEscolar);
    }

}
