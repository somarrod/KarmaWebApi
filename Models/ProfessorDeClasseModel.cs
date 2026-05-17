
using KarmaWebAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ProfessorDeClasse
{
    [Key]
    public long IdProfessorDeClasse { get; set; }

    [Required]
    
    public string IdProfessor { get; set; }
    [ForeignKey(nameof(IdProfessor))]
    public Professor Professor { get; set; } = null!;

    
    public long IdClasse { get; set; }
    [ForeignKey(nameof(IdClasse))]
    public Classe Classe { get; set; } = null!;

    [Required]
    
    public long IdMateria { get; set; }
    [ForeignKey(nameof(IdMateria))]
    public Materia Materia { get; set; } = null!;
}
