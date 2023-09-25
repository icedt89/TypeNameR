#if NET8_0
using System.Collections.Frozen;
#endif

namespace JanHafner.TypeNameR;

/// <summary>
/// The options class to configure a <see cref="TypeNameR"/> instance.
/// </summary>
public sealed record TypeNameROptions
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="predefinedTypeNames">
    /// If <see langword="null"/>, a default list with well known type names will be populated. The supplied dictionary will be copied and frozen.
    /// <code>
    /// { typeof(string), "string" },
    /// { typeof(object), "object" },
    /// { typeof(bool), "bool" },
    /// { typeof(double), "double" },
    /// { typeof(decimal), "decimal" },
    /// { typeof(float), "float" },
    /// { typeof(sbyte), "sbyte" },
    /// { typeof(byte), "byte" },
    /// { typeof(void), "void" },
    /// { typeof(ushort), "ushort" },
    /// { typeof(short), "short" },
    /// { typeof(uint), "uint" },
    /// { typeof(int), "int" },
    /// { typeof(ulong), "ulong" },
    /// { typeof(long), "long" },
    /// { typeof(char), "char" },
    /// { typeof(nint), "nint" },
    /// { typeof(nuint), "nuint" }
    /// </code>
    /// </param>
    /// <param name="excludedNamespaces">If <see langword="null"/>, a default list containing "System." will be populated. The supplied list will be enumerated and fixed.</param>
    public TypeNameROptions(
        IReadOnlyDictionary<Type, string>? predefinedTypeNames = null,
        IEnumerable<string>? excludedNamespaces = null)
    {
        PredefinedTypeNames = (predefinedTypeNames?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? new Dictionary<Type, string>(18)
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
        })
#if NET8_0_OR_GREATER
            .ToFrozenDictionary()
#endif
        ;
        ExcludedNamespaces = excludedNamespaces?.ToArray() ?? new []
        {
            "System."
        };
    }
    
    /// <summary>
    /// Defines the names used for predefined types.
    /// </summary>
    public IReadOnlyDictionary<Type, string> PredefinedTypeNames { get; }
    
    /// <summary>
    /// Namespaces starting with values defined in this list are excluded from stacktrace`s, if enabled via <see cref="NameRControlFlags.ExcludeStackFrameMethodsByNamespace"/>.
    /// The values will be compared by <see cref="StringComparison.Ordinal"/>.
    /// </summary>
    // TODO: Public API does not mirror the intention of immutability. Maybe use ImmutableArray<T>? Benchmark string[] and ImmutableArray<string>!
    // ImmutableArray<T> seems to be a good choice.
    public string[] ExcludedNamespaces { get; }
}