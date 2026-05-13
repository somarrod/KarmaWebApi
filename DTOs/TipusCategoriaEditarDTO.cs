using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class TipusCategoriaEditarDTO
    {
        [Required]
        public long IdTipusCategoria { get; set; }

        [Required]
        [StringLength(255)]
        public string Descripcio { get; set; } = string.Empty;

        [Required]
        public bool Actiu { get; set; }
    }

}
