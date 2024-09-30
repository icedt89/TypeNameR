using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Method)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
public class ReadOnlyRefStructCopy
{
    [GlobalSetup]
    public void GlobalSetup()
    {
    }

    [Benchmark]
    public MySmallReadOnlyRefStruct CopySmall()
    {
        var current = new MySmallReadOnlyRefStruct();
        for (var i = 0; i < 10_000; i++)
        {
            var a = current;

            current = a;
        }

        return current;
    }

    [Benchmark]
    public MyMediumReadOnlyRefStruct CopyMedium()
    {
        var current = new MyMediumReadOnlyRefStruct();
        for (var i = 0; i < 10_000; i++)
        {
            var a = current;

            current = a;
        }

        return current;
    }

    [Benchmark]
    public MyLargeReadOnlyRefStruct CopyLarge()
    {
        var current = new MyLargeReadOnlyRefStruct();
        for (var i = 0; i < 10_000; i++)
        {
            var a = current;

            current = a;
        }

        return current;
    }

    public readonly ref struct MySmallReadOnlyRefStruct
    {
        public readonly int IntValue1;

        public readonly int IntValue2;

        public readonly ReadOnlySpan<char> SpanValue;
    }

    public readonly ref struct MyMediumReadOnlyRefStruct
    {
        public readonly int IntValue1;

        public readonly int IntValue2;

        public readonly int IntValue3;

        public readonly ReadOnlySpan<char> SpanValue1;

        public readonly ReadOnlySpan<char> SpanValue2;
    }

    public readonly ref struct MyLargeReadOnlyRefStruct
    {
        public readonly int IntValue1;

        public readonly int IntValue2;

        public readonly int IntValue3;

        public readonly int IntValue4;

        public readonly int IntValue5;

        public readonly ReadOnlySpan<char> SpanValue1;

        public readonly ReadOnlySpan<char> SpanValue2;

        public readonly ReadOnlySpan<char> SpanValue3;

        public readonly ReadOnlySpan<char> SpanValue4;
    }
}