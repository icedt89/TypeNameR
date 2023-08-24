using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using System.Reflection;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

/// <summary>
/// This benchmarks should prove that calling <see cref="MethodInfo.IsGenericMethod" /> to check for generic parameters first, brings performance improvements.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Method)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
public class IsGenericMethodVersusGetGenericArgumentsBenchmarks
{
    private MethodInfo nonGenericMethod;
    
    private MethodInfo genericMethod;

    [GlobalSetup]
    public void GlobalSetup()
    {
        nonGenericMethod = typeof(TestClass).GetMethodOrThrow(nameof(TestClass.NonGenericMethod));
        genericMethod = typeof(TestClass).GetMethodOrThrow(nameof(TestClass.GenericMethod));
    }

    [Benchmark]
    public void IsGenericMethod_NonGenericMethod()
    {
        var _ = nonGenericMethod.IsGenericMethod;
    }
    
    [Benchmark]
    public void GetGenericArguments_NonGenericMethod() => nonGenericMethod.GetGenericArguments();
    
    [Benchmark]
    public void IsGenericMethod_GenericMethod()
    {
        var _ = genericMethod.IsGenericMethod;
    }

    [Benchmark]
    public void GetGenericArguments_GenericMethod() => genericMethod.GetGenericArguments();
}