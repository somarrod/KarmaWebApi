using KarmaWebAPI.DTOs;
using KarmaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarmaWebAPI.Serveis.Interfaces
{
    public interface IProfessorService
    {


        // Llistar tots els professors ordenats per cognoms
        Task<List<Professor>> LlistarProfessorsAsync();
        
        // Llistar tots els professors ACTIUS ordenats per cognoms
        Task<List<Professor>> LlistarProfessorsActiusAsync();
        

        // Buscar professor per identificador
        Task<Professor?> ObtenirProfessorPerIdAsync(string idProfessor);



        // Crear professor (i assignar rols si pertoca)
        Task<Professor> CrearProfessorAsync(ProfessorDTO professorDto);

        // Editar professor (admin, equip directiu o self)
        Task<Professor> EditarProfessorAsync(
            ProfessorDTO professorDto,
            string userId,
            bool esAdminOEquipDirectiu
        );

        // Activar professor
        Task<Professor> ActivarProfessorAsync(string idProfessor);

        // Desactivar professor
        Task<Professor> DesactivarProfessorAsync(string idProfessor);

        // Eliminar professor
        Task EliminarProfessorAsync(string idProfessor);

        // Comprovació d’existència
        bool ProfessorExisteix(string idProfessor);
    }
}
