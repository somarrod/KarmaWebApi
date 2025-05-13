using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs.DisplaySets
{

    public class VPrivilegiPeriodeDisplaySet
    {
        public int IdAnyEscolar { get; set; } //identificar únic del grup

        [StringLength(50)]
        public String IdGrup { get; set; } //identificar únic del grup

        public String DescripcioGrup { get; set; } //nom del grup


        [StringLength(50)]
        public int IdPeriode { get; set; } 
        public DateOnly DataInici{ get; set; } //identificar únic de tamany màxim 50 que assigna gva
        public DateOnly DataFi { get; set; } //identificar únic de tamany màxim 50 que assigna gva

        public String NIA { get; set; } //nia de l'alumne

        [StringLength(401)]
        public String NomCompletAlumne { get; set; } //nom de la materia  

        public int IdPrivilegi { get; set; } //identificar únic de tamany màxim 50 que assigna gva
        [StringLength(255)]
        public String DescripcioPrivilegi { get; set; } //identificar únic de tamany màxim 50 que assigna gva

    }



}
