using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.DTOs
{
    public class ClasseCrearDTO
    {
        [Required]
        public int IdAnyEscolar { get; set; }
        [Required]
        public string Nom { get; set; } = string.Empty;
    }
}
