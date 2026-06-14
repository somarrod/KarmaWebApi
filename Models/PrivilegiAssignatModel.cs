using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace KarmaWebAPI.Models
{
    public class PrivilegiAssignat

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdPrivilegiAssignat { get; set; }

        // 🔑 Camps definits en l’XMI
        [Required]
        public int NivellPrivilegi { get; set; }

        public string? Descripcio { get; set; }

        [Required]
        public DateTime DataCreacio { get; set; }

        [Required]
        public string Tipus { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string CodiIntern { get; set; } = string.Empty;

        public DateTime? DataExecucio { get; set; }

        // Relacions
        [Required]
        public string NIA { get; set; }
        [ForeignKey(nameof(NIA))]
        public Alumne Alumne { get; set; } = null!;

        [Required]
        public long IdPrivilegi { get; set; }

        [ForeignKey(nameof(IdPrivilegi))]
        public Privilegi Privilegi { get; set; } = null!;
    }
}