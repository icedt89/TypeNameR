#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace JanHafner.TypeNameR.Helper;

/// <summary>
/// Provides methods for working with predefined type names, such as a lookup for primitive types, e.g. typeof(ulong) => "ulong" and so on.
/// </summary>
public static class PredefinedTypeNames
{
    public static IReadOnlyDictionary<Type, string> Default = new Dictionary<Type, string>(18)
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
        { typeof(nuint), "nuint" },
    }
#if NET8_0_OR_GREATER
    .ToFrozenDictionary()
#endif
        ;
}