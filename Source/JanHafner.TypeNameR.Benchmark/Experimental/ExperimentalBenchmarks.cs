using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;

namespace JanHafner.TypeNameR.Benchmark.Experimental;

/// <summary>
/// This benchmark compares the actual default implementation of TypeNameR to others.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
// [SimpleJob(RuntimeMoniker.Net60)]
// [SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
public class ExperimentalBenchmarks
{
    private Exception catchedException;

    private ITypeNameR typeNameR;

    // private TypeNameR.TypeNameR experimentalTypeNameR;

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
        // experimentalTypeNameR = new TypeNameR.TypeNameR();
    }

    [Benchmark(Baseline = true)]
    public string Default()
        => typeNameR.GenerateDisplay(catchedException, NameRControlFlags.All);

    // [Benchmark]
    // public string Experimental()
    //     => experimentalTypeNameR.GenerateDisplay(catchedException, TypeNameR.NameRControlFlags.All);
}