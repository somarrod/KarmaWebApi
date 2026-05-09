using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.Models
{
    public class Privilegi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdPrivilegi { get; set; }

        [Required]
        public string Descripcio { get; set; }

        [Required]
        public string Tipus { get; set; }

        // Depén del curs escolar
        [Required]
        public long IdAnyEscolar { get; set; }
        public AnyEscolar AnyEscolar { get; set; } = null!;

        // nivell mínim de karma necessari
        [Required]
        public int NivellPrivilegi { get; set; }

        public bool Actiu { get; set; } = true;
    }
}
