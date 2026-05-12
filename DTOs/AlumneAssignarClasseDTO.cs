using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class AlumneAssignarClasseDTO
    {
        [Required]
        [StringLength(10)] 
        public string NIA { get; set; }

        public int IdAnyEscolar { get; set; } //opcional i ha de coincidir en la classe

        public long IdClasse { get; set; } //classe --> opcional
    }

}
