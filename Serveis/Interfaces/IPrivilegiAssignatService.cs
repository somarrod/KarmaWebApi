using KarmaWebAPI.Models;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IPrivilegiAssignatService
    {

        Task<List<PrivilegiAssignat>> AssignarAsync(string nia, long idPrivilegi);
        Task<bool> ExecutarAsync(string codiIntern);

        Task<List<PrivilegiAssignat>> LlistaPerAlumneAsync(string nia);

        Task<List<PrivilegiAssignat>> LlistaPerGrupAsync(long idGrup);
        Task<List<PrivilegiAssignat>> LlistaPerClasseAsync(long idClasse);


    }
}
