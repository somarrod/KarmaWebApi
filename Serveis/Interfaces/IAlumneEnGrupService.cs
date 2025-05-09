using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IAlumneEnGrupService
    {
        public ICollection<VPrivilegiPeriode> GetPrivilegisPeriode(int idAlumneEnGrup);
        public Task<ActionResult<AlumneEnGrup>> EditPuntuacioAsync(int idAlumneEnGrup, int Punts);
    }

}
