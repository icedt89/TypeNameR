using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
public class EnumHasFlagBenchmarks
{
    // | Method        | Job      | Runtime  | Mean     | Error    | StdDev   | Ratio | RatioSD | Rank | Allocated | Alloc Ratio |
    // |-------------- |--------- |--------- |---------:|---------:|---------:|------:|--------:|-----:|----------:|------------:|
    // | CachedHasFlag | .NET 6.0 | .NET 6.0 | 32.96 ns | 0.554 ns | 0.491 ns |  0.90 |    0.03 |    1 |         - |          NA |
    // | HasFlag       | .NET 6.0 | .NET 6.0 | 36.65 ns | 0.739 ns | 0.791 ns |  1.00 |    0.00 |    2 |         - |          NA |
    // |               |          |          |          |          |          |       |         |      |           |             |
    // | CachedHasFlag | .NET 7.0 | .NET 7.0 | 32.76 ns | 0.451 ns | 0.352 ns |  0.91 |    0.02 |    1 |         - |          NA |
    // | HasFlag       | .NET 7.0 | .NET 7.0 | 35.97 ns | 0.627 ns | 0.556 ns |  1.00 |    0.00 |    2 |         - |          NA |
    // |               |          |          |          |          |          |       |         |      |           |             |
    // | CachedHasFlag | .NET 8.0 | .NET 8.0 | 32.03 ns | 0.630 ns | 0.862 ns |  0.87 |    0.03 |    1 |         - |          NA |
    // | HasFlag       | .NET 8.0 | .NET 8.0 | 36.60 ns | 0.174 ns | 0.145 ns |  1.00 |    0.00 |    2 |         - |          NA |

    private readonly NameRControlFlags nameRControlFlags = NameRControlFlags.All;

    [Benchmark(Baseline = true)]
    public bool HasFlag()
    {
        var result = false;
        for (var i = 0; i < 100; i++)
        {
            if (HasFlagCore(NameRControlFlags.IncludeGenericParameters))
            {
                result = true;
            }
        }

        return result;
    }

    #region HasFlag

    private bool HasFlagCore(NameRControlFlags flag) => nameRControlFlags.HasFlag(flag);

    #endregion

    [Benchmark]
    public bool CachedHasFlag()
    {
        var hasFlag = HasFlagCore(NameRControlFlags.IncludeGenericParameters);

        var result = false;
        for (var i = 0; i < 100; i++)
        {
            if (hasFlag)
            {
                result = true;
            }
        }

        return result;
    }
}