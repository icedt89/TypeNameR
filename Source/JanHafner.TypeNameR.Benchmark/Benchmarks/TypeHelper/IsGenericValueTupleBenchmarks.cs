using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.TypeHelper;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
public class IsGenericValueTupleBenchmarks
{
    // | Method                               | Job      | Runtime  | Mean      | Error     | StdDev    | Ratio | RatioSD | Rank | Allocated | Alloc Ratio |
    // |------------------------------------- |--------- |--------- |----------:|----------:|----------:|------:|--------:|-----:|----------:|------------:|     
    // | FrameworkAgnosticIsGenericValueTuple | .NET 6.0 | .NET 6.0 | 14.286 ns | 0.0677 ns | 0.0634 ns |  0.95 |    0.01 |    1 |         - |          NA |     
    // | IsGenericValueTupleString            | .NET 6.0 | .NET 6.0 | 15.040 ns | 0.1792 ns | 0.1588 ns |  1.00 |    0.01 |    2 |         - |          NA |     
    // | IsGenericValueTupleReflection        | .NET 6.0 | .NET 6.0 | 21.481 ns | 0.2897 ns | 0.2710 ns |  1.43 |    0.02 |    3 |         - |          NA |     
    // |                                      |          |          |           |           |           |       |         |      |           |             |     
    // | FrameworkAgnosticIsGenericValueTuple | .NET 7.0 | .NET 7.0 |  7.174 ns | 0.0470 ns | 0.0439 ns |  1.00 |    0.01 |    1 |         - |          NA |     
    // | IsGenericValueTupleString            | .NET 7.0 | .NET 7.0 |  7.187 ns | 0.0458 ns | 0.0406 ns |  1.00 |    0.01 |    1 |         - |          NA |     
    // | IsGenericValueTupleReflection        | .NET 7.0 | .NET 7.0 | 10.357 ns | 0.2395 ns | 0.3197 ns |  1.44 |    0.04 |    2 |         - |          NA |
    // |                                      |          |          |           |           |           |       |         |      |           |             |     
    // | FrameworkAgnosticIsGenericValueTuple | .NET 8.0 | .NET 8.0 |  8.670 ns | 0.0661 ns | 0.0552 ns |  0.98 |    0.01 |    1 |         - |          NA |     
    // | IsGenericValueTupleString            | .NET 8.0 | .NET 8.0 |  8.846 ns | 0.0383 ns | 0.0358 ns |  1.00 |    0.01 |    1 |         - |          NA |     
    // | IsGenericValueTupleReflection        | .NET 8.0 | .NET 8.0 |  8.857 ns | 0.0469 ns | 0.0366 ns |  1.00 |    0.01 |    1 |         - |          NA |     
    // |                                      |          |          |           |           |           |       |         |      |           |             |     
    // | IsGenericValueTupleString            | .NET 9.0 | .NET 9.0 |  8.607 ns | 0.0107 ns | 0.0095 ns |  1.00 |    0.00 |    1 |         - |          NA |     
    // | IsGenericValueTupleReflection        | .NET 9.0 | .NET 9.0 |  8.750 ns | 0.1841 ns | 0.1632 ns |  1.02 |    0.02 |    1 |         - |          NA |     
    // | FrameworkAgnosticIsGenericValueTuple | .NET 9.0 | .NET 9.0 |  8.929 ns | 0.0311 ns | 0.0275 ns |  1.04 |    0.00 |    1 |         - |          NA |
    private readonly Type type = typeof((string Name, int Age, (bool IsTrue, double Score), float? Payment));

    [Benchmark(Baseline = true)]
    public bool IsGenericValueTupleString() => string.Equals(type.Namespace, Constants.SystemNamespaceName, StringComparison.Ordinal) && type.Name.StartsWith(Constants.GenericValueTupleName, StringComparison.Ordinal);
    
    [Benchmark]
    public bool IsGenericValueTupleReflection() => type.IsGenericType && type.IsAssignableTo(typeof(ITuple)) && type.IsValueType;
    
    [Benchmark]
    public bool FrameworkAgnosticIsGenericValueTuple()
    {
#if NET8_0_OR_GREATER
        return type.IsGenericType && type.IsAssignableTo(typeof(ITuple)) && type.IsValueType;
#else
        return string.Equals(type.Namespace, Constants.SystemNamespaceName, StringComparison.Ordinal) && type.Name.StartsWith(Constants.GenericValueTupleName, StringComparison.Ordinal);
#endif
    }
}