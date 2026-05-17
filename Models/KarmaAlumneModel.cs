using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.Models
{
    public class KarmaAlumne
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdKarmaAlumne { get; set; }

        // ---------- RELACIONS ----------

        [Required]
        
        public string NIA { get; set; }
        [ForeignKey(nameof(NIA))]
        public Alumne Alumne { get; set; } = null!;

        [Required]
        public long IdAvaluacio { get; set; }
        [ForeignKey(nameof(IdAvaluacio))]
        public Avaluacio Avaluacio { get; set; } = null!;

        // ---------- PUNTS ----------

        public double NumPuntsInicials { get; set; }
        public double NumPuntsActuals { get; set; }

        // ---------- KARMA (color / nivell) ----------

        public string KarmaInicial { get; set; } = string.Empty;
        public string KarmaActual { get; set; } = string.Empty;

        // ---------- NOTA ----------

        public double? NotaKarma { get; set; }

        // ---------- ALTRES ----------

        public string? Comentaris { get; set; }

        // Derivat en el model XMI
        public double? NumPuntsAvaluacioAnteriorDrv { get; set; }
    }
}