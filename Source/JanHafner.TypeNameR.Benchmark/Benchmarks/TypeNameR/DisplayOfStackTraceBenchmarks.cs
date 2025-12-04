using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using JanHafner.TypeNameR.StackTrace;
using System.Diagnostics;
using System.IO.Abstractions;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.TypeNameR;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.Method)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
public class DisplayOfStackTraceBenchmarks
{
    private Exception? catchedException;

    private ITypeNameR? typeNameR;

    private JanHafner.TypeNameR.TypeNameR? typeNameRWithStackFrameMetadataProvider;

    private System.Diagnostics.StackTrace? stackTraceWithSourceInformation;

    private System.Diagnostics.StackTrace? stackTraceWithoutSourceInformation;

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
            stackTraceWithSourceInformation = new System.Diagnostics.StackTrace(exception, true);
            stackTraceWithoutSourceInformation = new System.Diagnostics.StackTrace(exception, false);
        }

        typeNameR = GlobalBenchmarkSettings.TypeNameR ?? new JanHafner.TypeNameR.TypeNameR();
        typeNameRWithStackFrameMetadataProvider = new(new StackFrameMetadataProvider(new PdbLocator(new FileSystem())));
    }

    // [Benchmark]
    public void StackTrace_ToString() => stackTraceWithSourceInformation!.ToString();

    // [Benchmark]
    public void Demystify() => catchedException!.Demystify();

    [Benchmark]
    public void WithSourceInformation()
        => typeNameR!.GenerateDisplay(stackTraceWithSourceInformation!, NameRControlFlags.All
                                                                            & ~NameRControlFlags.IncludeHiddenStackFrames
                                                                            & ~NameRControlFlags.IncludeAccessModifier
                                                                            & ~NameRControlFlags.IncludeStaticModifier
                                                                            & ~NameRControlFlags.IncludeParameterDefaultValue);

    // [Benchmark]
    public void WithoutSourceInformation()
        => typeNameR!.GenerateDisplay(stackTraceWithoutSourceInformation!, NameRControlFlags.All
                                                                               & ~NameRControlFlags.IncludeHiddenStackFrames
                                                                               & ~NameRControlFlags.IncludeAccessModifier
                                                                               & ~NameRControlFlags.IncludeStaticModifier
                                                                               & ~NameRControlFlags.IncludeParameterDefaultValue);

    // [Benchmark]
    public void WithoutSourceInformationUsingSFMP()
        => typeNameRWithStackFrameMetadataProvider!.GenerateDisplay(stackTraceWithoutSourceInformation!, NameRControlFlags.All
            & ~NameRControlFlags.IncludeHiddenStackFrames
            & ~NameRControlFlags.IncludeAccessModifier
            & ~NameRControlFlags.IncludeStaticModifier
            & ~NameRControlFlags.IncludeParameterDefaultValue);
}