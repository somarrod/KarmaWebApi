using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs.DisplaySets
{

    public class ProfessorDeClasseDisplaySet
    {
        public long IdProfessorDeClasse { get; set; } //identificar únic de tamany màxim 50 que assigna gva

        [StringLength(50)]
        public string IdProfessor { get; set; } //identificar únic de tamany màxim 50 que assigna gva

        public string NomICognomsProfessor { get; set; } //nom del professor
        public long IdMateria { get; set; } //identificar únic de la materia

        public string NomMateria { get; set; } //nom de la materia  
                
        public long IdClasse { get; set; } //identificar únic de la classe
          
       
        public string NomClasse { get; set; } //nom de la classe
    }



}
