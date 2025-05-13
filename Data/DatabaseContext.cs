using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using KarmaWebAPI.Models;

namespace KarmaWebAPI.Data
{
    public class DatabaseContext : IdentityDbContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // Definición de DbSets para cada entidad
        public DbSet<Alumne> Alumne { get; set; } = null!;
        public DbSet<AlumneEnGrup> AlumneEnGrup { get; set; } = null!;
        public DbSet<AnyEscolar> AnyEscolar { get; set; } = null!;
        public DbSet<Categoria> Categoria { get; set; } = null!;
        public DbSet<ConfiguracioKarma> ConfiguracioKarma { get; set; } = null!;
        public DbSet<Grup> Grup { get; set; } = null!;
        public DbSet<Materia> Materia { get; set; } = null!;
        public DbSet<Periode> Periode { get; set; } = null!;
        public DbSet<PrivilegiAssignat> PrivilegiAssignat { get; set; } = null!;
        public DbSet<Privilegi> Privilegi { get; set; } = null!;
        public DbSet<ProfessorDeGrup> ProfessorDeGrup { get; set; } = null!;
        public DbSet<Professor> Professor { get; set; } = null!;
        public DbSet<Puntuacio> Puntuacio { get; set; } = null!;
        public DbSet<VPrivilegiPeriode> VPrivilegiPeriode { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Indica que la vista de base de datos no tiene clave primaria
            modelBuilder.Entity<VPrivilegiPeriode>()
                .HasNoKey()
                .ToView("VPrivilegiPeriode");

            // Configuración de la vista VPrivilegiPeriode
            modelBuilder.Entity<VPrivilegiPeriode>()
            .Property(v => v.IdPeriode)
            .HasColumnName("IdPeriode");

            modelBuilder.Entity<VPrivilegiPeriode>()
            .Property(v => v.IdAlumneEnGrup)
            .HasColumnName("IdAlumneEnGrup");

            modelBuilder.Entity<VPrivilegiPeriode>()
            .Property(v => v.IdPrivilegi)
            .HasColumnName("IdPrivilegi");

            modelBuilder.Entity<VPrivilegiPeriode>()
            .HasOne(v => v.AlumneEnGrup)
            .WithMany()
            .HasForeignKey(v => v.IdAlumneEnGrup);

            modelBuilder.Entity<VPrivilegiPeriode>()
            .HasOne(v => v.Periode)
            .WithMany()
            .HasForeignKey(v => v.IdPeriode);

            modelBuilder.Entity<VPrivilegiPeriode>()
            .HasOne(v => v.Privilegi)
            .WithMany()
            .HasForeignKey(v => v.IdPrivilegi);

            // Configuración de la entidad Grup
            modelBuilder.Entity<Grup>()
                .HasKey(g => new { g.IdAnyEscolar, g.IdGrup });

            modelBuilder.Entity<Grup>()
                .HasOne(g => g.AnyEscolar)
                .WithMany(a => a.Grups)
                .HasForeignKey(g => g.IdAnyEscolar);

            modelBuilder.Entity<Grup>()
                .HasOne(g => g.ProfessorTutor)
                .WithMany(a => a.GrupsTutoritzats)
                .HasForeignKey(g => g.IdProfessorTutor);

            // Configuración de la entidad AnyEscolar
            modelBuilder.Entity<AnyEscolar>()
                .HasKey(a => a.IdAnyEscolar);

            // Configuración de la entidad Alumne
            //modelBuilder.Entity<Alumne>()
            //    .Ignore(a => a.AlumnesEnGrup);

            // Configuración de la entidad AlumneEnGrup
            modelBuilder.Entity<AlumneEnGrup>()
                .HasKey(a => a.IdAlumneEnGrup);

            modelBuilder.Entity<AlumneEnGrup>()
                .HasOne(a => a.Alumne)
                .WithMany(ag => ag.AlumneEnGrups)
                .HasForeignKey(a => a.NIA);

            modelBuilder.Entity<AlumneEnGrup>()
                .HasOne(a => a.AnyEscolar)
                .WithMany(ag => ag.AlumnesEnGrup)
                .HasForeignKey(a => a.IdAnyEscolar)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AlumneEnGrup>()
                .HasOne(a => a.Grup)
                .WithMany(ag => ag.AlumnesEnGrup)
                .HasForeignKey(a => new { a.IdAnyEscolar, a.IdGrup })
                .OnDelete(DeleteBehavior.NoAction);

            // Configuración de la entidad ConfiguracioKarma
            modelBuilder.Entity<ConfiguracioKarma>()
                .HasOne(g => g.AnyEscolar)
                .WithMany(a => a.ConfiguracionsKarma)
                .HasForeignKey(g => g.IdAnyEscolar);

            // Configuración de la entidad Periode
            modelBuilder.Entity<Periode>()
                .HasOne(p => p.AnyEscolar)
                .WithMany(a => a.Periodes)
                .HasForeignKey(p => p.IdAnyEscolar);

            modelBuilder.Entity<Periode>()
                .Ignore(p => p.PrivilegisPeriode);

            // Configuración de la entidad Privilegi
            modelBuilder.Entity<Privilegi>()
                .Ignore(p => p.PrivilegisPeriode);

            modelBuilder.Entity<Privilegi>()
                .HasOne(p => p.AnyEscolar)
                .WithMany(a => a.Privilegis)
                .HasForeignKey(p => p.IdAnyEscolar);

            // Configuración de la entidad ProfessorDeGrup
            modelBuilder.Entity<ProfessorDeGrup>()
                .HasKey(p => p.IdProfessorDeGrup);

            modelBuilder.Entity<ProfessorDeGrup>()
                .HasOne(pg => pg.Materia)
                .WithMany(p => p.ProfessorsDelGrup)
                .HasForeignKey(pg => pg.IdMateria);

            modelBuilder.Entity<ProfessorDeGrup>()
                .HasOne(pg => pg.Professor)
                .WithMany(p => p.ProfessorDeGrups)
                .HasForeignKey(pg => pg.IdProfessor)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProfessorDeGrup>()
                .HasOne(pg => pg.Grup)
                .WithMany(g => g.ProfessorsDeGrup)
                .HasForeignKey(pg => new { pg.IdAnyEscolar, pg.IdGrup })
                .OnDelete(DeleteBehavior.NoAction);

            // Configuración de la entidad Puntuacio
            modelBuilder.Entity<Puntuacio>()
                .HasOne(p => p.AlumneEnGrup)
                .WithMany(p => p.Puntuacions)
                .HasForeignKey(p => p.IdAlumneEnGrup);

            modelBuilder.Entity<Puntuacio>()
                .HasOne(p => p.Periode)
                .WithMany(p => p.Puntuacions)
                .HasForeignKey(p => p.IdPeriode);

            // Llamada al método base
            base.OnModelCreating(modelBuilder);
        }
    }
}

