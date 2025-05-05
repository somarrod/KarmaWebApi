using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace KarmaWebAPI.DTOs
{

    public class AlumneEnGrupEditarDTO
    {
        [Required]
        public int IdAlumneEnGrup { get; set; }

        [Required]
        public int PuntuacioTotal { get; set; } = 0; //Puntuacio total de l'alumne en el grup i any escolar

    }

}
