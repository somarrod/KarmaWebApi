using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class Grup
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("AnyEscolar")]
        public int IdAnyEscolar { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public String IdGrup { get; set; } //identificar únic

        [Required]
        [StringLength(500)] // Corregido: Se usa paréntesis en lugar de '=' y se pasa el argumento requerido.  
        public String Descripcio { get; set; }

        public String KarmaBase { get; set; } = "CAP KARMA"; //Karma Base: Es el mínim de tots els karmes dels alumnes del grup


        // Nova propietat per al professor tutor
        [ForeignKey("Professor")]
        [StringLength(50)]
        public String? IdProfessorTutor { get; set; }

        #region Navegacions

        // Propietat de navegació
        public AnyEscolar AnyEscolar { get; set; } = null!;

        // Propietat de navegació per al professor tutor
        public Professor ProfessorTutor { get; set; } = null!;

        public ICollection<ProfessorDeGrup> ProfessorsDeGrup { get; set; }
        public ICollection<AlumneEnGrup> AlumnesEnGrup { get; set; }
        #endregion Navegacions

    }
}
