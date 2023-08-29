using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Method)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
public class CommonBenchmarks
{
    // [GlobalSetup]
    // public void GlobalSetup()
    // {
    // }
}