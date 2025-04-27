using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class Materia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMateria { get; set; } //identificar únic

        [Required]
        public String Nom { get; set; } //nom de la materia

        [Required]
        public bool Activa { get; set; } = true; //descripcio de la materia

        public ICollection<ProfessorDeGrup> ProfessorsDelGrup { get; set; } //relació amb professor grups
    }
}
