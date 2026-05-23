using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.DisplaySets;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IGrupService
    {

        Task<GrupDisplaySet> CrearAsync(GrupCrearDTO dto);

        Task<GrupDisplaySet> EditarAsync(GrupEditarDTO dto);
        Task<bool> EsborrarAsync(long idGrup);

        Task<GrupDisplaySet> AfegirAlumneAsync(AssignarAlumneAGrupDTO dto);
        Task<Alumne> LlevarAlumneAsync(string nia);

        Task<string?> CalcularKarmaBaseAsync(long idGrup);

        //consultes
        Task<GrupDisplaySet?> InstanciaAsync(long idGrup, string rolUsuari, string? niaUsuari);

        Task<List<GrupDisplaySet>> LlistaPerClasseAsync(long idClasse, string rolUsuari, string? niaUsuari);
    }

}
