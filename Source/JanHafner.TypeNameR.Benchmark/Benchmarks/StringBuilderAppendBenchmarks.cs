using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using System.Text;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

/// <summary>
/// This benchmark compares different methods of appending strings via <see cref="StringBuilder"/>.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
public class StringBuilderAppendBenchmarks
{
    // | Method                              | Job      | Runtime  | Mean     | Error   | StdDev   | Median   | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
    // |------------------------------------ |--------- |--------- |---------:|--------:|---------:|---------:|------:|--------:|-------:|-------:|----------:|------------:|
    // | AppendReadOnlySpanScopedRefReadOnly | .NET 6.0 | .NET 6.0 | 230.3 ns | 2.15 ns |  1.80 ns | 230.4 ns |  0.95 |    0.02 | 0.6037 | 0.0083 |    3.7 KB |        1.00 |
    // | AppendReadOnlySpan                  | .NET 6.0 | .NET 6.0 | 231.1 ns | 3.18 ns |  2.65 ns | 230.4 ns |  0.95 |    0.03 | 0.6037 | 0.0083 |    3.7 KB |        1.00 |
    // | AppendReadOnlySpanIn                | .NET 6.0 | .NET 6.0 | 233.0 ns | 4.71 ns |  5.60 ns | 232.3 ns |  0.96 |    0.03 | 0.6037 | 0.0081 |    3.7 KB |        1.00 |
    // | AppendString                        | .NET 6.0 | .NET 6.0 | 242.1 ns | 4.81 ns |  5.15 ns | 240.6 ns |  1.00 |    0.00 | 0.6037 | 0.0081 |    3.7 KB |        1.00 |
    // |                                     |          |          |          |         |          |          |       |         |        |        |           |             |
    // | AppendReadOnlySpanScopedRefReadOnly | .NET 7.0 | .NET 7.0 | 232.3 ns | 4.21 ns |  3.73 ns | 232.3 ns |  0.99 |    0.03 | 0.6037 | 0.0086 |    3.7 KB |        1.00 |
    // | AppendReadOnlySpanIn                | .NET 7.0 | .NET 7.0 | 233.0 ns | 4.12 ns |  3.44 ns | 232.5 ns |  0.99 |    0.02 | 0.6037 | 0.0086 |    3.7 KB |        1.00 |
    // | AppendString                        | .NET 7.0 | .NET 7.0 | 235.4 ns | 4.70 ns |  4.40 ns | 235.0 ns |  1.00 |    0.00 | 0.6037 | 0.0086 |    3.7 KB |        1.00 |
    // | AppendReadOnlySpan                  | .NET 7.0 | .NET 7.0 | 237.8 ns | 4.65 ns |  6.21 ns | 236.5 ns |  1.01 |    0.04 | 0.6037 | 0.0086 |    3.7 KB |        1.00 |
    // |                                     |          |          |          |         |          |          |       |         |        |        |           |             |
    // | AppendString                        | .NET 8.0 | .NET 8.0 | 233.3 ns | 3.54 ns |  3.48 ns | 233.4 ns |  1.00 |    0.00 | 0.6037 | 0.0086 |    3.7 KB |        1.00 |
    // | AppendReadOnlySpanIn                | .NET 8.0 | .NET 8.0 | 234.9 ns | 3.58 ns |  3.17 ns | 234.5 ns |  1.01 |    0.02 | 0.6037 | 0.0086 |    3.7 KB |        1.00 |
    // | AppendReadOnlySpanScopedRefReadOnly | .NET 8.0 | .NET 8.0 | 237.9 ns | 4.30 ns |  4.02 ns | 236.0 ns |  1.02 |    0.02 | 0.6037 | 0.0086 |    3.7 KB |        1.00 |
    // | AppendReadOnlySpan                  | .NET 8.0 | .NET 8.0 | 240.8 ns | 4.61 ns | 10.86 ns | 236.6 ns |  1.05 |    0.06 | 0.6037 | 0.0086 |    3.7 KB |        1.00 |

    private string largeString;

    [GlobalSetup]
    public void GlobalSetup()
    {
        largeString = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.   \n\nDuis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat.   \n\nUt wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi.   \n\nNam liber tempor cum soluta nobis eleifend option congue nihil imperdiet doming id quod mazim placerat facer";
    }

    [Benchmark(Baseline = true)]
    public StringBuilder AppendString()
    {
        var stringBuilder = new StringBuilder();

        StringLevel1(stringBuilder, largeString);

        return stringBuilder;
    }

    #region AppendString

    private void StringLevel1(StringBuilder stringBuilder, string value) => StringLevel2(stringBuilder, value);

    private void StringLevel2(StringBuilder stringBuilder, string value) => stringBuilder.Append(value);

    #endregion

    [Benchmark]
    public StringBuilder AppendReadOnlySpan()
    {
        var stringBuilder = new StringBuilder();

        ReadOnlySpanLevel1(stringBuilder, largeString);

        return stringBuilder;
    }

    #region AppendReadOnlySpan

    private void ReadOnlySpanLevel1(StringBuilder stringBuilder, ReadOnlySpan<char> value) => ReadOnlySpanLevel2(stringBuilder, value);

    private void ReadOnlySpanLevel2(StringBuilder stringBuilder, ReadOnlySpan<char> value) => stringBuilder.Append(value);

    #endregion

    [Benchmark]
    public StringBuilder AppendReadOnlySpanIn()
    {
        var stringBuilder = new StringBuilder();

        var valueSpan = largeString.AsSpan();

        ReadOnlySpanLevel1In(stringBuilder, in valueSpan);

        return stringBuilder;
    }

    #region AppendReadOnlySpanIn

    private void ReadOnlySpanLevel1In(StringBuilder stringBuilder, in ReadOnlySpan<char> value) => ReadOnlySpanLevel2In(stringBuilder, in value);

    private void ReadOnlySpanLevel2In(StringBuilder stringBuilder, in ReadOnlySpan<char> value) => stringBuilder.Append(value);

    #endregion

    [Benchmark]
    public StringBuilder AppendReadOnlySpanScopedRefReadOnly()
    {
        var stringBuilder = new StringBuilder();

        var valueSpan = largeString.AsSpan();

        ReadOnlySpanLevel1ScopedRefReadOnly(stringBuilder, in valueSpan);

        return stringBuilder;
    }

    #region AppendReadOnlySpanScopedRefReadOnly

    private void ReadOnlySpanLevel1ScopedRefReadOnly(StringBuilder stringBuilder, scoped ref readonly ReadOnlySpan<char> value) => ReadOnlySpanLevel2ScopedRefReadOnly(stringBuilder, in value);

    private void ReadOnlySpanLevel2ScopedRefReadOnly(StringBuilder stringBuilder, scoped ref readonly ReadOnlySpan<char> value) => stringBuilder.Append(value);

    #endregion
}