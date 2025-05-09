using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class Puntuacio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPuntuacio { get; set; } //identificar únic

        [Required]
        public DateOnly DataEntrada { get; set; } = DateOnly.FromDateTime(DateTime.Now); //data

        [Required]
        [Range(-3, 3, ErrorMessage = "El valor ha de ser mínim -3 i màxim 3.")]
        public int Punts { get; set; } = 0; //Activa, per defecte true
        
        [StringLength(255)]
        public String Motiu { get; set; }

        [StringLength(50)]
        public String? UsuariCreacio { get; set; } 

        [ForeignKey("Periode")]
        [Required]
        public int IdPeriode { get; set; }

        [ForeignKey("Categoria")]
        public int? IdCategoria { get; set; } //identificar únic

        [ForeignKey("AlumneEnGrup")]
        public int IdAlumneEnGrup { get; set; } //identificar únic

        //Navegacions
        public Periode Periode { get; set; } = null!;

        public Categoria Categoria { get; set; }

        public AlumneEnGrup AlumneEnGrup { get; set; } = null!;

    }
}
