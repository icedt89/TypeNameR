using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using JanHafner.TypeNameR.StackTrace;
using System.Diagnostics;
using System.IO.Abstractions;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
[SimpleJob(RuntimeMoniker.Net70)]
// [SimpleJob(RuntimeMoniker.Net80)] TODO
public class ExceptionBenchmarks
{
    private Exception catchedException;

    private TypeNameR typeNameR;
    
    [GlobalSetup]
    public async Task GlobalSetup()
    {
        typeNameR = new(new StackFrameMetadataProvider(new PdbLocator(new FileSystem()), new FileSystem()));

        try
        {
            int? int1 = 0;
            await StackTraceGenerator.CallRecursivGenericMethod<int>(ref int1);
        }
        catch (Exception exception)
        {
            catchedException = exception;
        }
    }

    [Benchmark(Baseline = true)]
    public void RewriteExceptionStackTracesWithBen() => catchedException.Demystify();

    [Benchmark]
    public void RewriteExceptionStackTraces()
        => typeNameR.RewriteStackTrace(catchedException, NameRControlFlags.All
                                                         & ~NameRControlFlags.IncludeHiddenStackFrames
                                                         & ~NameRControlFlags.IncludeAccessModifier
                                                         & ~NameRControlFlags.IncludeStaticModifier
                                                         & ~NameRControlFlags.IncludeParameterDefaultValue
                                                         & ~NameRControlFlags.StoreOriginalStackTraceInExceptionData);
}