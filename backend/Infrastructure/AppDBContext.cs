using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure
{
    public class AppDBContext : DbContext
    {
        public DbSet<Tarefa> Tarefas => Set<Tarefa>();

        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tarefa>()
                .Property(t => t.Titulo)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Tarefa>()
                .Property(t => t.Descricao)
                .HasMaxLength(400);

            modelBuilder.Entity<Tarefa>()
                .Property(t => t.DataCriacao)
                .IsRequired();

            modelBuilder.Entity<Tarefa>()
                .Property(t => t.Status)
                .HasConversion<int>()
                .IsRequired();
        }

    }
}
