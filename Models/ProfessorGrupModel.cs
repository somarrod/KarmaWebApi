using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class ProfessorGrup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public String IdProfessorGrup { get; set; } //identificar únic de tamany màxim 50 que assigna gva

        [ForeignKey("Professor")]
        public String IdProfessor { get; set; } //identificar únic de tamany màxim 50 que assigna gva

        [ForeignKey("Materia")]
        public int IdMateria { get; set; } //identificar únic de la materia

        [ForeignKey("Grup")]
        public String IdAnyEscolar { get; set; } //identificar únic del grup

        [ForeignKey("Grup")]
        public String IdGrup { get; set; } //identificar únic del grup


        #region Navegacions
        public Grup Grup { get; set; } 
        public Materia Materia { get; set; }
        public Professor Professor { get; set; } //identificar únic de tamany màxim 50 que assigna gva
        #endregion Navegacions  
    }

}
