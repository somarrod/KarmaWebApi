using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.Models
{
    public class Avaluacio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdAvaluacio { get; set; }

        [Required]
        public string Nom { get; set; } = string.Empty;

        [Required]
        public DateOnly DataInicial { get; set; }

        [Required]
        public DateOnly DataFinal { get; set; }

        [Required]
        public double NotaMinimaKarma { get; set; }

        [Required]
        public double NotaMaximaKarma { get; set; }

        // Relació (navegació només ací, com marca el model)
        [Required]
        
        public int IdAnyEscolar { get; set; }
        [ForeignKey("IdAnyEscolar")]
        public AnyEscolar AnyEscolar { get; set; } = null!;
    }
}

