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
public class EnumHasFlagComplexBenchmarks
{
    // | Method         | Job      | Runtime  | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | Rank | Allocated | Alloc Ratio |
    // |--------------- |--------- |--------- |----------:|----------:|----------:|----------:|------:|--------:|-----:|----------:|------------:|
    // | IsSetComplex   | .NET 6.0 | .NET 6.0 | 0.0859 ns | 0.0468 ns | 0.1359 ns | 0.0000 ns |     ? |       ? |    1 |         - |           ? |
    // | HasFlagComplex | .NET 6.0 | .NET 6.0 | 0.1377 ns | 0.1088 ns | 0.3069 ns | 0.0000 ns |     ? |       ? |    1 |         - |           ? |
    // |                |          |          |           |           |           |           |       |         |      |           |             |
    // | IsSetComplex   | .NET 7.0 | .NET 7.0 | 0.0989 ns | 0.0280 ns | 0.0757 ns | 0.0803 ns |     ? |       ? |    1 |         - |           ? |
    // | HasFlagComplex | .NET 7.0 | .NET 7.0 | 0.1474 ns | 0.0394 ns | 0.1136 ns | 0.1069 ns |     ? |       ? |    2 |         - |           ? |
    // |                |          |          |           |           |           |           |       |         |      |           |             |
    // | IsSetComplex   | .NET 8.0 | .NET 8.0 | 0.1497 ns | 0.0359 ns | 0.0491 ns | 0.1528 ns |  0.72 |    0.31 |    1 |         - |          NA |
    // | HasFlagComplex | .NET 8.0 | .NET 8.0 | 0.2367 ns | 0.0366 ns | 0.0660 ns | 0.2356 ns |  1.00 |    0.00 |    2 |         - |          NA |

    private NameRControlFlags flags;

    [GlobalSetup]
    public void GlobalSetup()
    {
        flags = NameRControlFlags.All;
    }

    [Benchmark(Baseline = true)]
    public bool HasFlagComplex()
    {
        if (flags.HasFlag(NameRControlFlags.IncludeHiddenStackFrames) && flags.HasFlag(NameRControlFlags.IncludeParamsKeyword))
        {
            return flags.HasFlag(NameRControlFlags.IncludeStaticModifier);
        }

        return flags.HasFlag(NameRControlFlags.IncludeAccessModifier);
    }

    [Benchmark]
    public bool IsSetComplex()
    {
        if (flags.IsSet(NameRControlFlags.IncludeHiddenStackFrames) && flags.IsSet(NameRControlFlags.IncludeParamsKeyword))
        {
            return flags.IsSet(NameRControlFlags.IncludeStaticModifier);
        }

        return flags.IsSet(NameRControlFlags.IncludeAccessModifier);
    }
}