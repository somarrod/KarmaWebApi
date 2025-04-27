using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class MateriaCrearDTO
    {
        [Required]
        public String Nom { get; set; } //nom de la materia
    }

}
