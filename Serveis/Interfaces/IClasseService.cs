using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.DisplaySets;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IClasseService
    {
        Task<List<ClasseDisplaySet>> LlistaAsync(int idAnyEscolar);
        Task<ClasseDisplaySet> InstanciaAsync(long idClasse);

        Task<ClasseDisplaySet> CrearAsync(ClasseCrearDTO dto);
        Task<ClasseDisplaySet> EditarAsync(ClasseEditarDTO dto);

        Task EsborrarAsync(long idClasse);

        
        Task AssignarAlumnesAsync(long idClasse, List<string> NIAs);

        Task DesassignarAlumnesAsync(List<string> NIAs);
    }
}
