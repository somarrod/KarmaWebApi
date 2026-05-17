using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IGrupService
    {

        Task<Grup> CrearAsync(GrupCrearDTO dto);

        Task<Grup> EditarAsync(GrupEditarDTO dto);
        Task<bool> EsborrarAsync(long idGrup);

        Task<Alumne> AfegirAlumneAsync(AssignarAlumneAGrupDTO dto);
        Task<Alumne> LlevarAlumneAsync(string nia);

        Task<string?> RecalcularKarmaBaseAsync(long idGrup);

        //consultes
        Task<Grup?> InstanciaAsync(long idGrup, string rolUsuari, string? niaUsuari);

        Task<List<Grup>> LlistaPerClasseAsync(long idClasse, string rolUsuari, string? niaUsuari);
    }

}
