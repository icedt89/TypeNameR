using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using System.Diagnostics;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Method)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
public class DisplayOfExceptionBenchmarks
{
    private Exception catchedException;

    private ITypeNameR typeNameR;
    
    [GlobalSetup]
    public async Task GlobalSetup()
    {
        try
        {
            await StackTraceGenerator.CallRecursiveGenericMethodAsync<int>();
        }
        catch (Exception exception)
        {
            catchedException = exception;
        }

        typeNameR = GlobalBenchmarkSettings.TypeNameR ?? new TypeNameR();
    }

    // [Benchmark]
    public void Exception_ToString() => catchedException.ToString();

    // [Benchmark]
    public void Demystify() => catchedException.Demystify();

    [Benchmark]
    public void GenerateDisplay()
        => typeNameR.GenerateDisplay(catchedException, NameRControlFlags.All
                                                                            & ~NameRControlFlags.IncludeHiddenStackFrames
                                                                            & ~NameRControlFlags.IncludeAccessModifier
                                                                            & ~NameRControlFlags.IncludeStaticModifier
                                                                            & ~NameRControlFlags.IncludeParameterDefaultValue);
}