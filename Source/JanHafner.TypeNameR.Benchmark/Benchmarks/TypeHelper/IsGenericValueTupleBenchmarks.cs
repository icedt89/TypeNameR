using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.TypeHelper;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.Method)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
public class IsGenericValueTupleBenchmarks
{
    // | Method                    | Job      | Runtime  | Mean     | Error    | StdDev   | Ratio | RatioSD | Rank | Allocated | Alloc Ratio |
    // |-------------------------- |--------- |--------- |---------:|---------:|---------:|------:|--------:|-----:|----------:|------------:|
    // | FrameworkReflectionHelper | .NET 6.0 | .NET 6.0 | 18.14 ns | 0.364 ns | 0.545 ns |  0.82 |    0.03 |    1 |         - |          NA |
    // | IsGenericValueTuple       | .NET 6.0 | .NET 6.0 | 22.43 ns | 0.440 ns | 0.367 ns |  1.00 |    0.00 |    2 |         - |          NA |
    // |                           |          |          |          |          |          |       |         |      |           |             |
    // | FrameworkReflectionHelper | .NET 7.0 | .NET 7.0 | 15.35 ns | 0.297 ns | 0.277 ns |  1.38 |    0.04 |    2 |         - |          NA |
    // | IsGenericValueTuple       | .NET 7.0 | .NET 7.0 | 11.14 ns | 0.218 ns | 0.224 ns |  1.00 |    0.00 |    1 |         - |          NA |
    // |                           |          |          |          |          |          |       |         |      |           |             |
    // | FrameworkReflectionHelper | .NET 8.0 | .NET 8.0 | 17.76 ns | 0.380 ns | 0.695 ns |  1.70 |    0.09 |    2 |         - |          NA |
    // | IsGenericValueTuple       | .NET 8.0 | .NET 8.0 | 10.25 ns | 0.225 ns | 0.221 ns |  1.00 |    0.00 |    1 |         - |          NA |

    private readonly Type type = typeof((string Name, int Age));

    [Benchmark(Baseline = true)]
    public bool IsGenericValueTuple() => JanHafner.TypeNameR.Helper.TypeHelper.IsGenericValueTuple(type);

    [Benchmark]
    public bool FrameworkReflectionHelper() => System.Diagnostics.Internal.ReflectionHelper.IsValueTuple(type);
}