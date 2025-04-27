using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class Periode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPeriodo { get; set; } //identificar únic

        public DateOnly DataInici { get; set; }
        public DateOnly DataFi { get; set; }

        // Clave foránea para referenciar a AnyEscolar
        //És constant per al periode. Sols es solicita al crear el periode (relació estàtica)
        [Required]
        public int IdAnyEscolar { get; set; }

        // Propiedad de navegación  
        [ForeignKey("IdAnyEscolar")]
        public AnyEscolar AnyEscolar { get; set; } = null!;
    }
}
