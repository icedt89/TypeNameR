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
/// This benchmark compares the cost of try-get-value'ing various dictionary types using struct keys.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
[HintColumn]
public class DictionaryTryGetValueStructKeyBenchmarks
{
    // | Method     | Job      | Runtime  | Mean       | Error     | StdDev    | Ratio | RatioSD | Rank | Hint        | Allocated | Alloc Ratio |
    // |----------- |--------- |--------- |-----------:|----------:|----------:|------:|--------:|-----:|------------ |----------:|------------:|
    // | Frozen     | .NET 6.0 | .NET 6.0 |  0.1407 ns | 0.0321 ns | 0.0300 ns |  0.01 |    0.00 |    1 | Unsupported |         - |          NA |
    // | Dictionary | .NET 6.0 | .NET 6.0 | 11.4998 ns | 0.2450 ns | 0.4355 ns |  1.00 |    0.00 |    2 |             |         - |          NA |
    // | ReadOnly   | .NET 6.0 | .NET 6.0 | 12.4143 ns | 0.2545 ns | 0.2381 ns |  1.05 |    0.05 |    3 |             |         - |          NA |
    // | Immutable  | .NET 6.0 | .NET 6.0 | 25.5985 ns | 0.5258 ns | 0.4918 ns |  2.17 |    0.09 |    4 |             |         - |          NA |
    // |            |          |          |            |           |           |       |         |      |             |           |             |
    // | Frozen     | .NET 7.0 | .NET 7.0 |  0.0170 ns | 0.0162 ns | 0.0136 ns | 0.002 |    0.00 |    1 | Unsupported |         - |          NA |
    // | Dictionary | .NET 7.0 | .NET 7.0 |  9.3578 ns | 0.1968 ns | 0.1840 ns | 1.000 |    0.00 |    2 |             |         - |          NA |
    // | ReadOnly   | .NET 7.0 | .NET 7.0 | 11.2429 ns | 0.1460 ns | 0.1366 ns | 1.202 |    0.02 |    3 |             |         - |          NA |
    // | Immutable  | .NET 7.0 | .NET 7.0 | 19.7211 ns | 0.3691 ns | 0.6168 ns | 2.094 |    0.09 |    4 |             |         - |          NA |
    // |            |          |          |            |           |           |       |         |      |             |           |             |
    // | Frozen     | .NET 8.0 | .NET 8.0 |  5.4213 ns | 0.1428 ns | 0.2346 ns |  0.90 |    0.05 |    1 |             |         - |          NA |
    // | Dictionary | .NET 8.0 | .NET 8.0 |  6.0154 ns | 0.0967 ns | 0.0904 ns |  1.00 |    0.00 |    2 |             |         - |          NA |
    // | ReadOnly   | .NET 8.0 | .NET 8.0 |  7.3721 ns | 0.1711 ns | 0.2164 ns |  1.22 |    0.04 |    3 |             |         - |          NA |
    // | Immutable  | .NET 8.0 | .NET 8.0 | 15.8133 ns | 0.3483 ns | 0.3420 ns |  2.63 |    0.07 |    4 |             |         - |          NA |

    private Guid typeGuid;

    private IReadOnlyDictionary<Guid, string> dictionary;

    private IReadOnlyDictionary<Guid, string> readOnlyDictionary;

    private IReadOnlyDictionary<Guid, string> immutableDictionary;
#if NET8_0_OR_GREATER
    
    private IReadOnlyDictionary<Guid, string> frozenDictionary;
#endif

