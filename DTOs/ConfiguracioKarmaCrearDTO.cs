using System;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.DTOs
{
    public class ConfiguracioKarmaCrearDTO : IValidatableObject
    {



        [Required]
        public double NumPuntsMinim { get; set; }

        [Required]
        public double NumPuntsMaxim { get; set; }

        [Required]
        public string ColorKarma { get; set; } = string.Empty;

        [Required]
        public int NivellPrivilegis { get; set; }

        [Required]
        public int IdAnyEscolar { get; set; }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NumPuntsMinim >= NumPuntsMaxim)
            {
                yield return new ValidationResult(
                    "Karma Mínim ha de ser menor o igual que Karma Màxim.",
                    new[] { nameof(NumPuntsMinim), nameof(NumPuntsMaxim) });
            }
        }
    }
}
