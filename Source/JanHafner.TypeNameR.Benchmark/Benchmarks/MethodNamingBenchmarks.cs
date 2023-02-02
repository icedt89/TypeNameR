using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Reflection;

namespace JanHafner.TypeNameR.Benchmark;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
[SimpleJob(RuntimeMoniker.Net70)]
// [SimpleJob(RuntimeMoniker.Net80)] TODO
public class MethodNamingBenchmarks
{
    private MethodInfo simpleCallMethodInfo;

    private MethodInfo genericCallMethodInfo;

    private TypeNameR typeNameR;

    [GlobalSetup]
    public void GlobalSetup()
    {
        simpleCallMethodInfo = typeof(MethodsClass).GetMethod("Call1") ?? throw new InvalidOperationException();
        genericCallMethodInfo = typeof(MethodsClass).GetMethod("Get18") ?? throw new InvalidOperationException();

        typeNameR = new();
    }

    [Benchmark(Baseline = true)]
    public void SimpleCall_Name()
    {
        var name = simpleCallMethodInfo.Name;
    }

    [Benchmark]
    public void SimpleCall() => typeNameR.ExtractReadable(simpleCallMethodInfo, NameRControlFlags.All);

    [Benchmark]
    public void GenericCall() => typeNameR.ExtractReadable(genericCallMethodInfo, NameRControlFlags.All);

    [Benchmark]
    public void GenericCall_Name()
    {
        var name = genericCallMethodInfo.Name;
    }
}
