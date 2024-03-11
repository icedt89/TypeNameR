using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.Helper;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

/// <summary>
/// This benchmark compares the cost of a custom enum HasFlag method, without the generic clutter of the framework method.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
public class EnumHasFlagBenchmarks
{
    // | Method    | Job      | Runtime  | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | Rank | Allocated | Alloc Ratio |
    // |---------- |--------- |--------- |----------:|----------:|----------:|----------:|------:|--------:|-----:|----------:|------------:|
    // | IsSetFast | .NET 6.0 | .NET 6.0 | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |     ? |       ? |    1 |         - |           ? |
    // | HasFlag   | .NET 6.0 | .NET 6.0 | 0.0908 ns | 0.0293 ns | 0.0718 ns | 0.0910 ns |     ? |       ? |    2 |         - |           ? |
    // |           |          |          |           |           |           |           |       |         |      |           |             |
    // | IsSetFast | .NET 7.0 | .NET 7.0 | 0.0120 ns | 0.0124 ns | 0.0354 ns | 0.0000 ns |     ? |       ? |    1 |         - |           ? |
    // | HasFlag   | .NET 7.0 | .NET 7.0 | 0.1314 ns | 0.0502 ns | 0.1479 ns | 0.0669 ns |     ? |       ? |    2 |         - |           ? |
    // |           |          |          |           |           |           |           |       |         |      |           |             |
    // | IsSetFast | .NET 8.0 | .NET 8.0 | 0.0190 ns | 0.0177 ns | 0.0513 ns | 0.0000 ns |     ? |       ? |    1 |         - |           ? |
    // | HasFlag   | .NET 8.0 | .NET 8.0 | 0.0514 ns | 0.0279 ns | 0.0822 ns | 0.0000 ns |     ? |       ? |    1 |         - |           ? |

    private NameRControlFlags flags;

    [GlobalSetup]
    public void GlobalSetup()
    {
        flags = NameRControlFlags.All;
    }

    [Benchmark(Baseline = true)]
    public bool HasFlag() => flags.HasFlag(NameRControlFlags.IncludeHiddenStackFrames);

    [Benchmark]
    public bool IsSetFast() => flags.IsSet(NameRControlFlags.IncludeHiddenStackFrames);
}