using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using System.Reflection;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.Reflection;

/// <summary>
/// This benchmarks should prove that calling <see cref="MethodInfo" />.<see cref="MethodInfo.IsGenericMethod" /> to check for generic parameters first, brings performance improvements.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
public class MethodGenericsComplexBenchmarks
{
    // | Method           | Job      | Runtime  | Mean      | Error     | StdDev     | Median    | Ratio | RatioSD | Rank | Gen0   | Allocated | Alloc Ratio |
    // |----------------- |--------- |--------- |----------:|----------:|-----------:|----------:|------:|--------:|-----:|-------:|----------:|------------:|
    // | NonGenericMethod | .NET 6.0 | .NET 6.0 |  4.069 ns | 0.2250 ns |  0.6635 ns |  3.958 ns |  1.00 |    0.00 |    1 |      - |         - |          NA |
    // | GenericMethod    | .NET 6.0 | .NET 6.0 | 92.702 ns | 4.8053 ns | 14.1685 ns | 87.890 ns | 23.37 |    5.16 |    2 | 0.0063 |      40 B |          NA |
    // |                  |          |          |           |           |            |           |       |         |      |        |           |             |
    // | NonGenericMethod | .NET 7.0 | .NET 7.0 |  3.054 ns | 0.0955 ns |  0.0797 ns |  3.049 ns |  1.00 |    0.00 |    1 |      - |         - |          NA |
    // | GenericMethod    | .NET 7.0 | .NET 7.0 | 78.109 ns | 1.6000 ns |  1.7120 ns | 78.811 ns | 25.61 |    1.07 |    2 | 0.0063 |      40 B |          NA |
    // |                  |          |          |           |           |            |           |       |         |      |        |           |             |
    // | NonGenericMethod | .NET 8.0 | .NET 8.0 |  1.634 ns | 0.0421 ns |  0.0394 ns |  1.620 ns |  1.00 |    0.00 |    1 |      - |         - |          NA |
    // | GenericMethod    | .NET 8.0 | .NET 8.0 | 80.034 ns | 1.3056 ns |  1.2823 ns | 80.163 ns | 48.98 |    1.06 |    2 | 0.0063 |      40 B |          NA |

    private MethodInfo? nonGenericMethod;

    private MethodInfo? genericMethod;

    [GlobalSetup]
    public void GlobalSetup()
    {
        nonGenericMethod = typeof(TestClass).GetMethodOrThrow(nameof(TestClass.NonGenericMethod));
        genericMethod = typeof(TestClass).GetMethodOrThrow(nameof(TestClass.GenericMethod));
    }

    [Benchmark(Baseline = true)]
    public Type[] NonGenericMethod()
    {
        if (nonGenericMethod!.IsGenericMethod)
        {
            return nonGenericMethod.GetGenericArguments();
        }

        return Array.Empty<Type>();
    }

    [Benchmark]
    public Type[] GenericMethod()
    {

        if (genericMethod!.IsGenericMethod)
        {
            return genericMethod.GetGenericArguments();
        }

        return Array.Empty<Type>();
    }
}