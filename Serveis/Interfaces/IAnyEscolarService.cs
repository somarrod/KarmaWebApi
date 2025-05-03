using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IAnyEscolarService
    {
        public Task<ActionResult<AnyEscolar>> CrearAnyEscolarAsync(AnyEscolarCrearDto anyEscolarDto);
        public Task<ActionResult<AnyEscolar>> TCREARAsync(AnyEscolarCrearDto anyEscolarDto);
    }

}
