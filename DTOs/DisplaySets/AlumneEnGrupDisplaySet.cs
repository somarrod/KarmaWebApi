using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs.DisplaySets
{

    public class AlumneEnGrupDisplaySet
    {
        public int IdAlumneEnGrup { get; set; }
        public string NIA { get; set; }
        public int IdAnyEscolar { get; set; }
        public string IdGrup { get; set; }
        public int PuntuacioTotal { get; set; }
        public string Karma { get; set; }
    }



}
