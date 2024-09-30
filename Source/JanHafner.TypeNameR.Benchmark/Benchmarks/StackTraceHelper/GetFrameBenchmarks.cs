using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.StackTraceHelper;

[MemoryDiagnoser]
[RankColumn]
[HintColumn]
[Orderer(SummaryOrderPolicy.Method)]
// [SimpleJob(RuntimeMoniker.Net60)]
// [SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
public class GetFrameBenchmarks
{
#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_stackFrames")]
    private static extern ref StackFrame[]? GetStackFrames(System.Diagnostics.StackTrace stackTrace);
    
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_methodsToSkip")]
    private static extern ref int GetMethodsToSkip(System.Diagnostics.StackTrace stackTrace);
#endif

    private System.Diagnostics.StackTrace stackTrace = default!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        try
        {
            StackTraceGenerator.RecursiveCall(100);

            throw new InvalidOperationException("That should not happen");
        }
        catch (StackOverflowException stackOverflowException)
        {
            stackTrace = new System.Diagnostics.StackTrace(stackOverflowException, false);
        }
    }

    [Benchmark(Baseline = true)]
    public StackFrame? GetFrame()
    {
        StackFrame? stackFrame = null;
        for (var index = 0; index < stackTrace.FrameCount; index++)
        {
            stackFrame = stackTrace.GetFrame(index);
        }

        return stackFrame;
    }

    [Benchmark]
    public StackFrame? ForEach()
    {
        StackFrame? stackFrame = null;
        foreach (var frame in stackTrace.GetFrames())
        {
            stackFrame = frame;
        }

        return stackFrame;
    }

    [Benchmark]
    [Hint(GlobalBenchmarkSettings.UnsupportedMessage)]
    [Hint(null, RuntimeMoniker.Net80)]
    public StackFrame? GetFrameRef()
    {
#if NET8_0_OR_GREATER
        ref var stackFrames = ref GetStackFrames(stackTrace);
        ref var methodsToSkip = ref GetMethodsToSkip(stackTrace);
        
        StackFrame? stackFrame = null;
        for (var index = 0; index < stackFrames.Length; index++)
        {
            stackFrame = stackFrames[index + methodsToSkip];
        }

        return stackFrame;
#else
        return null;
#endif
    }

    [Benchmark]
    [Hint(GlobalBenchmarkSettings.UnsupportedMessage)]
    [Hint(null, RuntimeMoniker.Net80)]
    public StackFrame? ForEachRef()
    {
#if NET8_0_OR_GREATER
        ref var stackFrames = ref GetStackFrames(stackTrace);
        ref var methodsToSkip = ref GetMethodsToSkip(stackTrace);

        StackFrame? stackFrame = null;
        foreach (var frame in stackFrames.AsSpan().Slice(methodsToSkip, stackTrace.FrameCount - methodsToSkip))
        {
            stackFrame = frame;
        }   

        return stackFrame;
#else
        return null;
#endif
    }
}