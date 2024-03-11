using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using System.Reflection;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

/// <summary>
/// This benchmark compares the performance of default bool comparison and pattern matching.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
public class PatternMatchingBenchmarks
{
    // | Method          | Job      | Runtime  | Mean     | Error     | StdDev    | Ratio | RatioSD | Rank | Allocated | Alloc Ratio |
    // |---------------- |--------- |--------- |---------:|----------:|----------:|------:|--------:|-----:|----------:|------------:|
    // | Default         | .NET 6.0 | .NET 6.0 | 3.302 ns | 0.0884 ns | 0.0946 ns |  1.00 |    0.00 |    1 |         - |          NA |
    // | PatternMatching | .NET 6.0 | .NET 6.0 | 3.426 ns | 0.0778 ns | 0.0956 ns |  1.04 |    0.04 |    2 |         - |          NA |
    // |                 |          |          |          |           |           |       |         |      |           |             |
    // | Default         | .NET 7.0 | .NET 7.0 | 3.073 ns | 0.0690 ns | 0.0646 ns |  1.00 |    0.00 |    1 |         - |          NA |
    // | PatternMatching | .NET 7.0 | .NET 7.0 | 3.222 ns | 0.0935 ns | 0.1563 ns |  1.04 |    0.05 |    2 |         - |          NA |
    // |                 |          |          |          |           |           |       |         |      |           |             |
    // | PatternMatching | .NET 8.0 | .NET 8.0 | 2.119 ns | 0.0949 ns | 0.1981 ns |  0.82 |    0.14 |    1 |         - |          NA |
    // | Default         | .NET 8.0 | .NET 8.0 | 2.637 ns | 0.1342 ns | 0.3695 ns |  1.00 |    0.00 |    2 |         - |          NA |

    private ParameterInfo parameter;

    [GlobalSetup]
    public void GlobalSetup()
    {
        parameter = typeof(ExtensionMethodsClass).GetParameter(nameof(ExtensionMethodsClass.This), 0);
    }

    [Benchmark(Baseline = true)]
    public bool Default() => !parameter.IsIn && !parameter.ParameterType.IsByRef;

    [Benchmark]
    public bool PatternMatching() => parameter is { IsIn: false, ParameterType.IsByRef: false };
}