using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using KarmaWebAPI.Models;

namespace KarmaWebAPI.Data
{
    public class DatabaseContext: IdentityDbContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) 
        { }

        public DbSet<Alumne> Alumne { get; set; } = null!;
        public DbSet<AnyEscolar> AnyEscolar { get; set; } = null!;
        public DbSet<Categoria> Categoria { get; set; } = null!;
        public DbSet<ConfiguraKarma> ConfiguraKarma { get; set; } = null!;
        public DbSet<Grup> Grup { get; set; } = null!;
        
        public DbSet<Materia> Materia { get; set; } = null!;
        public DbSet<Periode> Periode { get; set; } = null!;
        public DbSet<PrivilegiAssignat> PrivilegiAssignat { get; set; } = null!;
        public DbSet<Privilegi> Privilegi{ get; set; } = null!;
        public DbSet<ProfessorGrup> ProfessorGrup { get; set; } = null!;
        public DbSet<Professor> Professor { get; set; } = null!;
        public DbSet<Puntuacio> Puntuacio { get; set; } = null!;
        public DbSet<VPrivilegiPeriode> VPrivilegiPeriode { get; set; } = null!;

 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Indica que la vista de base de dades no té clau primària
            modelBuilder.Entity<VPrivilegiPeriode>()
                        .HasNoKey();


            modelBuilder.Entity<Privilegi>()
                        .HasOne(p => p.AnyEscolar)
                        .WithMany(a => a.Privilegis)
                        .HasForeignKey(p => p.IdAnyEscolar);

            modelBuilder.Entity<Periode>()
                        .HasOne(p => p.AnyEscolar)
                        .WithMany(a => a.Periodes)
                        .HasForeignKey(p => p.IdAnyEscolar);


            modelBuilder.Entity<Grup>()
                        .HasOne(g => g.AnyEscolar)
                        .WithMany(a => a.Grups)
                        .HasForeignKey(g => g.IdAnyEscolar);

            modelBuilder.Entity<ConfiguraKarma>()
                        .HasOne(g => g.AnyEscolar)
                        .WithMany(a => a.ConfiguracionsKarma)
                        .HasForeignKey(g => g.IdAnyEscolar);


            modelBuilder.Entity<ProfessorGrup>()
                        .HasOne(pg => pg.Materia)
                        .WithMany(p => p.ProfessorsGrup)
                        .HasForeignKey(pg => pg.IdMateria);

            modelBuilder.Entity<ProfessorGrup>()
                        .HasOne(pg => pg.Professor)
                        .WithMany(p => p.ProfessorsEnGrup)
                        .HasForeignKey(pg => pg.IdProfessor);

            modelBuilder.Entity<ProfessorGrup>()
                        .HasOne(pg => pg.Grup)
                        .WithMany(g => g.ProfessorsGrup)
                        .HasForeignKey(pg => new { pg.IdAnyEscolar, pg.IdGrup });

            modelBuilder.Entity<AlumneEnGrup>().
                        HasOne(a => a.Alumne).
                        WithMany(ag => ag.AlumneEnGrups)
                        .HasForeignKey(a => a.NIA);

            modelBuilder.Entity<AlumneEnGrup>().
                        HasOne(a => a.Grup).
                        WithMany(ag => ag.AlumnesEnGrup)
                        .HasForeignKey(a => new { a.IdAnyEscolar, a.IdGrup });

            //PENDENT COMPLETAR LA RESTA DE DEFINICIONS


            base.OnModelCreating(modelBuilder);
        }


    }
}
