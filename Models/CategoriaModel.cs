using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.Models
{
    public class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdCategoria { get; set; }

        // descripció NOT NULL (100)
        [Required]
        [StringLength(100)]
        public string Descripcio { get; set; } = string.Empty;

        // número de punts (real)
        [Required]
        public double NumPunts { get; set; }

        // editable o no
        [Required]
        public bool Editable { get; set; }

        // comentaris per al professor (999)
        [StringLength(999)]
        public string? Comentaris { get; set; }

        // activa
        [Required]
        public bool Activa { get; set; } = true;

        // relació amb TipusCategoria
        [Required]
        public long IdTipusCategoria { get; set; }
        public TipusCategoria TipusCategoria { get; set; } = null!;
    }
}