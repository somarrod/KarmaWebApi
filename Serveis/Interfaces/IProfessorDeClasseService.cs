using KarmaWebAPI.DTOs.DisplaySets;

namespace KarmaWebAPI.Serveis.Interfaces
{

    public interface IProfessorDeClasseService
    {
        Task<bool> ImparteixClasseAsync(string idProfessor, long idClasse);

        Task<bool> ImparteixMateriaEnClasseAsync(string idProfessor, long idClasse,long idMateria);

        Task<ProfessorDeClasse> AssignarAsync(string idProfessor, long idClasse, long idMateria);
        Task<bool> EsborrarAsync(string idProfessor, long idClasse, long idMateria);
        Task<List<ProfessorDeClasseDisplaySet>> GetLlistaAsync();

        Task<ProfessorDeClasseDisplaySet?> InstanciaAsync(long idProfessorDeClasse);
    }

}
