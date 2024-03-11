using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using System.Diagnostics;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.TypeNameR;

[MemoryDiagnoser]
[RankColumn]
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

        typeNameR = GlobalBenchmarkSettings.TypeNameR ?? new JanHafner.TypeNameR.TypeNameR();
    }

    // [Benchmark]
    public string Exception_ToString() => catchedException.ToString();

    // [Benchmark]
    public string Demystify() => catchedException.Demystify().ToString();

    [Benchmark]
    public string GenerateDisplay()
        => typeNameR.GenerateDisplay(catchedException, NameRControlFlags.All
                                                                            & ~NameRControlFlags.IncludeHiddenStackFrames
                                                                            & ~NameRControlFlags.IncludeAccessModifier
                                                                            & ~NameRControlFlags.IncludeStaticModifier
                                                                            & ~NameRControlFlags.IncludeParameterDefaultValue);
}