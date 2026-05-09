using System;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.DTOs
{
    public class ConfiguracioKarmaEditarDTO 
    {


        [Required]
        public long IdConfiguracioKarma { get; set; }

        [Required]
        public double NumPuntsMinim { get; set; }

        [Required]
        public double NumPuntsMaxim { get; set; }

        [Required]
        public string ColorKarma { get; set; } = string.Empty; //Color del Karma

        [Required]
        public int NivellPrivilegis { get; set; }

    }
}
