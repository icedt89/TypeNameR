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
/// This benchmark compares the cost of try-get-value'ing various empty dictionary types.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[HintColumn]
public class EmptyDictionaryTryGetValueBenchmarks
{
    // | Method     | Job      | Runtime  | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | Rank | Hint        | Allocated | Alloc Ratio |
    // |----------- |--------- |--------- |----------:|----------:|----------:|----------:|------:|--------:|-----:|------------ |----------:|------------:|
    // | ReadOnly   | .NET 6.0 | .NET 6.0 | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.000 |    0.00 |    1 | Unsupported |         - |          NA |
    // | Frozen     | .NET 6.0 | .NET 6.0 | 0.1159 ns | 0.0436 ns | 0.1285 ns | 0.0368 ns | 0.016 |    0.02 |    2 | Unsupported |         - |          NA |
    // | Dictionary | .NET 6.0 | .NET 6.0 | 5.5037 ns | 0.1060 ns | 0.0885 ns | 5.5291 ns | 1.000 |    0.00 |    3 |             |         - |          NA |
    // | Immutable  | .NET 6.0 | .NET 6.0 | 8.2359 ns | 0.1657 ns | 0.1842 ns | 8.2593 ns | 1.500 |    0.04 |    4 |             |         - |          NA |
    // |            |          |          |           |           |           |           |       |         |      |             |           |             |
    // | ReadOnly   | .NET 7.0 | .NET 7.0 | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.000 |    0.00 |    1 | Unsupported |         - |          NA |
    // | Frozen     | .NET 7.0 | .NET 7.0 | 0.0111 ns | 0.0165 ns | 0.0155 ns | 0.0019 ns | 0.002 |    0.00 |    1 | Unsupported |         - |          NA |
    // | Dictionary | .NET 7.0 | .NET 7.0 | 4.3599 ns | 0.0812 ns | 0.0678 ns | 4.3749 ns | 1.000 |    0.00 |    2 |             |         - |          NA |
    // | Immutable  | .NET 7.0 | .NET 7.0 | 7.6027 ns | 0.1297 ns | 0.1213 ns | 7.5363 ns | 1.746 |    0.04 |    3 |             |         - |          NA |
    // |            |          |          |           |           |           |           |       |         |      |             |           |             |
    // | Frozen     | .NET 8.0 | .NET 8.0 | 0.3117 ns | 0.0340 ns | 0.0364 ns | 0.3096 ns |  0.17 |    0.02 |    1 |             |         - |          NA |
    // | Dictionary | .NET 8.0 | .NET 8.0 | 1.9056 ns | 0.0464 ns | 0.0434 ns | 1.9146 ns |  1.00 |    0.00 |    2 |             |         - |          NA |
    // | ReadOnly   | .NET 8.0 | .NET 8.0 | 3.1528 ns | 0.0477 ns | 0.0423 ns | 3.1419 ns |  1.66 |    0.04 |    3 |             |         - |          NA |
    // | Immutable  | .NET 8.0 | .NET 8.0 | 4.6509 ns | 0.0818 ns | 0.0683 ns | 4.6475 ns |  2.44 |    0.07 |    4 |             |         - |          NA |

    private IReadOnlyDictionary<Type, string> dictionary;

    private IReadOnlyDictionary<Type, string> emptyDictionary;

    private IReadOnlyDictionary<Type, string> immutableDictionary;
#if NET8_0_OR_GREATER
    
    private IReadOnlyDictionary<Type, string> readOnlyDictionary;
    
    private IReadOnlyDictionary<Type, string> frozenDictionary;
#endif

    [GlobalSetup]
    public void GlobalSetup()
    {
        dictionary = new Dictionary<Type, string>();
        emptyDictionary = EmptyDictionary<Type, string>.Instance;
        immutableDictionary = ImmutableDictionary<Type, string>.Empty;
#if NET8_0_OR_GREATER
        readOnlyDictionary = ReadOnlyDictionary<Type, string>.Empty;
        frozenDictionary = FrozenDictionary<Type, string>.Empty;
#endif
    }

    [Benchmark(Baseline = true)]
    public bool Dictionary() => dictionary.TryGetValue(typeof(object), out _);

    [Benchmark]
    public bool EmptyDictionary() => emptyDictionary.TryGetValue(typeof(object), out _);

    [Benchmark]
    public bool Immutable() => immutableDictionary.TryGetValue(typeof(object), out _);

    [Benchmark]
    [Hint(GlobalBenchmarkSettings.UnsupportedMessage)]
    [Hint(null, RuntimeMoniker.Net80)]
    public bool ReadOnly()
    {
#if NET8_0_OR_GREATER
        return readOnlyDictionary.TryGetValue(typeof(object), out _);
#else
        return default!;
#endif
    }

    [Benchmark]
    [Hint(GlobalBenchmarkSettings.UnsupportedMessage)]
    [Hint(null, RuntimeMoniker.Net80)]
    public bool Frozen()
    {
#if NET8_0_OR_GREATER
        return frozenDictionary.TryGetValue(typeof(object), out _);
#else
        return default!;
#endif
    }
}