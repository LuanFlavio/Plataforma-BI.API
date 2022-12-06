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
        public DbSet<VendasMensais> vendasMensal { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            //GSOFT-GGRAPHS
            builder.UseSqlServer(@"Server=servidor\sql2019;User Id=ggraphs;password=ggraphs2022;Database=GGRAPHS");
            base.OnConfiguring(builder);
        }
    }
}