using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.StackTrace;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.Reflection;

/// <summary>
/// This benchmark compares the cost of setting a non public field in various ways.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
[HintColumn]
public class SetExceptionStackTraceStringFieldBenchmarks
{
    // | Method         | Job      | Runtime  | Mean       | Error     | StdDev    | Median     | Ratio | Rank | Hint        | Allocated | Alloc Ratio |
    // |--------------- |--------- |--------- |-----------:|----------:|----------:|-----------:|------:|-----:|------------ |----------:|------------:|
    // | UnsafeAccessor | .NET 6.0 | .NET 6.0 |  0.0071 ns | 0.0093 ns | 0.0087 ns |  0.0047 ns | 0.000 |    1 | Unsupported |         - |          NA |
    // | Reflection     | .NET 6.0 | .NET 6.0 | 34.2017 ns | 0.4130 ns | 0.3863 ns | 34.1720 ns | 1.000 |    2 |             |         - |          NA |
    // |                |          |          |            |           |           |            |       |      |             |           |             |
    // | UnsafeAccessor | .NET 7.0 | .NET 7.0 |  0.0004 ns | 0.0016 ns | 0.0015 ns |  0.0000 ns | 0.000 |    1 | Unsupported |         - |          NA |
    // | Reflection     | .NET 7.0 | .NET 7.0 | 30.8673 ns | 0.2497 ns | 0.2335 ns | 30.8927 ns | 1.000 |    2 |             |         - |          NA |
    // |                |          |          |            |           |           |            |       |      |             |           |             |
    // | UnsafeAccessor | .NET 8.0 | .NET 8.0 |  1.7484 ns | 0.0065 ns | 0.0060 ns |  1.7505 ns |  0.06 |    1 |             |         - |          NA |
    // | Reflection     | .NET 8.0 | .NET 8.0 | 29.3968 ns | 0.4328 ns | 0.4049 ns | 29.4945 ns |  1.00 |    2 |             |         - |          NA |

    private Exception? exception;

    private string? stackTrace;

    [GlobalSetup]
    public void GlobalSetup()
    {
        exception = new Exception();
        stackTrace = Guid.NewGuid().ToString();
    }

    [Benchmark(Baseline = true)]
    public void Reflection() => ReflectionExceptionStackTraceSetter.SetValue(exception!, stackTrace!);

    [Benchmark]
    [Hint(GlobalBenchmarkSettings.UnsupportedMessage)]
    [Hint(null, RuntimeMoniker.Net80)]
    public void UnsafeAccessor()
    {
#if NET8_0_OR_GREATER
        UnsafeExceptionStackTraceSetter.SetValue(exception!, stackTrace!);
#endif
    }
}