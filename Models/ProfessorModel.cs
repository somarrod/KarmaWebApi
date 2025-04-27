using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class Professor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public String IdProfessor { get; set; } //identificar únic de tamany màxim 50 que assigna gva
        public String Nom { get; set; } //nom del professor   
        public String Cognoms { get; set; } //cognoms del professor

        public Boolean Actiu { get; set; } //indica si el professor està actiu o no 

        public String Email { get; set; } //correu electrònic del professor

        public ICollection<ProfessorDeGrup> ProfessorDeGrups { get; set; } //relació amb professor grups

        public ICollection<Grup> GrupsTutoritzats{ get; set; } //relació amb professor grups
    }
}
