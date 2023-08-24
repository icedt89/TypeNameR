using BenchmarkDotNet.Running;
using JanHafner.TypeNameR.Benchmark.Benchmarks;

namespace JanHafner.TypeNameR.Benchmark;

public static class Program
{
    public static void Main()
    {
        // BenchmarkRunner.Run<CommonBenchmarks>();
        BenchmarkRunner.Run<DisplayOfTypeBenchmarks>();
        // BenchmarkRunner.Run<DisplayOfMethodBenchmarks>();
        // BenchmarkRunner.Run<DisplayOfStackTraceBenchmarks>();
        // BenchmarkRunner.Run<IsGenericMethodVersusGetGenericArgumentsBenchmarks>();
        // BenchmarkRunner.Run<IsGenericTypeVersusGetGenericArgumentsBenchmarks>();

        // BenchmarkRunner.Run(new[] { typeof(TypeGenericsBenchmarks), typeof(MethodGenericsBenchmarks) });
    }   
}
