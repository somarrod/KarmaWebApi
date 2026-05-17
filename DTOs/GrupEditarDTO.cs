using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class GrupEditarDTO
    {
        [Required]
        public long IdGrup { get; set; } //identificar únic del grup  

        [Required]
        [StringLength(20)] // Corregido: Se usa paréntesis en lugar de '=' y se pasa el argumento requerido.  
        public string Nom { get; set; }

    }

}
