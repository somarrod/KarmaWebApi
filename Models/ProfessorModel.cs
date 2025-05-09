using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class Professor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(50)]
        public String IdProfessor { get; set; } //identificar únic de tamany màxim 50 que assigna gva
        
        [StringLength(200)]
        public string Nom { get; set; } //nom del professor   
        
        [StringLength(200)]
        public string Cognoms { get; set; } //cognoms del professor

        public Boolean Actiu { get; set; } //indica si el professor està actiu o no 

        [StringLength(255)]
        public string Email { get; set; } //correu electrònic del professor

        public ICollection<ProfessorDeGrup> ProfessorDeGrups { get; set; } //relació amb professor grups

        public ICollection<Grup> GrupsTutoritzats{ get; set; } //relació amb professor grups
    }
}
