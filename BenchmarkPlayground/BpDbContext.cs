using Microsoft.EntityFrameworkCore;

namespace BenchmarkPlayground;

public class BpDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=127.0.0.1,1433;;User ID=sa;Password=P@ssw0rd;Initial Catalog=BenchmarkPlayground;");
    }

    public DbSet<Product> Products { get; set; }
}