using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KarmaWebAPI.Models
{
    public class Alumne
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public String NIA { get; set; } //identificador únic de tamany màxim 10 que assigna gva

        public String Nom { get; set; } //nom de l'alumne   
        public String Cognoms { get; set; } //cognoms de l'alumne   

        public Boolean Actiu { get; set; } //indica si està actiu o no 
        public String Email { get; set; } //correu electrònic 

    }
}
