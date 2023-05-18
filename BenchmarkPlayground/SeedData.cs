namespace BenchmarkPlayground;

public static class SeedData
{
    public static void Run(BpDbContext dbContext, int count)
    {
        for (var i = 1; i <= count; i++)
        {
            var product = new Product();
            var stringProps = typeof(Product).GetProperties().Where(p => p.PropertyType == typeof(string));
            foreach (var field in stringProps)
            {
                field.SetValue(product, $"{field.Name}_{i}_Value");
            }

            dbContext.Products.Add(product);

            // Save once per 1000
            if (i % 1000 == 0)
            {
                dbContext.SaveChanges();
                Console.WriteLine($"Seeded {i}");
            }
        }

        dbContext.SaveChanges();
    }
}