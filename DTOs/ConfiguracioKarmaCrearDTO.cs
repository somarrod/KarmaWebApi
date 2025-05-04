using System;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.DTOs
{
    public class ConfiguracioKarmaCrearDTO : IValidatableObject
    {
        public int IdAnyEscolar { get; set; } // identificador de l'any escolar

        [Required]
        public int KarmaMinim { get; set; } // Karma mínim per a l'usuari

        [Required]
        public int KarmaMaxim { get; set; } // Karma màxim per a l'usuari

        [Required]
        public string ColorNivell { get; set; } // Color del nivell

        public int NivellPrivilegis { get; set; } // Nivell de privilegis per a l'usuari

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (KarmaMinim > KarmaMaxim)
            {
                yield return new ValidationResult(
                    "Karma Mínim ha de ser menor o igual que Karma Màxim.",
                    new[] { nameof(KarmaMinim), nameof(KarmaMaxim) });
            }
        }
    }
}
