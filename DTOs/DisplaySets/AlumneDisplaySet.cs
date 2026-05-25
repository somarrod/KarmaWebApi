using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs.DisplaySets
{

    public class AlumneDisplaySet
    {
        public string NIA { get; set; }
        public int IdAnyEscolar { get; set; }
        public string Nom { get; set; }

        public string Cognoms { get; set; }


        public string? IdClasse { get; set; }
        public string? NomClasse { get; set; }

        public string? IdGrup { get; set; }
        public string? NomGrup { get; set; }

        public string? KarmaBaseGrup { get; set; }

        public List<string> AlumnesEnGrup{ get; set; }
    }

}
