using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class GrupCrearDTO
    {
        [Required]
        public int IdAnyEscolar { get; set; } //identificar únic

        [Required]
        public String IdProfessorTutor { get; set; } //tutor o tutora del grup
        
        [Required]
        public String IdGrup { get; set; } //identificar únic del grup

        public String Descripcio{ get; set; } 

    }

}
