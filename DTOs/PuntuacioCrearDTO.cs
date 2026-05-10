using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class PuntuacioCrearDTO
    {
        [ForeignKey("Alumne")]
        public string NIA { get; set; } //identificar únic

        [ForeignKey("Avaluacio")]
        [Required]
        public int IdAvaluacio { get; set; }

        [ForeignKey("Categoria")]
        public long IdCategoria { get; set; } //identificar únic

        public int NumPunts { get; set; } = 0; //Activa, per defecte true

        [StringLength(255)]
        public String Motiu { get; set; }

        [StringLength(255)]
        public String DescripcioAdicional { get; set; }
    }

}
