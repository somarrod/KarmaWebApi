using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using KarmaWebAPI.Models;

namespace KarmaWebAPI.Data
{
    public class DatabaseContext: IdentityDbContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) 
        { }

        public DbSet<Alumne> Alumnes { get; set; } = null!;
        public DbSet<AnyEscolar> AnysEscolar { get; set; } = null!;
        public DbSet<Categoria> Categories { get; set; } = null!;
        public DbSet<ConfiguraKarma> ConfiguracionsKarma { get; set; } = null!;
        public DbSet<Grup> Grups { get; set; } = null!;
        public DbSet<Materia> Materies { get; set; } = null!;
        public DbSet<Periode> Periodes { get; set; } = null!;
        public DbSet<PrivilegiAssignat> PrivilegisAssignats { get; set; } = null!;
        public DbSet<Privilegi> Privilegis{ get; set; } = null!;
        public DbSet<ProfessorDeGrup> ProfessorsDeGrup { get; set; } = null!;
        public DbSet<Professor> Professors { get; set; } = null!;
        public DbSet<Puntuacio> Puntuacions { get; set; } = null!;
        public DbSet<VPrivilegiPeriode> VPrivilegisPeriode { get; set; } = null!;

 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 

            modelBuilder.Entity<AlumneEnGrup>()
                        .HasOne(a => a.Alumne)
                        .WithMany(ag => ag.AlumneEnGrups)
                        .HasForeignKey(a => a.NIA);

            modelBuilder.Entity<AlumneEnGrup>().
                        HasOne(a => a.Grup).
                        WithMany(ag => ag.AlumnesEnGrup)
                        .HasForeignKey(a => new { a.IdAnyEscolar, a.IdGrup });


            modelBuilder.Entity<ConfiguraKarma>()
                        .HasOne(g => g.AnyEscolar)
                        .WithMany(a => a.ConfiguracionsKarma)
                        .HasForeignKey(g => g.IdAnyEscolar);

            modelBuilder.Entity<Grup>()
                        .HasOne(g => g.AnyEscolar)
                        .WithMany(a => a.Grups)
                        .HasForeignKey(g => g.IdAnyEscolar);

            modelBuilder.Entity<Grup>()
                        .HasOne(g => g.ProfessorTutor)
                        .WithMany(a => a.GrupsTutoritzats)
                        .HasForeignKey(g => g.IdProfessorTutor);

            modelBuilder.Entity<Periode>()
                        .HasOne(p => p.AnyEscolar)
                        .WithMany(a => a.Periodes)
                        .HasForeignKey(p => p.IdAnyEscolar);


            modelBuilder.Entity<Privilegi>()
                        .HasOne(p => p.AnyEscolar)
                        .WithMany(a => a.Privilegis)
                        .HasForeignKey(p => p.IdAnyEscolar);

            modelBuilder.Entity<ProfessorDeGrup>()
                        .HasOne(pg => pg.Materia)
                        .WithMany(p => p.ProfessorsDelGrup)
                        .HasForeignKey(pg => pg.IdMateria);

            modelBuilder.Entity<ProfessorDeGrup>()
                        .HasOne(pg => pg.Professor)
                        .WithMany(p => p.ProfessorDeGrups)
                        .HasForeignKey(pg => pg.IdProfessor);

            modelBuilder.Entity<ProfessorDeGrup>()
                        .HasOne(pg => pg.Grup)
                        .WithMany(g => g.ProfessorsGrup)
                        .HasForeignKey(pg => new { pg.IdAnyEscolar, pg.IdGrup });

            modelBuilder.Entity<Puntuacio>()
                        .HasOne(p => p.AlumneEnGrup)
                        .WithMany(p => p.Puntuacions)
                        .HasForeignKey(p => p.IdAlumneEnGrup);

            modelBuilder.Entity<Puntuacio>()
                        .HasOne(p => p.Periode)
                        .WithMany(p => p.Puntuacions)
                        .HasForeignKey(p => p.IdPeriode);



            //PENDENT COMPLETAR LA RESTA DE DEFINICIONS
            //Repassar-ho tot que no sé si falta algun



            // Indica que la vista de base de dades no té clau primària
            modelBuilder.Entity<VPrivilegiPeriode>()
                        .HasNoKey();


            
            base.OnModelCreating(modelBuilder);
        }


    }
}
