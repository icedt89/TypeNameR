using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.Dictionary;

/// <summary>
/// This benchmark compares the cost of try-get-value'ing reference and struct key types.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
[HintColumn]
public class DictionaryTryGetValueReferenceAndStructKeyBenchmarks
{
    private Type? type;

    private IReadOnlyDictionary<Type, string>? dictionaryReferenceKey;

    private IReadOnlyDictionary<Guid, string>? dictionaryStructKey;

#if NET8_0_OR_GREATER
    
    private IReadOnlyDictionary<Type, string>? frozenDictionaryReferenceKey;
    
    private IReadOnlyDictionary<Guid, string>? frozenDictionaryStructKey;
#endif

    [GlobalSetup]
    public void GlobalSetup()
    {
        type = typeof(nuint);
        dictionaryReferenceKey = new Dictionary<Type, string>(18)
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
        dictionaryStructKey = new Dictionary<Guid, string>(18)
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
#if NET8_0_OR_GREATER
        frozenDictionaryReferenceKey = new Dictionary<Type, string>(18)
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
        
        frozenDictionaryStructKey = new Dictionary<Guid, string>(18)
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
    public bool DictionaryReferenceKey() => dictionaryReferenceKey!.TryGetValue(type!, out var value);

    [Benchmark]
    public bool DictionaryStructKey() => dictionaryStructKey!.TryGetValue(type!.GUID, out var value);

    [Benchmark]
    [Hint(GlobalBenchmarkSettings.UnsupportedMessage)]
    [Hint(null, RuntimeMoniker.Net80)]
    public bool FrozenReferenceKey()
    {
#if NET8_0_OR_GREATER
        return frozenDictionaryReferenceKey!.TryGetValue(type!, out var value);
#else
        return false;
#endif
    }

    [Benchmark]
    [Hint(GlobalBenchmarkSettings.UnsupportedMessage)]
    [Hint(null, RuntimeMoniker.Net80)]
    public bool FrozenStructKey()
    {
#if NET8_0_OR_GREATER
        return frozenDictionaryStructKey!.TryGetValue(type!.GUID, out var value);
#else
        return false;
#endif
    }
}