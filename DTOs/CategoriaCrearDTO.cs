using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class CategoriaCrearDTO
    {

        [Required]
        public string Descripcio { get; set; }

        [Required]
        public int IdTipusCategoria { get; set; }


    }

}
