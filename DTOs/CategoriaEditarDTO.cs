using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class CategoriaEditarDTO
    {
        [Required]
        public int IdCategoria { get; set; } //identificar únic

        [Required]
        public String Descripcio { get; set; } //nom 

    }

}
