using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.Reflection;

/// <summary>
/// This benchmarks should prove that calling <see cref="Type" />.<see cref="Type.IsGenericType" /> to check for generic parameters first, brings performance improvements.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
public class TypeGenericsComplexBenchmarks
{
    // | Method            | Job      | Runtime  | Mean       | Error     | StdDev     | Median     | Ratio  | RatioSD | Rank | Gen0   | Allocated | Alloc Ratio |
    // |------------------ |--------- |--------- |-----------:|----------:|-----------:|-----------:|-------:|--------:|-----:|-------:|----------:|------------:|
    // | NonGenericType    | .NET 6.0 | .NET 6.0 |  2.8192 ns | 0.1569 ns |  0.4528 ns |  2.6944 ns |   1.00 |    0.00 |    1 |      - |         - |          NA |
    // | ClosedGenericType | .NET 6.0 | .NET 6.0 | 76.2250 ns | 1.5024 ns |  3.1360 ns | 76.1467 ns |  26.66 |    4.08 |    2 | 0.0076 |      48 B |          NA |
    // | OpenGenericType   | .NET 6.0 | .NET 6.0 | 82.5041 ns | 1.6911 ns |  2.9618 ns | 82.6468 ns |  28.81 |    4.51 |    3 | 0.0076 |      48 B |          NA |
    // |                   |          |          |            |           |            |            |        |         |      |        |           |             |
    // | NonGenericType    | .NET 7.0 | .NET 7.0 |  3.0642 ns | 0.1061 ns |  0.0829 ns |  3.0565 ns |   1.00 |    0.00 |    1 |      - |         - |          NA |
    // | ClosedGenericType | .NET 7.0 | .NET 7.0 | 83.4427 ns | 4.0678 ns | 11.6713 ns | 77.9169 ns |  25.82 |    2.40 |    2 | 0.0076 |      48 B |          NA |
    // | OpenGenericType   | .NET 7.0 | .NET 7.0 | 86.7790 ns | 1.7747 ns |  1.7430 ns | 86.9848 ns |  28.49 |    0.84 |    3 | 0.0076 |      48 B |          NA |
    // |                   |          |          |            |           |            |            |        |         |      |        |           |             |
    // | NonGenericType    | .NET 8.0 | .NET 8.0 |  0.8585 ns | 0.1831 ns |  0.5194 ns |  0.5973 ns |   1.00 |    0.00 |    1 |      - |         - |          NA |
    // | ClosedGenericType | .NET 8.0 | .NET 8.0 | 73.3851 ns | 1.3112 ns |  1.1624 ns | 73.3901 ns | 102.70 |   41.48 |    2 | 0.0076 |      48 B |          NA |
    // | OpenGenericType   | .NET 8.0 | .NET 8.0 | 82.9507 ns | 1.6923 ns |  1.9489 ns | 82.1881 ns | 125.28 |   44.21 |    3 | 0.0076 |      48 B |          NA |

    private Type closedGenericType;

    private Type openGenericType;

    private Type nonGenericType;

    [GlobalSetup]
    public void GlobalSetup()
    {
        closedGenericType = typeof(GenericTestClass<string[,,]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<KeyValuePair<string, double>>, object[,][]>);
        openGenericType = typeof(GenericTestClass<>.InnerNonGenericTestClass.MostInnerGenericTestClass<,>);
        nonGenericType = typeof(TestClass);
    }

    [Benchmark(Baseline = true)]
    public Type[] NonGenericType()
    {
        if (nonGenericType.IsGenericType)
        {
            return nonGenericType.GetGenericArguments();
        }

        return Array.Empty<Type>();
    }

    [Benchmark]
    public Type[] ClosedGenericType()
    {
        if (closedGenericType.IsGenericType)
        {
            return closedGenericType.GetGenericArguments();
        }

        return Array.Empty<Type>();
    }

    [Benchmark]
    public Type[] OpenGenericType()
    {
        if (openGenericType.IsGenericType)
        {
            return openGenericType.GetGenericArguments();
        }

        return Array.Empty<Type>();
    }
}