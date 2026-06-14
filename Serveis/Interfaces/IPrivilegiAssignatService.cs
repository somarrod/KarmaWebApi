using KarmaWebAPI.Models;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IPrivilegiAssignatService
    {

        Task<List<PrivilegiAssignat>> AssignarAsync(string nia, long idPrivilegi);
        Task<List<PrivilegiAssignat>> ExecutarAsync(long idPrivilegiAssignat);

        Task<List<PrivilegiAssignat>> LlistaPerAlumneAsync(string nia);

        Task<List<PrivilegiAssignat>> LlistaPerGrupAsync(long idGrup);
        Task<List<PrivilegiAssignat>> LlistaPerClasseAsync(long idClasse);


    }
}
