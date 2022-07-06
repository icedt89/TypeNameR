using BenchmarkDotNet.Attributes;
using JanHafner.TypeNameR.StackTrace;
using System.Diagnostics;
using System.IO.Abstractions;

namespace JanHafner.TypeNameR.Benchmark;

[MemoryDiagnoser]
public class ExceptionBenchmarks
{
    private Exception catchedException;

    private TypeNameR typeNameR;

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        this.typeNameR = new(new StackFrameMetadataProvider(new PdbLocator(new FileSystem()), new FileSystem()));

        try
        {
            int? int1 = 0;
            await StackTraceGenerator.CallRecursivGenericMethod<int>(ref int1);
        }
        catch (Exception exception)
        {
            this.catchedException = exception;
        }
    }

    [Benchmark(Baseline = true)]
    public void RewriteExceptionStackTracesWithBen()
    {
        this.catchedException.Demystify();
    }

    [Benchmark]
    public void RewriteExceptionStackTraces()
    {
        this.typeNameR.RewriteStackTrace(catchedException, NameRControlFlags.All
                                                        & ~NameRControlFlags.IncludeHiddenStackFrames
                                                        & ~NameRControlFlags.IncludeAccessModifier
                                                        & ~NameRControlFlags.IncludeStaticModifier
                                                        & ~NameRControlFlags.IncludeParameterDefaultValue
                                                        & ~NameRControlFlags.StoreOriginalStackTraceInExceptionData);
    }
}