using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.Models
{
    public class Classe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdClasse { get; set; }

        [Required]
        [StringLength(20)]
        public string Nom { get; set; } = string.Empty;

        // Relació amb AnyEscolar
        [Required]
        [ForeignKey(nameof(AnyEscolar))]
        public int IdAnyEscolar { get; set; }

        public AnyEscolar AnyEscolar { get; set; } = null!;


        // Navegacions NECESSÀRIES, a classes que tenen referències a Classe (clau primària composta)
        public ICollection<Alumne> Alumnes { get; set; } = new List<Alumne>();

        public ICollection<ProfessorDeClasse> ProfessorsDeClasse { get; set; }
               = new List<ProfessorDeClasse>();

        public ICollection<Grup> Grups { get; set; } 
               = new List<Grup>();


    }
}
