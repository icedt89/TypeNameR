using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.Dictionary;

/// <summary>
/// This benchmark compares the cost of try-get-value'ing various dictionary types using reference keys.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[HintColumn]
public class DictionaryTryGetValueReferenceKeyBenchmarks
{
    // | Method     | Job      | Runtime  | Mean       | Error     | StdDev    | Median     | Ratio | RatioSD | Rank | Hint        | Allocated | Alloc Ratio |
    // |----------- |--------- |--------- |-----------:|----------:|----------:|-----------:|------:|--------:|-----:|------------ |----------:|------------:|
    // | Frozen     | .NET 6.0 | .NET 6.0 |  0.0000 ns | 0.0000 ns | 0.0000 ns |  0.0000 ns | 0.000 |    0.00 |    1 | Unsupported |         - |          NA |
    // | Dictionary | .NET 6.0 | .NET 6.0 | 12.3580 ns | 0.2839 ns | 0.4585 ns | 12.2390 ns | 1.000 |    0.00 |    2 |             |         - |          NA |
    // | ReadOnly   | .NET 6.0 | .NET 6.0 | 15.1515 ns | 0.3247 ns | 0.6706 ns | 15.0032 ns | 1.228 |    0.08 |    3 |             |         - |          NA |
    // | Immutable  | .NET 6.0 | .NET 6.0 | 16.1584 ns | 0.3717 ns | 0.6702 ns | 16.1371 ns | 1.308 |    0.08 |    4 |             |         - |          NA |
    // |            |          |          |            |           |           |            |       |         |      |             |           |             |
    // | Frozen     | .NET 7.0 | .NET 7.0 |  0.0048 ns | 0.0103 ns | 0.0096 ns |  0.0000 ns | 0.000 |    0.00 |    1 | Unsupported |         - |          NA |
    // | Dictionary | .NET 7.0 | .NET 7.0 | 15.4971 ns | 0.3173 ns | 0.3395 ns | 15.5027 ns | 1.000 |    0.00 |    2 |             |         - |          NA |
    // | ReadOnly   | .NET 7.0 | .NET 7.0 | 17.2024 ns | 0.3731 ns | 0.7008 ns | 17.0217 ns | 1.102 |    0.04 |    3 |             |         - |          NA |
    // | Immutable  | .NET 7.0 | .NET 7.0 | 19.4590 ns | 1.2304 ns | 3.4300 ns | 18.4956 ns | 1.204 |    0.19 |    4 |             |         - |          NA |
    // |            |          |          |            |           |           |            |       |         |      |             |           |             |
    // | Dictionary | .NET 8.0 | .NET 8.0 |  9.6099 ns | 0.2200 ns | 0.4592 ns |  9.4881 ns |  1.00 |    0.00 |    1 |             |         - |          NA |
    // | Frozen     | .NET 8.0 | .NET 8.0 | 10.3965 ns | 0.2490 ns | 0.7103 ns | 10.1810 ns |  1.09 |    0.09 |    2 |             |         - |          NA |
    // | ReadOnly   | .NET 8.0 | .NET 8.0 | 11.5202 ns | 0.2516 ns | 0.3089 ns | 11.5108 ns |  1.19 |    0.06 |    3 |             |         - |          NA |
    // | Immutable  | .NET 8.0 | .NET 8.0 | 17.9053 ns | 0.3716 ns | 0.4424 ns | 17.7741 ns |  1.85 |    0.09 |    4 |             |         - |          NA |

    private Type type;

    private IReadOnlyDictionary<Type, string> dictionary;

    private IReadOnlyDictionary<Type, string> readOnlyDictionary;

    private IReadOnlyDictionary<Type, string> immutableDictionary;
#if NET8_0_OR_GREATER
    
    private IReadOnlyDictionary<Type, string> frozenDictionary;
#endif

    [GlobalSetup]
    public void GlobalSetup()
    {
        type = typeof(nuint);
        dictionary = new Dictionary<Type, string>(18)
        {
            { typeof(string), "string" },
            { typeof(object), "object" },
            { typeof(bool), "bool" },
            { typeof(double), "double" },
            { typeof(decimal), "decimal" },
            { typeof(float), "float" },
            { typeof(sbyte), "sbyte" },
            { typeof(byte), "byte" },
            { typeof(void), "void" },
            { typeof(ushort), "ushort" },
            { typeof(short), "short" },
            { typeof(uint), "uint" },
            { typeof(int), "int" },
            { typeof(ulong), "ulong" },
            { typeof(long), "long" },
            { typeof(char), "char" },
            { typeof(nint), "nint" },
            { typeof(nuint), "nuint" }
        };
        readOnlyDictionary = new ReadOnlyDictionary<Type, string>(new Dictionary<Type, string>(18)
        {
            { typeof(string), "string" },
            { typeof(object), "object" },
            { typeof(bool), "bool" },
            { typeof(double), "double" },
            { typeof(decimal), "decimal" },
            { typeof(float), "float" },
            { typeof(sbyte), "sbyte" },
            { typeof(byte), "byte" },
            { typeof(void), "void" },
            { typeof(ushort), "ushort" },
            { typeof(short), "short" },
            { typeof(uint), "uint" },
            { typeof(int), "int" },
            { typeof(ulong), "ulong" },
            { typeof(long), "long" },
            { typeof(char), "char" },
            { typeof(nint), "nint" },
            { typeof(nuint), "nuint" }
        });
        immutableDictionary = new Dictionary<Type, string>(18)
        {
            { typeof(string), "string" },
            { typeof(object), "object" },
            { typeof(bool), "bool" },
            { typeof(double), "double" },
            { typeof(decimal), "decimal" },
            { typeof(float), "float" },
            { typeof(sbyte), "sbyte" },
            { typeof(byte), "byte" },
            { typeof(void), "void" },
            { typeof(ushort), "ushort" },
            { typeof(short), "short" },
            { typeof(uint), "uint" },
            { typeof(int), "int" },
            { typeof(ulong), "ulong" },
            { typeof(long), "long" },
            { typeof(char), "char" },
            { typeof(nint), "nint" },
            { typeof(nuint), "nuint" }
        }.ToImmutableDictionary();
#if NET8_0_OR_GREATER
        frozenDictionary = new Dictionary<Type, string>(18)
        {
            { typeof(string), "string" },
            { typeof(object), "object" },
            { typeof(bool), "bool" },
            { typeof(double), "double" },
            { typeof(decimal), "decimal" },
            { typeof(float), "float" },
            { typeof(sbyte), "sbyte" },
            { typeof(byte), "byte" },
            { typeof(void), "void" },
            { typeof(ushort), "ushort" },
            { typeof(short), "short" },
            { typeof(uint), "uint" },
            { typeof(int), "int" },
            { typeof(ulong), "ulong" },
            { typeof(long), "long" },
            { typeof(char), "char" },
            { typeof(nint), "nint" },
            { typeof(nuint), "nuint" }
        }.ToFrozenDictionary();
#endif
    }

    [Benchmark(Baseline = true)]
    public bool Dictionary() => dictionary.TryGetValue(type, out var value);

    [Benchmark]
    public bool ReadOnly() => readOnlyDictionary.TryGetValue(type, out var value);

    [Benchmark]
    public bool Immutable() => immutableDictionary.TryGetValue(type, out var value);

    [Benchmark]
    [Hint(GlobalBenchmarkSettings.UnsupportedMessage)]
    [Hint(null, RuntimeMoniker.Net80)]
    public bool Frozen()
    {
#if NET8_0_OR_GREATER
        return frozenDictionary.TryGetValue(type, out var value);
#else
        return false;
#endif
    }
}