using KarmaWebAPI.DTOs.DisplaySets;

namespace KarmaWebAPI.Serveis.Interfaces
{

    public interface IProfessorDeClasseService
    {
        Task<bool> ImparteixClasseAsync(string idProfessor, string idClasse);

        Task<bool> ImparteixMateriaEnClasseAsync(string idProfessor, string idClasse,long idMateria);

        Task<ProfessorDeClasse> AssignarAsync(string idProfessor, string idClasse, long idMateria);
        Task<bool> EsborrarAsync(string idProfessor, string idClasse, long idMateria);
        Task<List<ProfessorDeClasseDisplaySet>> GetLlistaAsync();

        Task<ProfessorDeClasseDisplaySet?> InstanciaAsync(long idProfessorDeClasse);
    }

}