    [GlobalSetup]
    public void GlobalSetup()
    {
        typeGuid = typeof(nuint).GUID;
        dictionary = new Dictionary<Guid, string>(18)
        {
            { typeof(string).GUID, "string" },
            { typeof(object).GUID, "object" },
            { typeof(bool).GUID, "bool" },
            { typeof(double).GUID, "double" },
            { typeof(decimal).GUID, "decimal" },
            { typeof(float).GUID, "float" },
            { typeof(sbyte).GUID, "sbyte" },
            { typeof(byte).GUID, "byte" },
            { typeof(void).GUID, "void" },
            { typeof(ushort).GUID, "ushort" },
            { typeof(short).GUID, "short" },
            { typeof(uint).GUID, "uint" },
            { typeof(int).GUID, "int" },
            { typeof(ulong).GUID, "ulong" },
            { typeof(long).GUID, "long" },
            { typeof(char).GUID, "char" },
            { typeof(nint).GUID, "nint" },
            { typeof(nuint).GUID, "nuint" }
        };
        readOnlyDictionary = new ReadOnlyDictionary<Guid, string>(new Dictionary<Guid, string>(18)
        {
            { typeof(string).GUID, "string" },
            { typeof(object).GUID, "object" },
            { typeof(bool).GUID, "bool" },
            { typeof(double).GUID, "double" },
            { typeof(decimal).GUID, "decimal" },
            { typeof(float).GUID, "float" },
            { typeof(sbyte).GUID, "sbyte" },
            { typeof(byte).GUID, "byte" },
            { typeof(void).GUID, "void" },
            { typeof(ushort).GUID, "ushort" },
            { typeof(short).GUID, "short" },
            { typeof(uint).GUID, "uint" },
            { typeof(int).GUID, "int" },
            { typeof(ulong).GUID, "ulong" },
            { typeof(long).GUID, "long" },
            { typeof(char).GUID, "char" },
            { typeof(nint).GUID, "nint" },
            { typeof(nuint).GUID, "nuint" }
        });
        immutableDictionary = new Dictionary<Guid, string>(18)
        {
            { typeof(string).GUID, "string" },
            { typeof(object).GUID, "object" },
            { typeof(bool).GUID, "bool" },
            { typeof(double).GUID, "double" },
            { typeof(decimal).GUID, "decimal" },
            { typeof(float).GUID, "float" },
            { typeof(sbyte).GUID, "sbyte" },
            { typeof(byte).GUID, "byte" },
            { typeof(void).GUID, "void" },
            { typeof(ushort).GUID, "ushort" },
            { typeof(short).GUID, "short" },
            { typeof(uint).GUID, "uint" },
            { typeof(int).GUID, "int" },
            { typeof(ulong).GUID, "ulong" },
            { typeof(long).GUID, "long" },
            { typeof(char).GUID, "char" },
            { typeof(nint).GUID, "nint" },
            { typeof(nuint).GUID, "nuint" }
        }.ToImmutableDictionary();
#if NET8_0_OR_GREATER
        frozenDictionary = new Dictionary<Guid, string>(18)
        {
            { typeof(string).GUID, "string" },
            { typeof(object).GUID, "object" },
            { typeof(bool).GUID, "bool" },
            { typeof(double).GUID, "double" },
            { typeof(decimal).GUID, "decimal" },
            { typeof(float).GUID, "float" },
            { typeof(sbyte).GUID, "sbyte" },
            { typeof(byte).GUID, "byte" },
            { typeof(void).GUID, "void" },
            { typeof(ushort).GUID, "ushort" },
            { typeof(short).GUID, "short" },
            { typeof(uint).GUID, "uint" },
            { typeof(int).GUID, "int" },
            { typeof(ulong).GUID, "ulong" },
            { typeof(long).GUID, "long" },
            { typeof(char).GUID, "char" },
            { typeof(nint).GUID, "nint" },
            { typeof(nuint).GUID, "nuint" }
        }.ToFrozenDictionary();
#endif
    }

    [Benchmark(Baseline = true)]
    public bool Dictionary() => dictionary.TryGetValue(typeGuid, out var value);

    [Benchmark]
    public bool ReadOnly() => readOnlyDictionary.TryGetValue(typeGuid, out var value);

    [Benchmark]
    public bool Immutable() => immutableDictionary.TryGetValue(typeGuid, out var value);

    [Benchmark]
    [Hint(GlobalBenchmarkSettings.UnsupportedMessage)]
    [Hint(null, RuntimeMoniker.Net80)]
    public bool Frozen()
    {
#if NET8_0_OR_GREATER
        return frozenDictionary.TryGetValue(typeGuid, out var value);
#else
        return false;
#endif
    }
}