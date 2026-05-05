using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IPeriodeService
    {
        Task<Periode> TCrearAsync(PeriodeTCREARDTO periodeDto);

    }
}
