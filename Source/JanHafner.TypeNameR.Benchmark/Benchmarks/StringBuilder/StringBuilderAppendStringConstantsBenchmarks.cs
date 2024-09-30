using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using System.Text;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.StringBuilder;

/// <summary>
/// This benchmark compares different methods of appending constant like strings via <see cref="StringBuilder"/>.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
[RankColumn]
public class StringBuilderAppendStringConstantsBenchmarks
{
    // | Method                     | Job      | Runtime  | Mean      | Error    | StdDev    | Median    | Ratio | RatioSD | Rank | Gen0   | Gen1   | Allocated | Alloc Ratio |
    // |--------------------------- |--------- |--------- |----------:|---------:|----------:|----------:|------:|--------:|-----:|-------:|-------:|----------:|------------:|
    // | ReadOnlySpanEnsureCapacity | .NET 6.0 | .NET 6.0 |  83.47 ns | 1.744 ns |  3.054 ns |  83.03 ns |  0.48 |    0.07 |    1 | 0.0459 |      - |     288 B |        0.53 |
    // | StringEnsureCapacity       | .NET 6.0 | .NET 6.0 |  85.42 ns | 3.918 ns | 11.492 ns |  81.22 ns |  0.53 |    0.08 |    1 | 0.0459 |      - |     288 B |        0.53 |
    // | ReadOnlySpan               | .NET 6.0 | .NET 6.0 | 131.85 ns | 4.274 ns | 12.055 ns | 129.65 ns |  0.83 |    0.14 |    2 | 0.0865 |      - |     544 B |        1.00 |
    // | String                     | .NET 6.0 | .NET 6.0 | 163.04 ns | 7.667 ns | 22.243 ns | 153.59 ns |  1.00 |    0.00 |    3 | 0.0865 |      - |     544 B |        1.00 |
    // |                            |          |          |           |          |           |           |       |         |      |        |        |           |          
    //    |
    // | StringEnsureCapacity       | .NET 7.0 | .NET 7.0 |  71.31 ns | 1.418 ns |  1.517 ns |  70.95 ns |  0.64 |    0.02 |    1 | 0.0459 |      - |     288 B |        0.53 |
    // | ReadOnlySpanEnsureCapacity | .NET 7.0 | .NET 7.0 |  78.43 ns | 2.312 ns |  6.634 ns |  77.83 ns |  0.66 |    0.05 |    2 | 0.0459 |      - |     288 B |        0.53 |
    // | String                     | .NET 7.0 | .NET 7.0 | 118.08 ns | 2.944 ns |  8.302 ns | 115.07 ns |  1.00 |    0.00 |    3 | 0.0867 | 0.0001 |     544 B |        1.00 |
    // | ReadOnlySpan               | .NET 7.0 | .NET 7.0 | 118.28 ns | 3.100 ns |  8.945 ns | 116.76 ns |  1.01 |    0.09 |    3 | 0.0867 | 0.0001 |     544 B |        1.00 |
    // |                            |          |          |           |          |           |           |       |         |      |        |        |           |          
    //    |
    // | StringEnsureCapacity       | .NET 8.0 | .NET 8.0 |  41.32 ns | 1.092 ns |  3.115 ns |  39.96 ns |  0.51 |    0.04 |    1 | 0.0459 |      - |     288 B |        0.53 |
    // | ReadOnlySpanEnsureCapacity | .NET 8.0 | .NET 8.0 |  44.52 ns | 0.961 ns |  0.987 ns |  44.23 ns |  0.53 |    0.02 |    2 | 0.0459 |      - |     288 B |        0.53 |
    // | String                     | .NET 8.0 | .NET 8.0 |  84.95 ns | 1.667 ns |  2.281 ns |  85.33 ns |  1.00 |    0.00 |    3 | 0.0867 | 0.0001 |     544 B |        1.00 |
    // | ReadOnlySpan               | .NET 8.0 | .NET 8.0 |  92.37 ns | 2.846 ns |  8.256 ns |  91.51 ns |  1.17 |    0.09 |    4 | 0.0867 | 0.0001 |     544 B |        1.00 |

    private const string @string = "dynamic ";

    private static ReadOnlySpan<char> readOnlySpan => "dynamic ";

    [GlobalSetup]
    public void GlobalSetup()
    {
    }

    [Benchmark(Baseline = true)]
    public System.Text.StringBuilder String()
    {
        var stringBuilder = new System.Text.StringBuilder();

        for (var i = 0; i < 10; i++)
        {
            stringBuilder.Append(@string);
        }

        return stringBuilder;
    }

    [Benchmark]
    public System.Text.StringBuilder StringEnsureCapacity()
    {
        var stringBuilder = new System.Text.StringBuilder();

        stringBuilder.EnsureCapacity(@string.Length * 10);

        for (var i = 0; i < 10; i++)
        {
            stringBuilder.Append(@string);
        }

        return stringBuilder;
    }

    [Benchmark]
    public System.Text.StringBuilder ReadOnlySpan()
    {
        var stringBuilder = new System.Text.StringBuilder();

        for (var i = 0; i < 10; i++)
        {
            stringBuilder.Append(readOnlySpan);
        }

        return stringBuilder;
    }

    [Benchmark]
    public System.Text.StringBuilder ReadOnlySpanEnsureCapacity()
    {
        var stringBuilder = new System.Text.StringBuilder();

        stringBuilder.EnsureCapacity(readOnlySpan.Length * 10);

        for (var i = 0; i < 10; i++)
        {
            stringBuilder.Append(readOnlySpan);
        }

        return stringBuilder;
    }
}