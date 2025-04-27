using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using KarmaWebAPI.Models;

namespace KarmaWebAPI.Data
{
    public class DatabaseContext: IdentityDbContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) 
        { }

        public DbSet<AnyEscolar> AnyEscolar { get; set; } = null!;
        public DbSet<Privilegi> Privilegi{ get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Indica que la vista de base de dades no té clau primària
            modelBuilder.Entity<VPrivilegiPeriode>().HasNoKey(); 

            modelBuilder.Entity<Privilegi>()
                        .HasOne(p => p.AnyEscolar)
                        .WithMany(a => a.Privilegis)
                        .HasForeignKey(p => p.IdAnyEscolar);

            base.OnModelCreating(modelBuilder);
        }


    }
}
