
using KarmaWebAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ProfessorDeClasse
{
    [Key]
    public long IdProfessorDeClasse { get; set; }

    [Required]
    [ForeignKey(nameof(Professor))]
    public string IdProfessor { get; set; }
    public Professor Professor { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Classe))]
    public long IdClasse { get; set; }
    public Classe Classe { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Materia))]
    public long IdMateria { get; set; }
    public Materia Materia { get; set; } = null!;
}
