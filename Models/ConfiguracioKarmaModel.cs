using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.Models
{
    public class ConfiguracioKarma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdConfiguracioKarma { get; set; }

        [Required]
        public double NumPuntsMinim { get; set; }

        [Required]
        public double NumPuntsMaxim { get; set; }

        [Required]
        public string ColorKarma { get; set; } = string.Empty;

        [Required]
        public int NivellPrivilegis { get; set; }

        // Relació amb AnyEscolar
        [Required]
        
        public int IdAnyEscolar { get; set; }
        [ForeignKey(nameof(IdAnyEscolar))]
        public AnyEscolar AnyEscolar { get; set; } = null!;
    }
}