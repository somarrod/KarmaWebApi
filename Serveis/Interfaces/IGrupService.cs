using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IGrupService
    {

        Task<Grup> CrearAsync(long idClasse, string nom);
        Task<bool> EsborrarAsync(long idGrup);
        Task<Alumne> AfegirAlumneAsync(long idGrup, string nia);
        Task<Alumne> LlevarAlumneAsync(string nia);

        Task<string?> RecalcularKarmaBaseAsync(long idGrup);


        //consultes
        Task<Grup?> InstanciaAsync(long idGrup);
        Task<List<Grup>> LlistaPerClasseAsync(long idClasse);


    }

}
