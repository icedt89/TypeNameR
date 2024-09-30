using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.TypeHelper;

// [MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
public class IsGenericValueTupleBenchmarks
{
    // | Method                               | Job      | Runtime  | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | Rank |
    // |------------------------------------- |--------- |--------- |----------:|----------:|----------:|----------:|------:|--------:|-----:|
    // | FrameworkAgnosticIsGenericValueTuple | .NET 6.0 | .NET 6.0 | 17.534 ns | 0.5344 ns | 1.5672 ns | 17.029 ns |  1.00 |    0.14 |    1 |
    // | IsGenericValueTuple                  | .NET 6.0 | .NET 6.0 | 17.672 ns | 0.6901 ns | 1.9912 ns | 17.019 ns |  1.01 |    0.16 |    1 |
    // | IsGenericValueTuple1                 | .NET 6.0 | .NET 6.0 | 31.229 ns | 0.6501 ns | 1.4807 ns | 31.183 ns |  1.79 |    0.20 |    2 |
    // |                                      |          |          |           |           |           |           |       |         |      |
    // | IsGenericValueTuple                  | .NET 7.0 | .NET 7.0 |  8.290 ns | 0.1940 ns | 0.1620 ns |  8.335 ns |  1.00 |    0.03 |    1 |
    // | FrameworkAgnosticIsGenericValueTuple | .NET 7.0 | .NET 7.0 |  8.900 ns | 0.2467 ns | 0.6999 ns |  8.752 ns |  1.07 |    0.09 |    1 |
    // | IsGenericValueTuple1                 | .NET 7.0 | .NET 7.0 | 25.608 ns | 0.5099 ns | 0.5008 ns | 25.649 ns |  3.09 |    0.08 |    2 |
    // |                                      |          |          |           |           |           |           |       |         |      |
    // | IsGenericValueTuple1                 | .NET 8.0 | .NET 8.0 |  2.734 ns | 0.0829 ns | 0.1107 ns |  2.710 ns |  0.27 |    0.01 |    1 |
    // | FrameworkAgnosticIsGenericValueTuple | .NET 8.0 | .NET 8.0 |  2.873 ns | 0.0873 ns | 0.1195 ns |  2.893 ns |  0.28 |    0.01 |    1 |
    // | IsGenericValueTuple                  | .NET 8.0 | .NET 8.0 | 10.229 ns | 0.2305 ns | 0.3451 ns | 10.197 ns |  1.00 |    0.05 |    2 |
    // |                                      |          |          |           |           |           |           |       |         |      |
    // | FrameworkAgnosticIsGenericValueTuple | .NET 9.0 | .NET 9.0 |  2.272 ns | 0.0879 ns | 0.2535 ns |  2.187 ns |  0.23 |    0.03 |    1 |
    // | IsGenericValueTuple1                 | .NET 9.0 | .NET 9.0 |  2.515 ns | 0.0789 ns | 0.1157 ns |  2.527 ns |  0.26 |    0.01 |    2 |
    // | IsGenericValueTuple                  | .NET 9.0 | .NET 9.0 |  9.826 ns | 0.2266 ns | 0.3025 ns |  9.755 ns |  1.00 |    0.04 |    3 |
    private readonly Type type = typeof((string Name, int Age, (bool IsTrue, double Score), float? Payment));

    [Benchmark(Baseline = true)]
    public bool IsGenericValueTuple() => string.Equals(type.Namespace, Constants.SystemNamespaceName, StringComparison.Ordinal) && type.Name.StartsWith(Constants.GenericValueTupleName, StringComparison.Ordinal);

    [Benchmark]
    public bool IsGenericValueTuple1() => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ValueTuple<>);
    
    [Benchmark]
    public bool FrameworkAgnosticIsGenericValueTuple()
    {
#if NET8_0_OR_GREATER
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ValueTuple<>);
#else
        return string.Equals(type.Namespace, Constants.SystemNamespaceName, StringComparison.Ordinal) && type.Name.StartsWith(Constants.GenericValueTupleName, StringComparison.Ordinal);
#endif
    }
}