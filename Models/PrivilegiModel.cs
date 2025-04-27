using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.Models
{
    public class Privilegi
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPrivilegi { get; set; } //identificar únic

        [Required]
        public int Nivell { get; set; }

        [Required] 
        public String Descripcio { get; set; }
        
        [Required] 
        public String EsIndividualGrup { get; set; } //valors possibles: 'I' o 'G'


        // Clave foránea para referenciar a AnyEscolar
        [Required]
        public int IdAnyEscolar { get; set; }

        // Propiedad de navegación
        [ForeignKey("IdAnyEscolar")]
        public AnyEscolar AnyEscolar { get; set; }

    }
}
