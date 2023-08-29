using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

/// <summary>
/// This benchmarks should prove that calling <see cref="Type" />.<see cref="Type.IsGenericType" /> to check for generic parameters first, brings performance improvements.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Method)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
// [SimpleJob(RuntimeMoniker.Net80)]
public class IsGenericTypeVersusGetGenericArgumentsBenchmarks
{
    private Type closedGenericType;
    
    private Type openGenericType;

    private Type nonGenericType;

    [GlobalSetup]
    public void GlobalSetup()
    {
        closedGenericType = typeof(GenericTestClass<string[,,]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<KeyValuePair<string, double>>, object[,][]>);
        openGenericType = typeof(GenericTestClass<>.InnerNonGenericTestClass.MostInnerGenericTestClass<,>);
        nonGenericType = typeof(TestClass);
    }
    
    [Benchmark]
    public void GetGenericArguments_NonGenericType() => nonGenericType.GetGenericArguments();
    
    [Benchmark]
    public void IsGenericType_NonGenericType()
    {
        var _ = nonGenericType.IsGenericType;
    }
    
    [Benchmark]
    public void GetGenericArguments_ClosedGenericType() => closedGenericType.GetGenericArguments();
    
    [Benchmark]
    public void IsGenericType_ClosedGenericType()
    {
        var _ = closedGenericType.IsGenericType;
    }
    
    [Benchmark]
    public void GetGenericArguments_OpenGenericType() => openGenericType.GetGenericArguments();
    
    [Benchmark]
    public void IsGenericType_OpenGenericType()
    {
        var _ = openGenericType.IsGenericType;
    }
}