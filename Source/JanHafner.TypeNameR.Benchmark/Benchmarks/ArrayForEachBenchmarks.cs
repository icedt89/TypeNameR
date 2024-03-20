using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

/// <summary>
/// This benchmark compares the cost of iterating over arrays in different ways.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
public class ArrayForEachBenchmarks
{
    // | Method              | Job      | Runtime  | Mean     | Error    | StdDev    | Median   | Ratio | RatioSD | Rank | Allocated | Alloc Ratio |
    // |-------------------- |--------- |--------- |---------:|---------:|----------:|---------:|------:|--------:|-----:|----------:|------------:|
    // | ForEachLoopVariable | .NET 6.0 | .NET 6.0 | 66.80 ns | 1.378 ns |  2.226 ns | 66.96 ns |  0.84 |    0.08 |    1 |         - |          NA |
    // | ForIndexer          | .NET 6.0 | .NET 6.0 | 84.36 ns | 3.958 ns | 11.357 ns | 81.79 ns |  1.00 |    0.00 |    2 |         - |          NA |
    // |                     |          |          |          |          |           |          |       |         |      |           |             |
    // | ForEachLoopVariable | .NET 7.0 | .NET 7.0 | 65.51 ns | 1.374 ns |  1.218 ns | 65.30 ns |  0.79 |    0.05 |    1 |         - |          NA |
    // | ForIndexer          | .NET 7.0 | .NET 7.0 | 81.64 ns | 2.815 ns |  8.077 ns | 79.04 ns |  1.00 |    0.00 |    2 |         - |          NA |
    // |                     |          |          |          |          |           |          |       |         |      |           |             |
    // | ForEachLoopVariable | .NET 8.0 | .NET 8.0 | 78.20 ns | 3.892 ns | 11.352 ns | 77.94 ns |  0.96 |    0.16 |    1 |         - |          NA |
    // | ForIndexer          | .NET 8.0 | .NET 8.0 | 82.21 ns | 2.977 ns |  8.685 ns | 80.83 ns |  1.00 |    0.00 |    2 |         - |          NA |

    private string[] array;

    [GlobalSetup]
    public void GlobalSetup()
    {
        array = Enumerable.Range(0, 100).Select(_ => Guid.NewGuid().ToString()).ToArray();
    }

    [Benchmark(Baseline = true)]
    public (string?, bool) ForIndexer()
    {
        string? result = null;
        var isNotLast = true;
        for (var i = 0; i < array.Length; i++)
        {
            result = array[i];

            if (i < array.Length - 1)
            {
                isNotLast = true;
            }
        }

        return (result, isNotLast);
    }

    [Benchmark]
    public (string?, bool) ForEachLoopVariable()
    {
        string? result = null;
        var isNotLast = true;
        var i = 0;
        foreach (var t in array)
        {
            result = t;
            i += 1;

            if (i < array.Length - 1)
            {
                isNotLast = true;
            }
        }

        return (result, isNotLast);
    }
}