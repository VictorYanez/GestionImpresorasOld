using GestionImpresoras.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GestionImpresoras.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext()
        {
        }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
     
        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Impresora>().Property(i => i.CodigoActivoFijo).HasMaxLength(20).IsRequired();
        }
       */
        // Definición de Modelos del Sistema
        public DbSet<Impresora> Impresoras { get; set; }
        public DbSet<Estado> Estados { get; set; }

    }
}
