using Microsoft.EntityFrameworkCore;

namespace KarmaWebAPI.Models
{
    public class AnyEscolarContext: DbContext
    {
        public AnyEscolarContext(DbContextOptions<AnyEscolarContext> options)
            : base(options) { }

        public DbSet<AnyEscolar> AnyEscolar { get; set; } = null!;

        //En el videl sobrescriu el servei per a que no permeta noms duplicats
        //A nosaltres de moment no ens fa falta
        //protected override void OnModelCreating(ModelBuilder modelBuilder) 
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<AnyEscolar>().HasIasIndex(c => c.Nombre).IsUnique();  
        //}
    }
}
