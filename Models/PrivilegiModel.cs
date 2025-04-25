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
        public String EsIndividualGrup { get; set; }


        // Clave foránea para referenciar a AnyEscolar
        [Required]
        public int IdAnyEscolar { get; set; }
        
        // Propiedad de navegación
        public AnyEscolar AnyEscolar { get; set; }

    }
}
