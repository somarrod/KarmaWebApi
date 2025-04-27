using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class Puntuacio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdPuntuacio { get; set; } //identificar únic

        [Required]
        public DateOnly DataEntrada { get; set; } = DateOnly.FromDateTime(DateTime.Now); //data

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El valor ha de ser com a mínim 1.")]
        public int Punts { get; set; } = 0; //Activa, per defecte true

        public String Motiu { get; set; }

        [ForeignKey("Professor")] 
        public int IdProfessorCreacio { get; set; } //Estàtica


        [ForeignKey("Periode")]
        [Required]
        public int IdPeriode { get; set; }

        //Navegacions
        public Professor ProfessorCreacio { get; set; }
        public Periode Periode { get; set; }




    }
}
