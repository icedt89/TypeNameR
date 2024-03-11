using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.Helper;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.Dictionary;

/// <summary>
/// This benchmark compares the cost of creating empty dictionaries in various ways.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[HintColumn]
public class CreateEmptyDictionaryBenchmarks
{
    // | Method             | Job      | Runtime  | Mean       | Error     | StdDev    | Median     | Ratio | RatioSD | Rank | Hint        | Gen0   | Allocated | Alloc Ratio |
    // |------------------- |--------- |--------- |-----------:|----------:|----------:|-----------:|------:|--------:|-----:|------------ |-------:|----------:|------------:|
    // | FrozenEmpty        | .NET 6.0 | .NET 6.0 |  0.1297 ns | 0.0867 ns | 0.2431 ns |  0.0000 ns |  0.04 |    0.07 |    1 | Unsupported |      - |         - |        0.00 |
    // | ReadOnlyEmpty      | .NET 6.0 | .NET 6.0 |  0.1508 ns | 0.0590 ns | 0.0953 ns |  0.1210 ns |  0.04 |    0.02 |    2 | Unsupported |      - |         - |        0.00 |
    // | ImmutableEmpty     | .NET 6.0 | .NET 6.0 |  0.2676 ns | 0.0644 ns | 0.0632 ns |  0.2680 ns |  0.07 |    0.02 |    3 |             |      - |         - |        0.00 |
    // | NewEmptyDictionary | .NET 6.0 | .NET 6.0 |  3.9502 ns | 0.1398 ns | 0.3533 ns |  3.8835 ns |  1.00 |    0.00 |    4 |             | 0.0127 |      80 B |        1.00 |
    // |                    |          |          |            |           |           |            |       |         |      |             |        |           |             |
    // | ReadOnlyEmpty      | .NET 7.0 | .NET 7.0 |  0.0200 ns | 0.0245 ns | 0.0538 ns |  0.0000 ns | 0.003 |    0.01 |    1 | Unsupported |      - |         - |        0.00 |
    // | FrozenEmpty        | .NET 7.0 | .NET 7.0 |  0.0536 ns | 0.0447 ns | 0.0952 ns |  0.0000 ns | 0.008 |    0.01 |    1 | Unsupported |      - |         - |        0.00 |
    // | ImmutableEmpty     | .NET 7.0 | .NET 7.0 |  0.7997 ns | 0.0968 ns | 0.2745 ns |  0.6889 ns | 0.125 |    0.05 |    2 |             |      - |         - |        0.00 |
    // | NewEmptyDictionary | .NET 7.0 | .NET 7.0 |  6.6473 ns | 0.3904 ns | 1.1139 ns |  6.5230 ns | 1.000 |    0.00 |    3 |             | 0.0127 |      80 B |        1.00 |
    // |                    |          |          |            |           |           |            |       |         |      |             |        |           |             |
    // | ReadOnlyEmpty      | .NET 8.0 | .NET 8.0 |  0.4141 ns | 0.0756 ns | 0.1911 ns |  0.3479 ns |  0.03 |    0.01 |    1 |             |      - |         - |        0.00 |
    // | FrozenEmpty        | .NET 8.0 | .NET 8.0 |  0.6745 ns | 0.0529 ns | 0.0494 ns |  0.6679 ns |  0.05 |    0.00 |    2 |             |      - |         - |        0.00 |
    // | ImmutableEmpty     | .NET 8.0 | .NET 8.0 |  1.2653 ns | 0.1961 ns | 0.5781 ns |  1.0649 ns |  0.09 |    0.04 |    3 |             |      - |         - |        0.00 |
    // | NewEmptyDictionary | .NET 8.0 | .NET 8.0 | 15.1484 ns | 0.3779 ns | 1.0721 ns | 14.7756 ns |  1.00 |    0.00 |    4 |             | 0.0127 |      80 B |        1.00 |

    [Benchmark(Baseline = true)]
    public IReadOnlyDictionary<Type, string> NewEmptyDictionary() => new Dictionary<Type, string>();

    [Benchmark]
    public IReadOnlyDictionary<Type, string> EmptyDictionary() => EmptyDictionary<Type, string>.Instance;

    [Benchmark]
    public IReadOnlyDictionary<Type, string> ImmutableEmpty() => ImmutableDictionary<Type, string>.Empty;

    [Benchmark]
    [Hint(GlobalBenchmarkSettings.UnsupportedMessage)]
    [Hint(null, RuntimeMoniker.Net80)]
    public IReadOnlyDictionary<Type, string> ReadOnlyEmpty()
    {
#if NET8_0_OR_GREATER
        return ReadOnlyDictionary<Type, string>.Empty;
#else
        return default!;
#endif
    }

    [Benchmark]
    [Hint(GlobalBenchmarkSettings.UnsupportedMessage)]
    [Hint(null, RuntimeMoniker.Net80)]
    public IReadOnlyDictionary<Type, string> FrozenEmpty()
    {
#if NET8_0_OR_GREATER
        return FrozenDictionary<Type, string>.Empty;
#else
        return default!;
#endif
    }
}