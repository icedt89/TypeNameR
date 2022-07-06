using BenchmarkDotNet.Attributes;
using System.Reflection;

namespace JanHafner.TypeNameR.Benchmark;

[MemoryDiagnoser]
public class MethodNamingBenchmarks
{
    private MethodInfo simpleCallMethodInfo;

    private MethodInfo genericCallMethodInfo;

    private TypeNameR typeNameR;

    [GlobalSetup]
    public void GlobalSetup()
    {
        this.simpleCallMethodInfo = typeof(MethodsClass).GetMethod("Call1") ?? throw new InvalidOperationException();
        this.genericCallMethodInfo = typeof(MethodsClass).GetMethod("Get18") ?? throw new InvalidOperationException();

        this.typeNameR = new();
    }

    [Benchmark(Baseline = true)]
    public void SimpleCall_Name()
    {
        var name = this.simpleCallMethodInfo.Name;
    }

    [Benchmark]
    public void SimpleCall()
    {
        this.typeNameR.ExtractReadable(this.simpleCallMethodInfo, NameRControlFlags.All);
    }

    [Benchmark]
    public void GenericCall()
    {
        this.typeNameR.ExtractReadable(this.genericCallMethodInfo, NameRControlFlags.All);
    }

    [Benchmark]
    public void GenericCall_Name()
    {
        var name = this.genericCallMethodInfo.Name;
    }
}
