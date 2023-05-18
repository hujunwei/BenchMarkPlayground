``` ini

BenchmarkDotNet=v0.13.5, OS=macOS Ventura 13.2.1 (22D68) [Darwin 22.3.0]
Apple M1 Pro, 1 CPU, 10 logical and 10 physical cores
.NET SDK=6.0.408
  [Host]     : .NET 6.0.16 (6.0.1623.17311), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 6.0.16 (6.0.1623.17311), Arm64 RyuJIT AdvSIMD


```
|                       Method |       Mean |     Error |    StdDev |       Gen0 |      Gen1 |      Gen2 | Allocated |
|----------------------------- |-----------:|----------:|----------:|-----------:|----------:|----------:|----------:|
| StreamingWithSkipTakeInQuery | 4,008.2 ms | 112.78 ms | 318.11 ms | 38000.0000 |         - |         - |  74.44 MB |
|    StreamingWithAsEnumerable |   110.8 ms |   4.84 ms |  13.96 ms | 16666.6667 |         - |         - |  33.55 MB |
|          StreamingWithToList |   114.2 ms |   2.43 ms |   6.78 ms | 13200.0000 | 3600.0000 | 1200.0000 |  46.93 MB |
