using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.DTOs
{
    public class ClasseEditarDTO
    {
        [Required]
        public int IdAnyEscolar { get; set; }
        [Required]
        public long IdClasse { get; set; }
        [Required]
        public string Nom { get; set; } = string.Empty;
    }
}
