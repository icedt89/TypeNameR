using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.Reflection;

/// <summary>
/// This benchmarks the performance of <see cref="Type" />.<see cref="Type.IsGenericType" /> and <see cref="Type" />.<see cref="Type.GetGenericArguments" />.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
public class TypeGenericsBenchmarks
{
    // | Method                               | Job      | Runtime  | Mean       | Error     | StdDev     | Median     | Ratio  | RatioSD | Rank | Gen0   | Allocated | Alloc Ratio |
    // |------------------------------------- |--------- |--------- |-----------:|----------:|-----------:|-----------:|-------:|--------:|-----:|-------:|----------:|------------:|
    // | OpenGenericTypeIsGenericType         | .NET 6.0 | .NET 6.0 |  1.2099 ns | 0.0339 ns |  0.0265 ns |  1.2069 ns |   0.71 |    0.05 |    1 |      - |         - |          NA |
    // | ClosedGenericTypeIsGenericType       | .NET 6.0 | .NET 6.0 |  1.3815 ns | 0.0617 ns |  0.1739 ns |  1.3330 ns |   0.79 |    0.13 |    2 |      - |         - |          NA |
    // | NonGenericTypeIsGenericType          | .NET 6.0 | .NET 6.0 |  1.7379 ns | 0.0618 ns |  0.1276 ns |  1.7242 ns |   1.00 |    0.00 |    3 |      - |         - |          NA |
    // | NonGenericTypeGetGenericArguments    | .NET 6.0 | .NET 6.0 | 15.9583 ns | 0.7154 ns |  2.0869 ns | 15.0855 ns |   9.34 |    1.41 |    4 |      - |         - |          NA |
    // | ClosedGenericTypeGetGenericArguments | .NET 6.0 | .NET 6.0 | 73.6055 ns | 1.5334 ns |  1.7658 ns | 73.5024 ns |  44.36 |    3.03 |    5 | 0.0076 |      48 B |          NA |
    // | OpenGenericTypeGetGenericArguments   | .NET 6.0 | .NET 6.0 | 79.1018 ns | 1.4341 ns |  2.2747 ns | 79.4119 ns |  46.39 |    3.69 |    6 | 0.0076 |      48 B |          NA |
    // |                                      |          |          |            |           |            |            |        |         |      |        |           |             |
    // | NonGenericTypeIsGenericType          | .NET 7.0 | .NET 7.0 |  1.2988 ns | 0.0205 ns |  0.0160 ns |  1.2993 ns |   1.00 |    0.00 |    1 |      - |         - |          NA |
    // | ClosedGenericTypeIsGenericType       | .NET 7.0 | .NET 7.0 |  1.3751 ns | 0.0797 ns |  0.2337 ns |  1.2853 ns |   1.19 |    0.19 |    1 |      - |         - |          NA |
    // | OpenGenericTypeIsGenericType         | .NET 7.0 | .NET 7.0 |  1.7969 ns | 0.1650 ns |  0.4864 ns |  1.8152 ns |   1.14 |    0.33 |    2 |      - |         - |          NA |
    // | NonGenericTypeGetGenericArguments    | .NET 7.0 | .NET 7.0 | 14.0919 ns | 0.6343 ns |  1.8301 ns | 13.2498 ns |  11.28 |    1.77 |    3 |      - |         - |          NA |
    // | ClosedGenericTypeGetGenericArguments | .NET 7.0 | .NET 7.0 | 83.8944 ns | 2.4477 ns |  7.0623 ns | 83.4489 ns |  66.63 |    5.15 |    4 | 0.0076 |      48 B |          NA |
    // | OpenGenericTypeGetGenericArguments   | .NET 7.0 | .NET 7.0 | 98.2776 ns | 4.4359 ns | 12.7985 ns | 95.1923 ns |  71.04 |    6.27 |    5 | 0.0076 |      48 B |          NA |
    // |                                      |          |          |            |           |            |            |        |         |      |        |           |             |
    // | NonGenericTypeIsGenericType          | .NET 8.0 | .NET 8.0 |  0.1731 ns | 0.0441 ns |  0.0490 ns |  0.1567 ns |   1.00 |    0.00 |    1 |      - |         - |          NA |
    // | ClosedGenericTypeIsGenericType       | .NET 8.0 | .NET 8.0 |  0.6464 ns | 0.0441 ns |  0.0472 ns |  0.6503 ns |   4.09 |    1.20 |    2 |      - |         - |          NA |
    // | OpenGenericTypeIsGenericType         | .NET 8.0 | .NET 8.0 |  0.9730 ns | 0.1060 ns |  0.3109 ns |  0.8483 ns |   5.60 |    2.12 |    3 |      - |         - |          NA |
    // | NonGenericTypeGetGenericArguments    | .NET 8.0 | .NET 8.0 | 14.7100 ns | 0.3506 ns |  0.8665 ns | 14.5415 ns |  91.63 |   30.87 |    4 |      - |         - |          NA |
    // | ClosedGenericTypeGetGenericArguments | .NET 8.0 | .NET 8.0 | 76.5516 ns | 1.5607 ns |  3.9155 ns | 76.2559 ns | 484.94 |  147.30 |    5 | 0.0076 |      48 B |          NA |
    // | OpenGenericTypeGetGenericArguments   | .NET 8.0 | .NET 8.0 | 90.7158 ns | 3.7177 ns | 10.8448 ns | 86.4668 ns | 589.79 |  183.76 |    6 | 0.0076 |      48 B |          NA |

    private Type nonGenericType;

    private Type openGenericType;

    private Type closedGenericType;

    [GlobalSetup]
    public void GlobalSetup()
    {
        nonGenericType = typeof(TestClass);
        openGenericType = typeof(GenericTestClass<>.InnerNonGenericTestClass.MostInnerGenericTestClass<,>);
        closedGenericType = typeof(GenericTestClass<string[,,]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<KeyValuePair<string, double>>, object[,][]>);
    }

    [Benchmark(Baseline = true)]
    public bool NonGenericTypeIsGenericType() => nonGenericType.IsGenericType;

    [Benchmark]
    public Type[] NonGenericTypeGetGenericArguments() => nonGenericType.GetGenericArguments();

    [Benchmark]
    public bool OpenGenericTypeIsGenericType() => openGenericType.IsGenericType;

    [Benchmark]
    public Type[] OpenGenericTypeGetGenericArguments() => openGenericType.GetGenericArguments();

    [Benchmark]
    public bool ClosedGenericTypeIsGenericType() => closedGenericType.IsGenericType;

    [Benchmark]
    public Type[] ClosedGenericTypeGetGenericArguments() => closedGenericType.GetGenericArguments();
}