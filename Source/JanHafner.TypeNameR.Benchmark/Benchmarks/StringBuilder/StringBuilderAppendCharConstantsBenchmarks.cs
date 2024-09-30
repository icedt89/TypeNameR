using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using System.Text;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.StringBuilder;

/// <summary>
/// This benchmark compares different methods of appending constant like chars via <see cref="StringBuilder"/>.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
public class StringBuilderAppendCharConstantsBenchmarks
{
    // | Method     | Job      | Runtime  | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
    // |----------- |--------- |--------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
    // | AppendChar | .NET 6.0 | .NET 6.0 |  24.94 ns | 0.666 ns | 1.965 ns |  1.00 |    0.00 | 0.0166 |     104 B |        1.00 |
    // | AppendByte | .NET 6.0 | .NET 6.0 | 105.69 ns | 2.163 ns | 5.347 ns |  4.29 |    0.36 | 0.0381 |     240 B |        2.31 |
    // |            |          |          |           |          |          |       |         |        |           |             |
    // | AppendChar | .NET 7.0 | .NET 7.0 |  24.65 ns | 0.548 ns | 0.694 ns |  1.00 |    0.00 | 0.0166 |     104 B |        1.00 |
    // | AppendByte | .NET 7.0 | .NET 7.0 |  95.44 ns | 2.699 ns | 7.957 ns |  3.86 |    0.32 | 0.0381 |     240 B |        2.31 |
    // |            |          |          |           |          |          |       |         |        |           |             |
    // | AppendChar | .NET 8.0 | .NET 8.0 |  22.79 ns | 0.494 ns | 0.625 ns |  1.00 |    0.00 | 0.0166 |     104 B |        1.00 |
    // | AppendByte | .NET 8.0 | .NET 8.0 |  74.09 ns | 2.195 ns | 6.404 ns |  3.34 |    0.26 | 0.0331 |     208 B |        2.00 |

    private const char @char = '+';

    private const byte @byte = (byte)'+';

    [GlobalSetup]
    public void GlobalSetup()
    {
    }

    [Benchmark(Baseline = true)]
    public System.Text.StringBuilder AppendChar()
    {
        var stringBuilder = new System.Text.StringBuilder();

        for (var i = 0; i < 10; i++)
        {
            stringBuilder.Append(@char);
        }

        return stringBuilder;
    }

    [Benchmark]
    public System.Text.StringBuilder AppendByte()
    {
        var stringBuilder = new System.Text.StringBuilder();

        for (var i = 0; i < 10; i++)
        {
            stringBuilder.Append(@byte);
        }

        return stringBuilder;
    }
}