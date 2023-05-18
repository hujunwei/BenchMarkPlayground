using System.Linq.Expressions;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace BenchmarkPlayground;

[MemoryDiagnoser]
public class BenchmarkMethods
{
    private const int BATCH_SIZE = 10;

    private static Expression<Func<Product, bool>> dataFilter = p => p.Id < 100000;

    [Benchmark]
    public async Task StreamingWithSkipTakeInQuery()
    {
        await using var dbContext = new BpDbContext();
        var query = dbContext
            .Products
            .Where(dataFilter)
            .AsNoTracking();

        var total = await query.CountAsync();
        var batches = (int)Math.Ceiling((double)total / BATCH_SIZE);
        for (var i = 0; i < batches; i++)
        {
            var batch = await query
                .OrderBy(x => x.Id)
                .Skip(i * BATCH_SIZE)
                .Take(BATCH_SIZE)
                .ToListAsync();

            Console.WriteLine($"{nameof(StreamingWithSkipTakeInQuery)} batch {i} with cnt {batch.Count}");
        }
    }

    [Benchmark]
    public async Task StreamingWithKeySetPagination()
    {
        await using var dbContext = new BpDbContext();
        var query = dbContext
            .Products
            .Where(dataFilter)
            .OrderBy(x => x.Id)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var lastId = 0;
        while (lastId < totalCount)
        {
            var page = await query
                .Where(x => x.Id > lastId)
                .Take(BATCH_SIZE)
                .ToListAsync();

            lastId = (int)(page.LastOrDefault()?.Id ?? lastId);
            Console.WriteLine($"StreamingWithKeySetPagination batch with lastId: {lastId} and pageCount: {page.Count}");
        }
    }


    [Benchmark]
    public async Task StreamingWithAsEnumerable()
    {
        // Streaming client as batches to memory
        await using var dbContext = new BpDbContext();
        var products = dbContext
            .Products
            .AsNoTracking()
            .Where(dataFilter)
            .AsEnumerable()
            .Batch(BATCH_SIZE);

        var i = 0;
        foreach (var batch in products) {
            Console.WriteLine($"{nameof(StreamingWithAsEnumerable)} batch {i} with cnt {batch.Count()}");
            i++;
        }
    }

    [Benchmark]
    public async Task StreamingWithToList()
    {
        // Streaming client as batches to memory
        await using var dbContext = new BpDbContext();
        var products = await dbContext
            .Products
            .AsNoTracking()
            .Where(dataFilter)
            .ToListAsync();

        var i = 0;
        foreach (var batch in products.Batch(BATCH_SIZE)) {
            Console.WriteLine($"{nameof(StreamingWithToList)} batch {i} with cnt {batch.Count()}");
            i++;
        }
    }
}