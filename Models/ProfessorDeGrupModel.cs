using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class ProfessorDeGrup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public String IdProfessorDeGrup { get; set; } //identificar únic de tamany màxim 50 que assigna gva

        [ForeignKey("Professor")]
        public String IdProfessor { get; set; } //identificar únic de tamany màxim 50 que assigna gva

        [ForeignKey("Materia")]
        public int IdMateria { get; set; } //identificar únic de la materia

        [ForeignKey("AnyEscolar")]
        public int IdAnyEscolar { get; set; } //identificar únic del grup

        [ForeignKey("Grup")]
        [StringLength(50)]
        public String IdGrup { get; set; } //identificar únic del grup


        #region Navegacions
        public AnyEscolar AnyEscolar { get; set; }
        public Grup Grup { get; set; } 
        public Materia Materia { get; set; }
        public Professor Professor { get; set; } //identificar únic de tamany màxim 50 que assigna gva
        #endregion Navegacions  
    }

}
