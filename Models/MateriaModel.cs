using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class Materia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMateria { get; set; } //identificar únic
        public String Nom { get; set; } //nom de la materia
        public bool Activa { get; set; } = true; //descripcio de la materia
    }
}
