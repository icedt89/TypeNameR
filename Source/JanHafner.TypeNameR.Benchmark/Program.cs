using BenchmarkDotNet.Running;
using JanHafner.TypeNameR.Benchmark.Benchmarks;
using JanHafner.TypeNameR.Benchmark.Benchmarks.StackTraceHelper;

namespace JanHafner.TypeNameR.Benchmark;

public static class Program
{
    public static void Main()
    {
        // BenchmarkRunner.Run<CommonBenchmarks>();
        // BenchmarkRunner.Run<EnumHasFlagBenchmarks>();
        // BenchmarkRunner.Run<EnumHasFlagComplexBenchmarks>();
        // BenchmarkRunner.Run<CreateEmptyArrayBenchmarks>();
        // BenchmarkRunner.Run<CreateEmptyDictionaryBenchmarks>();
        // BenchmarkRunner.Run<StringBuilderAppendBenchmarks>();
        BenchmarkRunner.Run<InstancePassingBenchmarks>();
        // BenchmarkRunner.Run<ExperimentalBenchmarks>();
        // BenchmarkRunner.Run<EmptyDictionaryTryGetValueBenchmarks>();
        // BenchmarkRunner.Run<DictionaryTryGetValueReferenceKeyBenchmarks>();
        // BenchmarkRunner.Run<DictionaryTryGetValueReferenceValueKeyCompareBenchmarks>();
        // BenchmarkRunner.Run<DictionaryTryGetValueKeyBenchmarks>();
        // BenchmarkRunner.Run<PatternMatchingBenchmarks>();
        // BenchmarkRunner.Run<SetExceptionStackTraceStringFieldBenchmarks>();
        // BenchmarkRunner.Run<FullTypeNameBenchmarks>();
        // BenchmarkRunner.Run<DisplayOfMethodBenchmarks>();
        // BenchmarkRunner.Run<DisplayOfStackTraceBenchmarks>();
        // BenchmarkRunner.Run<StartsWithBenchmarks>();
        // BenchmarkRunner.Run<DisplayOfExceptionBenchmarks>();
        // BenchmarkRunner.Run<IsGenericValueTupleBenchmarks>();
        // BenchmarkRunner.Run<HashSetArrayEnumerationBenchmarks>();
        // BenchmarkRunner.Run<MethodGenericsBenchmarks>();
        // BenchmarkRunner.Run<MethodGenericsComplexBenchmarks>();
        // BenchmarkRunner.Run<TypeGenericsBenchmarks>();
        // BenchmarkRunner.Run<TypeGenericsComplexBenchmarks>();
        // BenchmarkRunner.Run<ProcessStackFramesBenchmarks>();

        // BenchmarkRunner.Run(new[] { typeof(TypeGenericsBenchmarks), typeof(MethodGenericsBenchmarks) });
    }
}