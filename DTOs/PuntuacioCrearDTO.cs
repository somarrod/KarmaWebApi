using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class PuntuacioCrearDTO
    {
        [ForeignKey("AlumneEnGrup")]
        public int IdAlumneEnGrup { get; set; } //identificar únic

        [ForeignKey("Periode")]
        [Required]
        public int IdPeriode { get; set; }

        [ForeignKey("Categoria")]
        public int? IdCategoria { get; set; } //identificar únic

        [Required]
        [Range(-3, 3, ErrorMessage = "El valor ha de ser mínim -3 i màxim 3.")]
        public int Punts { get; set; } = 0; //Activa, per defecte true

        [StringLength(255)]
        public String Motiu { get; set; }

    }

}
