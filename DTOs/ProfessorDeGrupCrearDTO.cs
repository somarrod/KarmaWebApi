using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.DTOs
{
    public class ProfessorDeGrupCrearDTO
    {
        [StringLength(50)]
        public String IdProfessor { get; set; } //identificar únic de tamany màxim 50 que assigna gva

        public int IdMateria { get; set; } //identificar únic de la materia
        public int IdAnyEscolar { get; set; } //identificar únic del grup

        [StringLength(50)]
        public String IdGrup { get; set; } //identificar únic del grup
    }
}


