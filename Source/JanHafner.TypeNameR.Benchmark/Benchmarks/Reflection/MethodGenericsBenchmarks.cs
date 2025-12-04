using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using System.Reflection;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.Reflection;

/// <summary>
/// This benchmarks measures the performance of <see cref="MethodInfo" />.<see cref="MethodInfo.IsGenericMethod" /> and <see cref="MethodInfo" />.<see cref="MethodInfo.GetGenericArguments" />.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
public class MethodGenericsBenchmarks
{
    // | Method                              | Job      | Runtime  | Mean      | Error     | StdDev     | Median    | Ratio | RatioSD | Rank | Gen0   | Allocated | Alloc Ratio |
    // |------------------------------------ |--------- |--------- |----------:|----------:|-----------:|----------:|------:|--------:|-----:|-------:|----------:|------------:|
    // | NonGenericMethodIsGenericMethod     | .NET 6.0 | .NET 6.0 |  2.585 ns | 0.0794 ns |  0.1918 ns |  2.517 ns |  1.00 |    0.00 |    1 |      - |         - |          NA |
    // | GenericMethodIsGenericMethod        | .NET 6.0 | .NET 6.0 |  3.680 ns | 0.1837 ns |  0.5328 ns |  3.483 ns |  1.47 |    0.21 |    2 |      - |         - |          NA |
    // | NonGenericMethodGetGenericArguments | .NET 6.0 | .NET 6.0 | 12.793 ns | 0.1562 ns |  0.1305 ns | 12.812 ns |  4.89 |    0.32 |    3 |      - |         - |          NA |
    // | GenericMethodGetGenericArguments    | .NET 6.0 | .NET 6.0 | 79.156 ns | 2.7931 ns |  8.0139 ns | 75.743 ns | 31.53 |    4.31 |    4 | 0.0063 |      40 B |          NA |
    // |                                     |          |          |           |           |            |           |       |         |      |        |           |             |
    // | NonGenericMethodIsGenericMethod     | .NET 7.0 | .NET 7.0 |  1.430 ns | 0.0549 ns |  0.0539 ns |  1.412 ns |  1.00 |    0.00 |    1 |      - |         - |          NA |
    // | GenericMethodIsGenericMethod        | .NET 7.0 | .NET 7.0 |  3.515 ns | 0.1924 ns |  0.5674 ns |  3.427 ns |  2.73 |    0.42 |    2 |      - |         - |          NA |
    // | NonGenericMethodGetGenericArguments | .NET 7.0 | .NET 7.0 | 16.263 ns | 0.7108 ns |  2.0507 ns | 15.556 ns | 11.69 |    1.57 |    3 |      - |         - |          NA |
    // | GenericMethodGetGenericArguments    | .NET 7.0 | .NET 7.0 | 80.567 ns | 1.6288 ns |  2.4874 ns | 80.335 ns | 56.78 |    2.59 |    4 | 0.0063 |      40 B |          NA |
    // |                                     |          |          |           |           |            |           |       |         |      |        |           |             |
    // | NonGenericMethodIsGenericMethod     | .NET 8.0 | .NET 8.0 |  1.966 ns | 0.0655 ns |  0.0643 ns |  1.964 ns |  1.00 |    0.00 |    1 |      - |         - |          NA |
    // | GenericMethodIsGenericMethod        | .NET 8.0 | .NET 8.0 |  2.870 ns | 0.0991 ns |  0.2844 ns |  2.816 ns |  1.58 |    0.11 |    2 |      - |         - |          NA |
    // | NonGenericMethodGetGenericArguments | .NET 8.0 | .NET 8.0 | 15.649 ns | 0.9913 ns |  2.9228 ns | 14.141 ns |  7.10 |    0.53 |    3 |      - |         - |          NA |
    // | GenericMethodGetGenericArguments    | .NET 8.0 | .NET 8.0 | 94.676 ns | 5.0849 ns | 14.8329 ns | 90.034 ns | 43.00 |    5.27 |    4 | 0.0063 |      40 B |          NA |

    private MethodInfo? nonGenericMethod;

    private MethodInfo? genericMethod;

    [GlobalSetup]
    public void GlobalSetup()
    {
        nonGenericMethod = typeof(TestClass).GetMethodOrThrow(nameof(TestClass.NonGenericMethod));
        genericMethod = typeof(TestClass).GetMethodOrThrow(nameof(TestClass.GenericMethod));
    }

    [Benchmark(Baseline = true)]
    public bool NonGenericMethodIsGenericMethod() => nonGenericMethod!.IsGenericMethod;

    [Benchmark]
    public Type[] NonGenericMethodGetGenericArguments() => nonGenericMethod!.GetGenericArguments();

    [Benchmark]
    public bool GenericMethodIsGenericMethod() => genericMethod!.IsGenericMethod;

    [Benchmark]
    public Type[] GenericMethodGetGenericArguments() => genericMethod!.GetGenericArguments();
}