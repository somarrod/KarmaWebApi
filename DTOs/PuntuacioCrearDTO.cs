using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class PuntuacioCrearDTO
    {
        public string NIA { get; set; } //identificar únic

        [Required]
        public int IdAvaluacio { get; set; }

        public long IdCategoria { get; set; } //identificar únic

        public int NumPunts { get; set; } = 0; //Activa, per defecte true

        [StringLength(255)]
        public string Motiu { get; set; }

        [StringLength(255)]
        public string? DescripcioAdicional { get; set; }

        [Required]
        public DateOnly DataEvent { get; set; }
    }

}
