using KarmaWebAPI.DTOs;
using KarmaWebAPI.DTOs.DisplaySets;

namespace KarmaWebAPI.Serveis.Interfaces
{

    public interface IProfessorDeClasseService
    {
        Task<ProfessorDeClasseDisplaySet> AssignarProfessorAClasseAsync(ProfessorDeClasseCrearDTO dto);
        Task<bool> EsborrarAsync(long idProfessorDeClasse);
        Task<List<ProfessorDeClasseDisplaySet>> GetLlistaAsync(int idAnyEscolar);

        Task<ProfessorDeClasseDisplaySet?> InstanciaAsync(long idProfessorDeClasse);

        Task<bool> ImparteixClasseAsync(string idProfessor, long idClasse);
        Task<bool> ImparteixMateriaEnClasseAsync(string idProfessor, long idClasse, long idMateria);
    }

}
