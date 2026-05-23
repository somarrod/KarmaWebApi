using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class AlumneAssignarClasseIGrupDTO
    {
        [Required]
        [StringLength(10)] 
        public string NIA { get; set; }

        public long IdClasse { get; set; } 

        public long? IdGrup { get; set; } //grup --> ha de pertanyer a la classe
    }

}
