using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using JanHafner.TypeNameR.Helper;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.StackTraceHelper;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.Method)]
// [SimpleJob(RuntimeMoniker.Net60)]
// [SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
public class ProcessStackFramesBenchmarks
{
    private System.Diagnostics.StackTrace stackTrace = default!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        try
        {
            StackTraceGenerator.RecursiveCall(10);

            throw new InvalidOperationException("That should not happen");
        }
        catch (StackOverflowException stackOverflowException)
        {
            stackTrace = new System.Diagnostics.StackTrace(stackOverflowException, false);
        }
    }

    [Benchmark(Baseline = true)]
    public Array Default() => stackTrace.EnumerateRecursiveStackFrames(includeHiddenStackFrames: false).ToArray();
}