using BenchmarkDotNet.Running;

namespace BenchmarkPlayground;

internal static class Program
{
    private static void Main(string[] args)
    {
        using var dbContext = new BpDbContext();
        dbContext.Database.EnsureCreated();

        if (dbContext.Products.Any())
        {
            Console.WriteLine("SeedData already exists");
        }
        else
        {
            Console.WriteLine("Start to seed...");
            SeedData.Run(dbContext, 1000000);
            Console.WriteLine("Seed completed");
        }

        // Start to Benchmark
        BenchmarkRunner.Run<BenchmarkMethods>();
    }
}