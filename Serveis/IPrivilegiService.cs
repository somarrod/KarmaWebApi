using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis
{
    public interface IPrivilegiService
    {
        public Task<ActionResult<Privilegi>> CrearPrivilegiAsync(PrivilegiCrearDto privilegiDto);
    }

}
