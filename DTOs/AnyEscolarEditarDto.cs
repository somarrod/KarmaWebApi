using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class AnyEscolarEditarDto
    {
        public int IdAnyEscolar { get; set; } //identificar únic. Es solicita al crear
        public DateOnly DataIniciCurs { get; set; }
        public DateOnly DataFiCurs { get; set; }

        [Required]
        public double SaldoKarmaInicial { get; set; }

        [Required]
        public bool ReiniciaCadaAvaluacio { get; set; }

        [Required]
        public bool Actiu { get; set; }

    }

}
