using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class AnyEscolarEditarDTO
    {
        public int IdAnyEscolar { get; set; } //identificar únic. Es solicita al crear

        [Required]
        public double SaldoKarmaInicial { get; set; }

        [Required]
        public bool ReiniciaCadaAvaluacio { get; set; }

        [Required]
        public bool Actiu { get; set; }

    }

}
