using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.Models
{
    public class Puntuacio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdPuntuacio { get; set; }

        // =========================
        // Alumne
        // =========================
        [Required]
        public string NIA { get; set; } = string.Empty;
        public Alumne Alumne { get; set; } = null!;

        // =========================
        // Professor que crea la puntuació
        // =========================
        [Required]
        public string IdProfessor { get; set; } = string.Empty;
        public Professor Professor { get; set; } = null!;

        // =========================
        // Categoria (XMI)
        // =========================
        [Required]
        public long IdCategoria { get; set; }
        public Categoria Categoria { get; set; } = null!;

        // =========================
        // Snapshot de context
        // =========================
        [Required]
        public long IdClasse { get; set; }

        [Required]
        [StringLength(100)]
        public string NomClasse { get; set; } = string.Empty;

        public long? IdGrup { get; set; }

        [StringLength(100)]
        public string? NomGrup { get; set; }

        // =========================
        // Dades de puntuació
        // =========================
        [Required]
        public double NumPunts { get; set; }

        // 'S' = Sumar/Restar, 'I' = ReIniciar
        [Required]
        [StringLength(1)]
        public string Tipus { get; set; } = string.Empty;

        // NOT NULL (XMI)
        [Required]
        public string Motiu { get; set; } = string.Empty;

        public string? DescripcioAdicional { get; set; }

        // =========================
        // Dates
        // =========================
        [Required]
        public DateOnly DataEvent { get; set; }

        [Required]
        public DateTime DataCreacio { get; set; }

        // =========================
        // Avaluació
        // =========================
        [Required]
        public long IdAvaluacio { get; set; }
        public Avaluacio Avaluacio { get; set; } = null!;
    }
}