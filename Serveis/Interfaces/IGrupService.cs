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
        

        //Si l'avaluació és null, agafa l'activa, sino la que li passe
        Task<string?> CalcularKarmaBaseAsync(long idGrup, long? idAvaluacio);
        Task<string?> CalcularKarmaBaseCoreAsync(long idGrup,long? idAvaluacio = null, bool saveChanges = true);

        //consultes
        Task<GrupDisplaySet?> InstanciaAsync(long idGrup, string rolUsuari, string? niaUsuari);

        Task<List<GrupDisplaySet>> LlistaPerClasseAsync(long idClasse, string rolUsuari, string? niaUsuari);
    }

}
