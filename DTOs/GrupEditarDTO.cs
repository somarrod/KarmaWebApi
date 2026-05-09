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
        public String Nom { get; set; }

        [StringLength(50)]
        public String? IdProfessorTutor { get; set; } //tutor o tutora del grup  
    }

}
