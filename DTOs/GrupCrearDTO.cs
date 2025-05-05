using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class GrupCrearDTO
    {
        [Required]
        public int IdAnyEscolar { get; set; } //identificar únic
        
        [Required]
        [StringLength(50)]
        public String IdGrup { get; set; } //identificar únic del grup

        [Required]
        [StringLength(500)] // Corregido: Se usa paréntesis en lugar de '=' y se pasa el argumento requerido.  
        public String Descripcio{ get; set; }

        [StringLength(50)]
        public String? IdProfessorTutor { get; set; } //tutor o tutora del grup

    }

}
