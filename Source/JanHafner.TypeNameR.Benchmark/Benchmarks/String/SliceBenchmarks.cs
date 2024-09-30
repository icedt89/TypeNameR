using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.String;

/// <summary>
/// This benchmark compares different methods of slicing parts from a <see cref="string"/> or <see cref="ReadOnlySpan{char}"/>.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
public class SliceBenchmarks
{
    // | Method     | Job      | Runtime  | Mean      | Error     | StdDev    | Median    | Ratio | Rank | Gen0   | Allocated | Alloc Ratio |
    // |----------- |--------- |--------- |----------:|----------:|----------:|----------:|------:|-----:|-------:|----------:|------------:|
    // | SpanRange  | .NET 6.0 | .NET 6.0 |  7.258 ns | 0.1694 ns | 0.1585 ns |  7.246 ns |  0.07 |    1 |      - |         - |        0.00 |
    // | SpanAsSpan | .NET 6.0 | .NET 6.0 |  7.960 ns | 0.1818 ns | 0.2364 ns |  7.944 ns |  0.08 |    2 |      - |         - |        0.00 |
    // | String     | .NET 6.0 | .NET 6.0 | 96.863 ns | 1.9885 ns | 4.0620 ns | 96.434 ns |  1.00 |    3 | 0.0892 |     560 B |        1.00 |
    // |            |          |          |           |           |           |           |       |      |        |           |             |
    // | SpanAsSpan | .NET 7.0 | .NET 7.0 |  4.600 ns | 0.1102 ns | 0.1716 ns |  4.579 ns |  0.05 |    1 |      - |         - |        0.00 |
    // | SpanRange  | .NET 7.0 | .NET 7.0 |  5.400 ns | 0.1406 ns | 0.3423 ns |  5.312 ns |  0.06 |    2 |      - |         - |        0.00 |
    // | String     | .NET 7.0 | .NET 7.0 | 93.350 ns | 1.8583 ns | 4.8299 ns | 92.113 ns |  1.00 |    3 | 0.0892 |     560 B |        1.00 |
    // |            |          |          |           |           |           |           |       |      |        |           |             |
    // | SpanAsSpan | .NET 8.0 | .NET 8.0 |  5.162 ns | 0.1332 ns | 0.2034 ns |  5.140 ns |  0.07 |    1 |      - |         - |        0.00 |
    // | SpanRange  | .NET 8.0 | .NET 8.0 |  7.689 ns | 0.1554 ns | 0.2763 ns |  7.634 ns |  0.11 |    2 |      - |         - |        0.00 |
    // | String     | .NET 8.0 | .NET 8.0 | 71.587 ns | 1.4980 ns | 4.1508 ns | 70.428 ns |  1.00 |    3 | 0.0892 |     560 B |        1.00 |

    private string typeNameWithGenericsArity = string.Empty;

    private int genericsArityMarkerIndex;

    [GlobalSetup]
    public void GlobalSetup()
    {
        typeNameWithGenericsArity = "SliceBenchmarks`1";
        genericsArityMarkerIndex = typeNameWithGenericsArity.IndexOf('`');
    }

    [Benchmark(Baseline = true)]
    public string String()
    {
        var result = string.Empty;
        for (var i = 0; i < 10; i++)
        {
            result = typeNameWithGenericsArity[..genericsArityMarkerIndex];
        }

        return result;
    }

    [Benchmark]
    public ReadOnlySpan<char> SpanRange()
    {
        var result = ReadOnlySpan<char>.Empty;
        for (var i = 0; i < 10; i++)
        {
            result = typeNameWithGenericsArity.AsSpan()[..genericsArityMarkerIndex];
        }

        return result;
    }

    [Benchmark]
    public ReadOnlySpan<char> SpanAsSpan()
    {
        var result = ReadOnlySpan<char>.Empty;
        for (var i = 0; i < 10; i++)
        {
            result = typeNameWithGenericsArity.AsSpan(0, genericsArityMarkerIndex);
        }

        return result;
    }
}