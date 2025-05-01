using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class MateriaEditarDTO
    {
        [Required]
        public int IdMateria { get; set; } //identificar únic

        [Required]
        public String Nom { get; set; } //nom de la materia

        [Required]
        public bool Activa { get; set; }
    }

}
