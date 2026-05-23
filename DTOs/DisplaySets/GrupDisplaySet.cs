using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.DTOs.DisplaySets
{
    public class GrupDisplaySet
    {
        public long IdGrup { get; set; } //identificar únic del grup  
        public string Nom { get; set; }

        public long IdClasse { get; set; }
        public string NomClasse { get; set; }

        public int IdAnyEscolar { get; set; }
        public List<string> Alumnes { get; set; }

        public string KarmaBase { get; set; }
    }
}
