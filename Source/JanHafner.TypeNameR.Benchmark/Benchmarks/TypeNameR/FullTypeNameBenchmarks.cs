using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.TypeNameR;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
public class FullTypeNameBenchmarks
{
    // | Method              | Job      | Runtime  | Mean       | Error    | StdDev   | Median     | Ratio | RatioSD | Rank | Gen0   | Allocated | Alloc Ratio |
    // |-------------------- |--------- |--------- |-----------:|---------:|---------:|-----------:|------:|--------:|-----:|-------:|----------:|------------:|
    // | NonGenericNonNested | .NET 6.0 | .NET 6.0 |   146.0 ns |  2.97 ns |  8.09 ns |   145.2 ns |  1.00 |    0.00 |    1 | 0.0825 |     520 B |        1.00 |
    // | NonGenericNested    | .NET 6.0 | .NET 6.0 |   312.5 ns |  8.10 ns | 22.58 ns |   304.2 ns |  2.15 |    0.20 |    2 | 0.1311 |     824 B |        1.58 |
    // | GenericNonNested    | .NET 6.0 | .NET 6.0 |   322.6 ns |  5.96 ns |  4.65 ns |   323.4 ns |  2.25 |    0.09 |    3 | 0.0916 |     576 B |        1.11 |
    // | GenericNested       | .NET 6.0 | .NET 6.0 | 1,656.2 ns | 32.74 ns | 32.15 ns | 1,664.7 ns | 11.38 |    0.50 |    4 | 0.2651 |    1672 B |        3.22 |
    // |                     |          |          |            |          |          |            |       |         |      |        |           |             |
    // | NonGenericNonNested | .NET 7.0 | .NET 7.0 |   126.8 ns |  2.58 ns |  4.02 ns |   125.6 ns |  1.00 |    0.00 |    1 | 0.0827 |     520 B |        1.00 |
    // | NonGenericNested    | .NET 7.0 | .NET 7.0 |   261.8 ns |  4.35 ns |  5.50 ns |   262.4 ns |  2.05 |    0.08 |    2 | 0.1311 |     824 B |        1.58 |
    // | GenericNonNested    | .NET 7.0 | .NET 7.0 |   329.7 ns | 11.09 ns | 31.83 ns |   318.8 ns |  2.62 |    0.30 |    3 | 0.0916 |     576 B |        1.11 |
    // | GenericNested       | .NET 7.0 | .NET 7.0 | 1,548.5 ns | 30.51 ns | 50.14 ns | 1,540.9 ns | 12.19 |    0.41 |    4 | 0.2651 |    1672 B |        3.22 |
    // |                     |          |          |            |          |          |            |       |         |      |        |           |             |
    // | NonGenericNonNested | .NET 8.0 | .NET 8.0 |   115.3 ns |  3.20 ns |  8.97 ns |   113.8 ns |  1.00 |    0.00 |    1 | 0.0827 |     520 B |        1.00 |
    // | NonGenericNested    | .NET 8.0 | .NET 8.0 |   225.3 ns |  5.84 ns | 16.28 ns |   219.7 ns |  1.97 |    0.23 |    2 | 0.1311 |     824 B |        1.58 |
    // | GenericNonNested    | .NET 8.0 | .NET 8.0 |   234.2 ns |  4.69 ns | 10.58 ns |   232.2 ns |  2.03 |    0.16 |    3 | 0.0916 |     576 B |        1.11 |
    // | GenericNested       | .NET 8.0 | .NET 8.0 | 1,292.4 ns | 25.49 ns | 44.65 ns | 1,284.0 ns | 11.20 |    0.92 |    4 | 0.2651 |    1672 B |        3.22 |

    private Type? nonGenericNonNestedType;

    private Type? nonGenericNestedType;

    private Type? genericNonNestedType;

    private Type? genericNestedType;

    private ITypeNameR? typeNameR;

    [GlobalSetup]
    public void GlobalSetup()
    {
        nonGenericNonNestedType = typeof(TestClass);
        nonGenericNestedType = typeof(TestClass.InnerTestClass.MostInnerTestClass);
        genericNonNestedType = typeof(GenericTestClass<int>);
        genericNestedType =
            typeof(GenericTestClass<string[,,]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<KeyValuePair<string, double>>, object[,][]>[]
                [,,]);

        typeNameR = GlobalBenchmarkSettings.TypeNameR ?? new JanHafner.TypeNameR.TypeNameR();
    }

    [Benchmark(Baseline = true)]
    public string NonGenericNonNested() => typeNameR!.GenerateDisplay(nonGenericNonNestedType!, true, NameRControlFlags.All);

    [Benchmark]
    public string NonGenericNested() => typeNameR!.GenerateDisplay(nonGenericNestedType!, true, NameRControlFlags.All);

    [Benchmark]
    public string GenericNonNested() => typeNameR!.GenerateDisplay(genericNonNestedType!, true, NameRControlFlags.All);

    [Benchmark]
    public string GenericNested() => typeNameR!.GenerateDisplay(genericNestedType!, true, NameRControlFlags.All);
}