using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IAlumneEnGrupService
    {
        public ICollection<VPrivilegiPeriode> GetPrivilegisPeriode(int idAlumneEnGrup);
        public Task<ActionResult<AlumneEnGrup>> AfegirPuntuacioAsync(int idAlumneEnGrup, int punts);
        public Task<ActionResult<AlumneEnGrup>> ResetPuntuacioTotalAsync(int idAlumneEnGrup, int puntuacioTotal);
    }

}
