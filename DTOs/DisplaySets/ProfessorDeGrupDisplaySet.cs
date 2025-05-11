using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs.DisplaySets
{

    public class ProfessorDeGrupDisplaySet
    {
        public int IdProfessorDeGrup { get; set; } //identificar únic de tamany màxim 50 que assigna gva

        [StringLength(50)]
        public String IdProfessor { get; set; } //identificar únic de tamany màxim 50 que assigna gva

        public int IdMateria { get; set; } //identificar únic de la materia
        public int IdAnyEscolar { get; set; } //identificar únic del grup

        [StringLength(50)]
        public String IdGrup { get; set; } //identificar únic del grup

        public String NomICognomsProfessor { get; set; } //nom del professor

        public String NomMateria { get; set; } //nom de la materia  

        public String DescripcioGrup { get; set; } //nom del grup
    }



}
