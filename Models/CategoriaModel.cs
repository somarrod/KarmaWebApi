using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCategoria { get; set; } //identificar únic
        
        [Required]
        public String Descripcio { get; set; } //nom 

        [Required]
        public bool Activa { get; set; } = true; //Activa, per defecte true
    }
}
