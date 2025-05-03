using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IPeriodeService
    {
        ICollection<VPrivilegiPeriode> GetPrivilegisPeriode(int idPeriode);
    }
}
