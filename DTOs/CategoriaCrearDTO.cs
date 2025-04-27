using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class CategoriaCrearDTO
    {
        [Required]
        public String Descripcio { get; set; } //nom 

    }

}
