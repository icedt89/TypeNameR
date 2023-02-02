using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace JanHafner.TypeNameR.Benchmark;

public class Program
{
    public static void Main(string[] args)
    {
        // BenchmarkRunner.Run<TypeNamingBenchmarks>();
        // BenchmarkRunner.Run<MethodNamingBenchmarks>();
        BenchmarkRunner.Run<ExceptionBenchmarks>();
    }   
}
