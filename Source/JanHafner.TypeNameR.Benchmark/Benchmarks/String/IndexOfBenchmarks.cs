using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.String;

/// <summary>
/// This benchmark compares different methods of IndexOf overloads.
/// </summary>
// [MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
public class IndexOfBenchmarks
{
    // | Method | Job      | Runtime  | Mean     | Error    | StdDev   | Ratio | RatioSD | Rank |
    // |------- |--------- |--------- |---------:|---------:|---------:|------:|--------:|-----:|
    // | String | .NET 6.0 | .NET 6.0 | 51.10 ns | 1.048 ns | 1.076 ns |  1.00 |    0.00 |    1 |
    // | Span   | .NET 6.0 | .NET 6.0 | 55.70 ns | 1.004 ns | 1.886 ns |  1.11 |    0.05 |    2 |
    // |        |          |          |          |          |          |       |         |      |
    // | String | .NET 7.0 | .NET 7.0 | 33.99 ns | 0.702 ns | 1.248 ns |  1.00 |    0.00 |    1 |
    // | Span   | .NET 7.0 | .NET 7.0 | 38.47 ns | 0.782 ns | 0.989 ns |  1.12 |    0.05 |    2 |
    // |        |          |          |          |          |          |       |         |      |
    // | Span   | .NET 8.0 | .NET 8.0 | 38.40 ns | 0.782 ns | 1.070 ns |  0.98 |    0.03 |    1 |
    // | String | .NET 8.0 | .NET 8.0 | 39.09 ns | 0.295 ns | 0.262 ns |  1.00 |    0.00 |    1 |

    private string typeNameWithGenericsArity = string.Empty;

    private char genericsArityMarker;

    [GlobalSetup]
    public void GlobalSetup()
    {
        typeNameWithGenericsArity = "StartsWithBenchmarks`1";
        genericsArityMarker = Constants.GraveAccent;
    }

    [Benchmark(Baseline = true)]
    public int String()
    {
        var result = -1;
        for (var i = 0; i < 10; i++)
        {
            result = typeNameWithGenericsArity.IndexOf(genericsArityMarker);
        }

        return result;
    }

    [Benchmark]
    public int Span()
    {
        var result = -1;
        for (var i = 0; i < 10; i++)
        {
            result = typeNameWithGenericsArity.AsSpan().IndexOf(genericsArityMarker);
        }

        return result;
    }
}