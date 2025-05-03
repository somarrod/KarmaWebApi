using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IProfessorService
    {
        public List<Professor> GetProfessors();
        public Task<ActionResult<Professor>> CrearProfessorAsync(ProfessorDTO professorDto);

        public Task<ActionResult<Professor>> ActivarProfessorAsync(String idProfessor);

        public Task<ActionResult<Professor>> DesactivarProfessorAsync(String idProfessor);

        public bool ProfessorExisteix(string idProfessor);
    }
}
