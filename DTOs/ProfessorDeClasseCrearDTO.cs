using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.DTOs
{
    public class ProfessorDeClasseCrearDTO
    {
        [StringLength(50)]
        public String IdProfessor { get; set; } //identificar únic de tamany màxim 50 que assigna gva

        public int IdMateria { get; set; } //identificar únic de la materia

        [StringLength(50)]
        public String IdClasse { get; set; } //identificar únic del grup
    }
}


