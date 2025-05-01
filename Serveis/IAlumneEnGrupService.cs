using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis
{
    public interface IAlumneEnGrupService
    {
        public ICollection<VPrivilegiPeriode> GetPrivilegisPeriode(int idAlumneEnGrup);
    }

}
