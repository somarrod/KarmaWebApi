using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.Models
{
    public class Classe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string IdClasse { get; set; }

        [Required]
        public string Nom { get; set; } = string.Empty;

        // Relació amb AnyEscolar
        [Required]
        [ForeignKey(nameof(AnyEscolar))]
        public int IdAnyEscolar { get; set; }

        public AnyEscolar AnyEscolar { get; set; } = null!;
    }
}
