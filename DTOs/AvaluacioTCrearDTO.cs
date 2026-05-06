using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.DTOs.Avaluacio
{
    public class AvaluacioTCrearDTO
    {
        [Required]
        public string Nom { get; set; } = string.Empty;

        [Required]
        public DateOnly DataInicial { get; set; }

        [Required]
        public DateOnly DataFinal { get; set; }

        [Required]
        public double NotaMinimaKarma { get; set; }

        [Required]
        public double NotaMaximaKarma { get; set; }

        [Required]
        public int IdAnyEscolar { get; set; }
    }
}