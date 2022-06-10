namespace JanHafner.TypeNameR;

/// <summary>
/// The options class to configure a <see cref="JanHafner.TypeNameR"/> instance.
/// </summary>
public sealed class TypeNameROptions
{
    /// <summary>
    /// The default value used to configure <see cref="PredefinedTypeNames"/>.
    /// </summary>
    public static readonly IReadOnlyDictionary<Type, string> DefaultPredefinedTypeNames = new Dictionary<Type, string>
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
    };

    /// <summary>
    /// Defines the names used for primitive types.
    /// </summary>
    public IReadOnlyDictionary<Type, string> PredefinedTypeNames { get; set; } = DefaultPredefinedTypeNames;
}
