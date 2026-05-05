using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.DTOs
{

    public class AnyEscolarCrearDto
    {
        public DateOnly DataIniciCurs { get; set; }
        public DateOnly DataFiCurs { get; set; }

        [Required]
        public double SaldoKarmaInicial { get; set; }

        [Required]
        public bool ReiniciaCadaAvaluacio { get; set; }
    }

}
