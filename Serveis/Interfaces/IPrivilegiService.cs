using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IPrivilegiService
    {
        Task<Privilegi> CrearAsync(PrivilegiCrearDTO privilegi);
        Task<Privilegi> EditarAsync(PrivilegiEditarDTO privilegi);
        Task<bool> EliminarAsync(long idPrivilegi);

        Task<Privilegi?> InstanciaAsync(long idPrivilegi);
        Task<List<Privilegi>> LlistaPerAnyEscolarAsync(long idAnyEscolar);


    }

}
