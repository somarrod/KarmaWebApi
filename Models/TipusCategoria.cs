using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarmaWebAPI.Models
{
    //Cada categoria té un tipus de categoria
    public class TipusCategoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTipusCategoria { get; set; }

        [Required]
        [StringLength(100)]
        public string Descripcio { get; set; }

        public bool Actiu { get; set; } = true;

    }
}