using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TesteTecnico.Infrastructure.Data
{
    public class TesteTecnicoDbContextFactory 
        : IDesignTimeDbContextFactory<TesteTecnicoDbContext>
    {
        public TesteTecnicoDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TesteTecnicoDbContext>();

            optionsBuilder.UseSqlite("Data Source=teste_tecnico.db");

            return new TesteTecnicoDbContext(optionsBuilder.Options);
        }
    }
}