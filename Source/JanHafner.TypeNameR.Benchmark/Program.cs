using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using JanHafner.TypeNameR.Benchmark.Benchmarks;
using Perfolizer.Horology;

namespace JanHafner.TypeNameR.Benchmark;

public class Program
{
    public static void Main(string[] args)
    {
        var config = DefaultConfig.Instance.WithSummaryStyle(new SummaryStyle(null, true, SizeUnit.B, TimeUnit.Microsecond, ratioStyle: RatioStyle.Trend));

        // BenchmarkRunner.Run<TypeNamingBenchmarks>(config);
        // BenchmarkRunner.Run<MethodNamingBenchmarks>(config);
        BenchmarkRunner.Run<ExceptionBenchmarks>(config);
    }   
}
