using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class ProfessorGrupModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public String IdProfessorGrup { get; set; } //identificar únic de tamany màxim 50 que assigna gva

        [ForeignKey("Professor")]
        public String IdProfessor { get; set; } //identificar únic de tamany màxim 50 que assigna gva

        public Professor Professor { get; set; } //identificar únic de tamany màxim 50 que assigna gva

        [ForeignKey("Grup")]
        public String IdGrup { get; set; } //identificar únic del grup

        public Grup Grup { get; set; } 

        [ForeignKey("Materia")]
        public int IdMateria { get; set; } //identificar únic de la materia
        public Materia Materia { get; set; } 

    }

}
