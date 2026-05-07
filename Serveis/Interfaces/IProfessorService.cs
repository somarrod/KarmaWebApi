using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IProfessorService
    {
        public List<Professor> GetProfessors();
        public Task<ActionResult<Professor>> CrearProfessorAsync(ProfessorDTO professorDto);

        public Task<ActionResult<Professor>> ActivarProfessorAsync(string idProfessor);

        public Task<ActionResult<Professor>> DesactivarProfessorAsync(string idProfessor);

        public Task PertanyEquipDirectiuAsync(string idProfessor, bool pertanyAEquipDirectiu);
        public bool ProfessorExisteix(string idProfessor);
    }
}
