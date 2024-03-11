using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

/// <summary>
/// This benchmark compares the cost of iterating over an array and a HashSet
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[HintColumn]
public class HashSetArrayEnumerationBenchmarks
{
    // | Method        | Job      | Runtime  | Mean        | Error     | StdDev     | Ratio  | RatioSD | Rank | Hint        | Gen0   | Allocated | Alloc Ratio |
    // |-------------- |--------- |--------- |------------:|----------:|-----------:|-------:|--------:|-----:|------------ |-------:|----------:|------------:|
    // | FrozenHashSet | .NET 6.0 | .NET 6.0 |   0.1279 ns | 0.0542 ns |  0.0507 ns |   0.08 |    0.04 |    1 | Unsupported |      - |         - |          NA |
    // | Array         | .NET 6.0 | .NET 6.0 |   1.7090 ns | 0.1881 ns |  0.5305 ns |   1.00 |    0.00 |    2 |             |      - |         - |          NA |
    // | HashSet       | .NET 6.0 | .NET 6.0 | 126.3311 ns | 7.6705 ns | 22.6167 ns |  82.07 |   29.51 |    3 |             | 0.0062 |      40 B |          NA |
    // |               |          |          |             |           |            |        |         |      |             |        |           |             |
    // | FrozenHashSet | .NET 7.0 | .NET 7.0 |   0.0000 ns | 0.0000 ns |  0.0000 ns |  0.000 |    0.00 |    1 | Unsupported |      - |         - |          NA |
    // | Array         | .NET 7.0 | .NET 7.0 |   3.0521 ns | 0.1158 ns |  0.2418 ns |  1.000 |    0.00 |    2 |             |      - |         - |          NA |
    // | HashSet       | .NET 7.0 | .NET 7.0 | 107.7233 ns | 2.2004 ns |  2.9374 ns | 35.600 |    2.92 |    3 |             | 0.0063 |      40 B |          NA |
    // |               |          |          |             |           |            |        |         |      |             |        |           |             |
    // | Array         | .NET 8.0 | .NET 8.0 |   2.5714 ns | 0.1106 ns |  0.1908 ns |   1.00 |    0.00 |    1 |             |      - |         - |          NA |
    // | FrozenHashSet | .NET 8.0 | .NET 8.0 |  23.6530 ns | 0.5331 ns |  0.4452 ns |   9.58 |    0.66 |    2 |             | 0.0051 |      32 B |          NA |
    // | HashSet       | .NET 8.0 | .NET 8.0 |  34.8794 ns | 0.7863 ns |  2.2306 ns |  13.89 |    1.79 |    3 |             | 0.0063 |      40 B |          NA |

    private string[] array;

    private IReadOnlySet<string> hashSet;

#if NET8_0_OR_GREATER
    
    private IReadOnlySet<string> frozenHashSet;
#endif

    [GlobalSetup]
    public void GlobalSetup()
    {
        array = [
            "18433145230404097895",
            "23353763701852247644",
            "30821250177234757691",
            "29522125251199595802",
            "84971302463224599570",
            "49597527583937234443",
            "11823751844658536385",
            "35077959465081958555",
            "59256893075762118766",
            "71002689560589648707"
        ];
        hashSet = array.ToHashSet();
#if NET8_0_OR_GREATER
        frozenHashSet = hashSet.ToFrozenSet();
#endif
    }

    [Benchmark(Baseline = true)]
    public string Array()
    {
        var result = string.Empty;
        foreach (var item in array)
        {
            result = item;
        }

        return result;
    }

    [Benchmark]
    public string HashSet()
    {
        var result = string.Empty;
        foreach (var item in hashSet)
        {
            result = item;
        }

        return result;
    }

    [Benchmark]
    [Hint(GlobalBenchmarkSettings.UnsupportedMessage)]
    [Hint(null, RuntimeMoniker.Net80)]
    public string FrozenHashSet()
    {
#if NET8_0_OR_GREATER
        var result = string.Empty;
        foreach (var item in frozenHashSet)
        {
            result = item;
        }

        return result;
#else
        return default!;
#endif
    }
}