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
        public DbSet<TipusCategoria> TipusCategoria { get; set; } = null!;
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
            //// Configuración de la entidad Grup
            //modelBuilder.Entity<Grup>()
            //    .HasKey(g => new { g.IdAnyEscolar, g.IdGrup });

            //modelBuilder.Entity<Grup>()
            //    .HasOne(g => g.AnyEscolar)
            //    .WithMany(a => a.Grups)
            //    .HasForeignKey(g => g.IdAnyEscolar);


            modelBuilder.Entity<ProfessorDeClasse>()
                .HasIndex(p => new { p.IdProfessor, p.IdClasse, p.IdMateria })
                .IsUnique();

            // Configuración de la entidad AnyEscolar
            modelBuilder.Entity<AnyEscolar>()
                .HasKey(a => a.IdAnyEscolar);

            // Configuración de la entidad ConfiguracioKarma
            modelBuilder.Entity<ConfiguracioKarma>()
                .HasOne(g => g.AnyEscolar)
                .WithMany(a => a.ConfiguracionsKarma)
                .HasForeignKey(g => g.IdAnyEscolar);

            // Configuración de la entidad Avaluacio
            modelBuilder.Entity<Avaluacio>()
                .HasOne(p => p.AnyEscolar)
                .WithMany(a => a.Avaluacios)
                .HasForeignKey(p => p.IdAnyEscolar);

            modelBuilder.Entity<Privilegi>()
                .HasOne(p => p.AnyEscolar)
                .WithMany(a => a.Privilegis)
                .HasForeignKey(p => p.IdAnyEscolar);

            // Configuración de la entidad ProfessorDeClasse
            modelBuilder.Entity<ProfessorDeClasse>()
                .HasKey(p => p.IdProfessorDeClasse);



            // Llamada al método base
            base.OnModelCreating(modelBuilder);
        }
    }
}

