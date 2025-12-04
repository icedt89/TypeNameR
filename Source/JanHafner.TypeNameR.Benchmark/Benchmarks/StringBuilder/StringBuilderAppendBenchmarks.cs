using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using System.Text;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.StringBuilder;

/// <summary>
/// This benchmark compares different methods of appending strings via <see cref="StringBuilder"/>.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
public class StringBuilderAppendBenchmarks
{
    // | Method             | Job      | Runtime  | Mean     | Error    | StdDev   | Median   | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
    // |------------------- |--------- |--------- |---------:|---------:|---------:|---------:|------:|--------:|-------:|-------:|----------:|------------:|
    // | AppendReadOnlySpan | .NET 6.0 | .NET 6.0 | 246.0 ns |  4.99 ns | 13.16 ns | 244.0 ns |  0.79 |    0.07 | 0.4268 | 0.0043 |   2.62 KB |        1.00 |
    // | AppendString       | .NET 6.0 | .NET 6.0 | 312.4 ns |  8.89 ns | 25.79 ns | 302.5 ns |  1.00 |    0.00 | 0.4268 | 0.0043 |   2.62 KB |        1.00 |
    // |                    |          |          |          |          |          |          |       |         |        |        |           |             |
    // | AppendString       | .NET 7.0 | .NET 7.0 | 251.0 ns |  5.69 ns | 15.95 ns | 252.9 ns |  1.00 |    0.00 | 0.4270 | 0.0043 |   2.62 KB |        1.00 |
    // | AppendReadOnlySpan | .NET 7.0 | .NET 7.0 | 271.6 ns | 12.29 ns | 35.84 ns | 264.9 ns |  1.07 |    0.13 | 0.4268 | 0.0043 |   2.62 KB |        1.00 |
    // |                    |          |          |          |          |          |          |       |         |        |        |           |             |
    // | AppendString       | .NET 8.0 | .NET 8.0 | 232.2 ns |  5.87 ns | 16.94 ns | 226.1 ns |  1.00 |    0.00 | 0.4268 | 0.0043 |   2.62 KB |        1.00 |
    // | AppendReadOnlySpan | .NET 8.0 | .NET 8.0 | 246.4 ns |  8.24 ns | 23.52 ns | 243.1 ns |  1.07 |    0.12 | 0.4268 | 0.0043 |   2.62 KB |        1.00 |

    private string? @string;

    [GlobalSetup]
    public void GlobalSetup()
    {
        @string = "JanHafner.TypeNameR.Benchmark.Benchmarks.StringBuilderAppendBenchmarks";
    }

    [Benchmark(Baseline = true)]
    public System.Text.StringBuilder AppendString()
    {
        var stringBuilder = new System.Text.StringBuilder();

        for (var i = 0; i < 10; i++)
        {
            stringBuilder.Append(@string);
        }

        return stringBuilder;
    }

    [Benchmark]
    public System.Text.StringBuilder AppendReadOnlySpan()
    {
        var stringBuilder = new System.Text.StringBuilder();

        var span = @string.AsSpan();

        for (var i = 0; i < 10; i++)
        {
            stringBuilder.Append(span);
        }

        return stringBuilder;
    }
}