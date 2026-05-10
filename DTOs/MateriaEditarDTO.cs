using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class MateriaEditarDTO
    {
        [Required]
        public long IdMateria { get; set; } //identificar únic

        [Required]
        public string Nom { get; set; } //nom de la materia

    }

}
