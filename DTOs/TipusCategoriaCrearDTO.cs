using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.DTOs
{
    public class TipusCategoriaCrearDTO
    {
        [Required]
        [StringLength(100)]
        public string Descripcio { get; set; }
    }
}
