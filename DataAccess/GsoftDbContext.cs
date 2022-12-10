using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class GsoftDbContext : DbContext
    {
        public GsoftDbContext() : base() { }

        public DbSet<Empresas> empresas { get; set; }
        public DbSet<Usuarios> usuarios { get; set; }
        public DbSet<Metas> metas { get; set; }
        public DbSet<VendasDiarias> vendasDiarias { get; set; }
        public DbSet<VendasMensal> vendasMensal { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            //GSOFT-GGRAPHS
            builder.UseSqlServer(/*connection string*/);
            base.OnConfiguring(builder);
        }
    }
}