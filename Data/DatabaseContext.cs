using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using KarmaWebAPI.Models;

namespace KarmaWebAPI.Data
{
    public class DatabaseContext : IdentityDbContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // Definición de DbSets para cada entidad
        public DbSet<Alumne> Alumnes { get; set; } = null!;
        public DbSet<AnyEscolar> AnyEscolars { get; set; } = null!;
        public DbSet<Categoria> Categories { get; set; } = null!;

        public DbSet<Classe> Classes { get; set; } = null!;
        public DbSet<TipusCategoria> TipusCategories { get; set; } = null!;
        public DbSet<ConfiguracioKarma> ConfiguracionsKarma { get; set; } = null!;
        public DbSet<Grup> Grups { get; set; } = null!;
        public DbSet<Materia> Materies { get; set; } = null!;
        public DbSet<Avaluacio> Avaluacions { get; set; } = null!;
        public DbSet<PrivilegiAssignat> PrivilegisAssignats { get; set; } = null!;
        public DbSet<Privilegi> Privilegis { get; set; } = null!;
        public DbSet<ProfessorDeClasse> ProfessorsDeClasse { get; set; } = null!;
        public DbSet<Professor> Professors { get; set; } = null!;
        public DbSet<Puntuacio> Puntuacions { get; set; } = null!;

        public DbSet<KarmaAlumne> KarmaAlumnes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configuración de la entidad AnyEscolar
            modelBuilder.Entity<AnyEscolar>()
                .HasKey(a => a.IdAnyEscolar);

            // =========================
            // CLASSE → clau composta
            // =========================
            modelBuilder.Entity<Classe>()
                .HasKey(c => new { c.IdAnyEscolar, c.IdClasse });

            // =========================
            // ALUMNE → FK composta cap a CLASSE
            // =========================
            modelBuilder.Entity<Alumne>()
                .HasOne(a => a.Classe)
                .WithMany(c => c.Alumnes)
                .HasForeignKey(a => new { a.IdAnyEscolar, a.IdClasse })
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // PROFESSORDECLASSE → FK composta cap a CLASSE
            // =========================
            //modelBuilder.Entity<ProfessorDeClasse>()
            //    .HasOne(pc => pc.Classe)
            //    .WithMany(c => c.ProfessorsDeClasse)
            //    .HasForeignKey(pc => new { pc.IdAnyEscolar, pc.IdClasse })
            //    .OnDelete(DeleteBehavior.Restrict);


            // =========================
            // GRUP → FK composta
            // =========================
            //modelBuilder.Entity<Grup>()
            //    .HasOne(g => g.Classe)
            //    .WithMany(c => c.Grups)   // o .WithMany() si no vols navegació
            //    .HasForeignKey(g => new { g.IdAnyEscolar, g.IdClasse })
            //    .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Privilegi>()
                .HasOne(p => p.AnyEscolar)
                .WithMany(a => a.Privilegis)
                .HasForeignKey(p => p.IdAnyEscolar)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de la entidad ProfessorDeClasse
            modelBuilder.Entity<ProfessorDeClasse>()
                .HasKey(p => p.IdProfessorDeClasse);


            modelBuilder.Entity<Puntuacio>()
                .HasOne(p => p.Professor)
                .WithMany()
                .HasForeignKey(p => p.IdProfessor)
                .OnDelete(DeleteBehavior.Restrict);


            //ÍNDEX
            modelBuilder.Entity<Puntuacio>()
                .HasIndex(p => new { p.NIA, p.IdAvaluacio });

            modelBuilder.Entity<KarmaAlumne>()
                .HasIndex(k => new { k.NIA, k.IdAvaluacio });

            modelBuilder.Entity<ProfessorDeClasse>()
                .HasIndex(p => new { p.IdProfessor, p.IdClasse, p.IdMateria })
                .IsUnique();


            modelBuilder.Entity<Categoria>()
                .HasOne(c => c.TipusCategoria)
                .WithMany(t => t.Categories)
                .HasForeignKey(c => c.IdTipusCategoria);


            // Llamada al método base
            base.OnModelCreating(modelBuilder);
        }
    }
}

