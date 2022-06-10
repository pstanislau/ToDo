using Microsoft.EntityFrameworkCore;
using Desafio.Domain.Entity;


namespace Desafio.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
        {
        }
        public virtual DbSet<Tarefa> Lista { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tarefa>(entity =>
            {
                entity.Property(x => x.Nome).IsUnicode(false);
                entity.Property(x => x.Descricao).IsUnicode(false);
            });
        }
    }
}
