using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.Models
{
    public class Privilegi
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdPrivilegi { get; set; } //identificar únic

        [Required]
        public int Nivell { get; set; }

        [Required]
        [StringLength(255)]
        public string Descripcio { get; set; }
        
        [Required]
        [StringLength(1)]
        public string EsIndividualGrup { get; set; } //valors possibles: 'I' o 'G'


        // Clave foránea para referenciar a AnyEscolar
        [Required]
        public int IdAnyEscolar { get; set; }

        // Propiedad de navegación
        [ForeignKey("IdAnyEscolar")]
        public AnyEscolar AnyEscolar { get; set; }


    }
}
