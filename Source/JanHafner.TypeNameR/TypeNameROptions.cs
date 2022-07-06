namespace JanHafner.TypeNameR;

/// <summary>
/// The options class to configure a <see cref="JanHafner.TypeNameR"/> instance.
/// </summary>
public sealed class TypeNameROptions
{
    /// <summary>
    /// Defines the names used for predefined types.
    /// </summary>
    public IReadOnlyDictionary<Type, string> PredefinedTypeNames { get; set; } = new Dictionary<Type, string>(16)
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
    /// Namespaces starting with values defined in this list are excluded from stacktraces, if enabled via <see cref="NameRControlFlags.ExcludeStackFrameMethodsByNamespace"/>.
    /// </summary>
    public IEnumerable<string> ExcludedNamespaces { get; set; } = new string[]
    {
        "System."
    };
}
