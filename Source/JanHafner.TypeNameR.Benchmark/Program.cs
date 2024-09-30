using BenchmarkDotNet.Running;
using JanHafner.TypeNameR.Benchmark.Benchmarks.TypeHelper;

namespace JanHafner.TypeNameR.Benchmark;

public static class Program
{
    public static void Main()
    {
        // BenchmarkRunner.Run<CommonBenchmarks>();
        // BenchmarkRunner.Run<CreateEmptyArrayBenchmarks>();
        // BenchmarkRunner.Run<CreateEmptyDictionaryBenchmarks>();
        // BenchmarkRunner.Run<StringBuilderAppendBenchmarks>();
        // BenchmarkRunner.Run<StringBuilderAppendStringConstantsBenchmarks>();
        // BenchmarkRunner.Run<StringBuilderAppendCharConstantsBenchmarks>();
        // BenchmarkRunner.Run<StringBuilderInliningAppendBenchmarks>();
        // BenchmarkRunner.Run<ExperimentalBenchmarks>();
        // BenchmarkRunner.Run<InstancePassingBenchmarks>();
        // BenchmarkRunner.Run<ArrayForEachBenchmarks>();
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
        // BenchmarkRunner.Run<IndexOfBenchmarks>();
        // BenchmarkRunner.Run<EnumHasFlagBenchmarks>();
        // BenchmarkRunner.Run<ReadOnlyRefStructCopy>();
        // BenchmarkRunner.Run<GetFrameBenchmarks>();
        BenchmarkRunner.Run<IsGenericValueTupleBenchmarks>();
        // BenchmarkRunner.Run<SliceBenchmarks>();
        // BenchmarkRunner.Run<HashSetArrayEnumerationBenchmarks>();
        // BenchmarkRunner.Run<MethodGenericsBenchmarks>();
        // BenchmarkRunner.Run<MethodGenericsComplexBenchmarks>();
        // BenchmarkRunner.Run<TypeGenericsBenchmarks>();
        // BenchmarkRunner.Run<TypeGenericsComplexBenchmarks>();
        // BenchmarkRunner.Run<ProcessStackFramesBenchmarks>();

        // BenchmarkRunner.Run(new[] { typeof(TypeGenericsBenchmarks), typeof(MethodGenericsBenchmarks) });
    }
}