using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;

using ActualTypeNameR = JanHafner.TypeNameR.TypeNameR;
using ExperimentalTypeNameR = JanHafner.TypeNameR.Experimental.TypeNameR;

namespace JanHafner.TypeNameR.Benchmark.Experimental;

/// <summary>
/// This benchmark compares the actual default implementation of TypeNameR to others.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class ExperimentalBenchmarks
{
    // | Method       | Job      | Runtime  | Mean     | Error    | StdDev   | Median   | Ratio | RatioSD | Rank | Gen0   | Gen1   | Allocated | Alloc Ratio |   
    // |------------- |--------- |--------- |---------:|---------:|---------:|---------:|------:|--------:|-----:|-------:|-------:|----------:|------------:|   
    // | Default      | .NET 6.0 | .NET 6.0 | 76.12 us | 2.846 us | 8.347 us | 72.25 us |  1.01 |    0.15 |    1 | 1.9531 |      - |  12.59 KB |        1.00 |   
    // | Experimental | .NET 6.0 | .NET 6.0 | 78.02 us | 2.838 us | 8.368 us | 76.38 us |  1.04 |    0.15 |    1 | 1.9531 |      - |  12.82 KB |        1.02 |   
    // |              |          |          |          |          |          |          |       |         |      |        |        |           |             |
    // | Experimental | .NET 7.0 | .NET 7.0 | 28.02 us | 0.553 us | 0.591 us | 27.99 us |  0.96 |    0.09 |    1 | 3.3569 | 0.0610 |   20.6 KB |        1.01 |   
    // | Default      | .NET 7.0 | .NET 7.0 | 29.48 us | 1.040 us | 3.034 us | 28.04 us |  1.01 |    0.14 |    1 | 3.2959 | 0.0916 |  20.37 KB |        1.00 |   
    // |              |          |          |          |          |          |          |       |         |      |        |        |           |             |   
    // | Default      | .NET 8.0 | .NET 8.0 | 25.65 us | 0.988 us | 2.819 us | 24.74 us |  1.01 |    0.15 |    1 | 3.2959 | 0.0610 |  20.23 KB |        1.00 |   
    // | Experimental | .NET 8.0 | .NET 8.0 | 28.10 us | 0.638 us | 1.790 us | 28.50 us |  1.11 |    0.13 |    2 | 3.2959 |      - |  20.42 KB |        1.01 |
    private Exception? catchedException;

    private readonly Type type =
        typeof(GenericTestStruct<int>.InnerGenericTestStruct<bool[]>.MoreInnerGenericTestStruct<string, char>.MoreMoreInnerGenericTestStruct.
            MostInnerGenericTestStruct<uint>);

    private ActualTypeNameR? actualTypeNameR;

    private ExperimentalTypeNameR? experimentalTypeNameR;

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        try
        {
            await StackTraceGenerator.CallRecursiveGenericMethodAsync<int>();
        }
        catch (Exception exception)
        {
            catchedException = exception;
        }

        actualTypeNameR = new ActualTypeNameR();
        experimentalTypeNameR = new ExperimentalTypeNameR();
    }

    [Benchmark(Baseline = true)]
    public string Default()
        => actualTypeNameR!.GenerateDisplay(type, fullTypeName: true, JanHafner.TypeNameR.NameRControlFlags.All);

    [Benchmark]
    public string Experimental()
        => experimentalTypeNameR!.GenerateDisplay(type, fullTypeName: true, JanHafner.TypeNameR.Experimental.NameRControlFlags.All);

    // [Benchmark(Baseline = true)]
    // public string Default()
    //     => actualTypeNameR.GenerateDisplay(
    //         catchedException,
    //         NameRControlFlags.All
    //         | NameRControlFlags.DontEliminateRecursiveStackFrames);
    //
    // [Benchmark]
    // public string Experimental()
    //     => experimentalTypeNameR.GenerateDisplay(
    //         catchedException,
    //         JanHafner.TypeNameR.Experimental.NameRControlFlags.All
    //         | JanHafner.TypeNameR.Experimental.NameRControlFlags.DontEliminateRecursiveStackFrames);

    // [Benchmark(Baseline = true)]
    // public string Default()
    //     => actualTypeNameR.GenerateDisplay(
    //         catchedException,
    //         NameRControlFlags.All);
    //
    // [Benchmark]
    // public string Experimental()
    //     => experimentalTypeNameR.GenerateDisplay(
    //         catchedException,
    //         JanHafner.TypeNameR.Experimental.NameRControlFlags.All);
}