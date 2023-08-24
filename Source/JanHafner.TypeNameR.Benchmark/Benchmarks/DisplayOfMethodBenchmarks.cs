using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using System.Reflection;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Method)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
public class DisplayOfMethodBenchmarks
{
    private MethodInfo nonGenericMethod;

    private MethodInfo genericMethod;

    private MethodInfo withEveryParameterKeyword;

    private TypeNameR typeNameR;
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        nonGenericMethod = typeof(TestClass).GetMethodOrThrow(nameof(TestClass.NonGenericMethod));
        genericMethod = typeof(TestClass).GetMethodOrThrow(nameof(TestClass.GenericMethod));
        withEveryParameterKeyword = typeof(ExtensionMethodsClass).GetMethodOrThrow(nameof(ExtensionMethodsClass.MethodWithEveryParameterKeyword));

        typeNameR = new();
    }

    // [Benchmark]
    public void NonGenericMethod_Name()
    {
        var _ = nonGenericMethod.Name;
    }
    
    // [Benchmark]
    public void GenericMethod_Name()
    {
        var _ = genericMethod.Name;
    }
    
    // [Benchmark]
    public void WithEveryParameterKeyword_Name()
    {
        var _ = withEveryParameterKeyword.Name;
    }

    // [Benchmark]
    public void NonGenericMethod() => typeNameR.GenerateDisplay(nonGenericMethod, NameRControlFlags.All);
    
    // [Benchmark]
    public void GenericMethod() => typeNameR.GenerateDisplay(genericMethod, NameRControlFlags.All);
    
    [Benchmark(Baseline = true)]
    public void EveryParameterKeyword() => typeNameR.GenerateDisplay(withEveryParameterKeyword, NameRControlFlags.All);
}
