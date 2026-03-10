using Microsoft.EntityFrameworkCore;
using TesteTecnico.Domain.Entities; 

namespace TesteTecnico.Infrastructure.Data
{
    public class TesteTecnicoDbContext : DbContext
    {
        public TesteTecnicoDbContext(DbContextOptions<TesteTecnicoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pessoa>()
                .HasMany(p => p.Transacoes)
                .WithOne(t => t.Pessoa)
                .HasForeignKey(t => t.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pessoa>()
                .Property(p => p.Nome)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Categoria>()
                .Property(c => c.Descricao)
                .HasMaxLength(400)
                .IsRequired();

            modelBuilder.Entity<Transacao>()
                .Property(t => t.Descricao)
                .HasMaxLength(400)
                .IsRequired();

            modelBuilder.Entity<Transacao>()
                .Property(t => t.Valor)
                .HasPrecision(18, 2);
        }
    }
}