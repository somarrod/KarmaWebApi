using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IPrivilegiService
    {
        public Task<ActionResult<Privilegi>> CrearPrivilegiAsync(PrivilegiCrearDto privilegiDto);

        public ICollection<VPrivilegiPeriode> GetPrivilegisPeriode(int idPrivilegi);
    }

}
