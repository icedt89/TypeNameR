using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

/// <summary>
/// This benchmark compares the cost of creating empty arrays in various ways.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
public class CreateEmptyArrayBenchmarks
{
    // | Method        | Job      | Runtime  | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | Rank | Gen0   | Allocated | Alloc Ratio |
    // |-------------- |--------- |--------- |----------:|----------:|----------:|----------:|------:|--------:|-----:|-------:|----------:|------------:|
    // | ArrayEmpty    | .NET 6.0 | .NET 6.0 | 0.3835 ns | 0.0722 ns | 0.2082 ns | 0.3238 ns |  0.18 |    0.10 |    1 |      - |         - |        0.00 |
    // | NewEmptyEmpty | .NET 6.0 | .NET 6.0 | 2.2686 ns | 0.1032 ns | 0.1342 ns | 2.2897 ns |  1.00 |    0.00 |    2 | 0.0038 |      24 B |        1.00 |
    // |               |          |          |           |           |           |           |       |         |      |        |           |             |
    // | ArrayEmpty    | .NET 7.0 | .NET 7.0 | 0.5488 ns | 0.0731 ns | 0.1710 ns | 0.5239 ns |  0.13 |    0.04 |    1 |      - |         - |        0.00 |
    // | NewEmptyEmpty | .NET 7.0 | .NET 7.0 | 4.3154 ns | 0.1973 ns | 0.5661 ns | 4.1496 ns |  1.00 |    0.00 |    2 | 0.0038 |      24 B |        1.00 |
    // |               |          |          |           |           |           |           |       |         |      |        |           |             |
    // | ArrayEmpty    | .NET 8.0 | .NET 8.0 | 0.0821 ns | 0.0515 ns | 0.1320 ns | 0.0000 ns |  0.02 |    0.03 |    1 |      - |         - |        0.00 |
    // | NewEmptyEmpty | .NET 8.0 | .NET 8.0 | 4.4108 ns | 0.2749 ns | 0.7753 ns | 4.1941 ns |  1.00 |    0.00 |    2 | 0.0038 |      24 B |        1.00 |

    [Benchmark(Baseline = true)]
    public string[] NewEmptyEmpty() => new string[0];

    [Benchmark]
    public string[] ArrayEmpty() => Array.Empty<string>();
}