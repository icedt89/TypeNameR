using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using System.Text;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.StringBuilder;

/// <summary>
/// This benchmark compares different methods of appending strings via <see cref="StringBuilder"/>.
/// </summary>
[MemoryDiagnoser]
// [InliningDiagnoser(logFailuresOnly: false, allowedNamespaces: ["JanHafner.TypeNameR.Benchmark.Benchmarks.StringBuilder", "System.Text"])]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
public class StringBuilderInliningAppendBenchmarks
{
    // | Method                      | Job      | Runtime  | Mean     | Error    | StdDev   | Median   | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
    // |---------------------------- |--------- |--------- |---------:|---------:|---------:|---------:|------:|--------:|-------:|-------:|----------:|------------:|
    // | AppendNonFluentString       | .NET 6.0 | .NET 6.0 | 647.9 ns | 12.83 ns | 11.37 ns | 649.2 ns |  0.96 |    0.04 | 0.8488 | 0.0162 |    5.2 KB |        1.00 |
    // | AppendFluentString          | .NET 6.0 | .NET 6.0 | 662.8 ns | 12.03 ns | 29.06 ns | 655.8 ns |  1.00 |    0.00 | 0.8488 | 0.0162 |    5.2 KB |        1.00 |
    // | AppendNonFluentReadOnlySpan | .NET 6.0 | .NET 6.0 | 697.3 ns | 13.88 ns | 32.17 ns | 690.0 ns |  1.05 |    0.07 | 0.8488 | 0.0162 |    5.2 KB |        1.00 |
    // | AppendFluentReadOnlySpan    | .NET 6.0 | .NET 6.0 | 699.6 ns | 13.98 ns | 28.25 ns | 693.0 ns |  1.05 |    0.06 | 0.8488 | 0.0162 |    5.2 KB |        1.00 |
    // |                             |          |          |          |          |          |          |       |         |        |        |           |             |
    // | AppendFluentString          | .NET 7.0 | .NET 7.0 | 613.7 ns | 12.31 ns | 21.23 ns | 611.3 ns |  1.00 |    0.00 | 0.8488 | 0.0162 |    5.2 KB |        1.00 |
    // | AppendFluentReadOnlySpan    | .NET 7.0 | .NET 7.0 | 666.6 ns | 23.29 ns | 67.95 ns | 655.4 ns |  1.12 |    0.10 | 0.8488 | 0.0162 |    5.2 KB |        1.00 |
    // | AppendNonFluentString       | .NET 7.0 | .NET 7.0 | 677.9 ns | 18.95 ns | 54.97 ns | 669.6 ns |  1.10 |    0.10 | 0.8488 | 0.0162 |    5.2 KB |        1.00 |
    // | AppendNonFluentReadOnlySpan | .NET 7.0 | .NET 7.0 | 729.4 ns | 25.30 ns | 74.61 ns | 710.7 ns |  1.21 |    0.12 | 0.8488 | 0.0162 |    5.2 KB |        1.00 |
    // |                             |          |          |          |          |          |          |       |         |        |        |           |             |
    // | AppendNonFluentString       | .NET 8.0 | .NET 8.0 | 506.6 ns |  9.39 ns | 14.05 ns | 505.5 ns |  0.87 |    0.05 | 0.8488 | 0.0162 |    5.2 KB |        1.00 |
    // | AppendFluentReadOnlySpan    | .NET 8.0 | .NET 8.0 | 541.0 ns | 10.58 ns | 25.15 ns | 534.9 ns |  0.97 |    0.10 | 0.8488 | 0.0162 |    5.2 KB |        1.00 |
    // | AppendFluentString          | .NET 8.0 | .NET 8.0 | 546.5 ns | 16.48 ns | 48.33 ns | 542.7 ns |  1.00 |    0.00 | 0.8488 | 0.0162 |    5.2 KB |        1.00 |
    // | AppendNonFluentReadOnlySpan | .NET 8.0 | .NET 8.0 | 565.4 ns | 12.17 ns | 34.14 ns | 552.4 ns |  1.04 |    0.09 | 0.8488 | 0.0162 |    5.2 KB |        1.00 |

    private const string @string = "StringBuilderInliningAppendBenchmarks";

    private static ReadOnlySpan<char> span => @string;

    [GlobalSetup]
    public void GlobalSetup()
    {
    }

    [Benchmark(Baseline = true)]
    public System.Text.StringBuilder AppendFluentString()
    {
        var stringBuilder = new System.Text.StringBuilder();

        for (var i = 0; i < 5; i++)
        {
            AppendFluentString(stringBuilder, @string);
        }

        return stringBuilder;
    }

    #region AppendFluent

    private void AppendFluentString(System.Text.StringBuilder stringBuilder, string value)
        => stringBuilder
            .Append(value)
            .Append(value)
            .Append(value)
            .Append(value)
            .Append(value)
            .Append(value)
            .Append(value)
            .Append(value)
            .Append(value)
            .Append(value);

    #endregion

    [Benchmark]
    public System.Text.StringBuilder AppendFluentReadOnlySpan()
    {
        var stringBuilder = new System.Text.StringBuilder();

        for (var i = 0; i < 5; i++)
        {
            AppendFluentReadOnlySpan(stringBuilder, span);
        }

        return stringBuilder;
    }

    #region AppendFluentReadOnlySpan

    private void AppendFluentReadOnlySpan(System.Text.StringBuilder stringBuilder, ReadOnlySpan<char> value)
        => stringBuilder
            .Append(value)
            .Append(value)
            .Append(value)
            .Append(value)
            .Append(value)
            .Append(value)
            .Append(value)
            .Append(value)
            .Append(value)
            .Append(value);

    #endregion

    [Benchmark]
    public System.Text.StringBuilder AppendNonFluentString()
    {
        var stringBuilder = new System.Text.StringBuilder();

        for (var i = 0; i < 5; i++)
        {
            AppendNonFluentString(stringBuilder, @string);
        }

        return stringBuilder;
    }

    #region AppendNonFluent

    private void AppendNonFluentString(System.Text.StringBuilder stringBuilder, string value)
    {
        stringBuilder.Append(value);
        stringBuilder.Append(value);
        stringBuilder.Append(value);
        stringBuilder.Append(value);
        stringBuilder.Append(value);
        stringBuilder.Append(value);
        stringBuilder.Append(value);
        stringBuilder.Append(value);
        stringBuilder.Append(value);
        stringBuilder.Append(value);
    }

    #endregion

    [Benchmark]
    public System.Text.StringBuilder AppendNonFluentReadOnlySpan()
    {
        var stringBuilder = new System.Text.StringBuilder();

        for (var i = 0; i < 5; i++)
        {
            AppendNonFluentReadOnlySpan(stringBuilder, span);
        }

        return stringBuilder;
    }

    #region AppendNonFluentReadOnlySpan

    private void AppendNonFluentReadOnlySpan(System.Text.StringBuilder stringBuilder, ReadOnlySpan<char> value)
    {
        stringBuilder.Append(value);
        stringBuilder.Append(value);
        stringBuilder.Append(value);
        stringBuilder.Append(value);
        stringBuilder.Append(value);
        stringBuilder.Append(value);
        stringBuilder.Append(value);
        stringBuilder.Append(value);
        stringBuilder.Append(value);
        stringBuilder.Append(value);
    }

    #endregion
}