namespace JanHafner.TypeNameExtractor;

/// <summary>
/// The options class to configure a <see cref="TypeNameExtractor"/> instance.
/// </summary>
public sealed class TypeNameExtractorOptions
{
    /// <summary>
    /// The default value used to configure <see cref="OutputTypeVariableNames"/>.
    /// </summary>
    public const bool DefaultOutputTypeVariableNames = true;

    /// <summary>
    /// The default value used to configure <see cref="DefaultUseClrTypeNameForPrimitiveTypes"/>.
    /// </summary>
    public const bool DefaultUseClrTypeNameForPrimitiveTypes = true;

    /// <summary>
    /// The default value used to configure <see cref="DefaultFullQualifyOuterMostTypeNameOnNestedTypes"/>.
    /// </summary>
    public const bool DefaultFullQualifyOuterMostTypeNameOnNestedTypes = true;

    /// <summary>
    /// The default value used to configure <see cref="DefaultUseNullableTypeShortForm"/>.
    /// </summary>
    public const bool DefaultUseNullableTypeShortForm = true;

    /// <summary>
    /// The default value used to configure <see cref="PrimitiveTypeNames"/>.
    /// </summary>
    public static readonly IReadOnlyDictionary<Type, string> DefaultPrimitiveTypeNames = new Dictionary<Type, string>
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
    /// Handles the output of type variable names in generic parameters.
    /// <br />
    /// Example: If set to <c>true</c>, <c>MyGenericType{T}</c> becomes <c>MyGenericType{T}</c>, otherweise it becomes <c>MyGenericType{}</c>.
    /// </summary>
    public bool OutputTypeVariableNames { get; set; } = DefaultOutputTypeVariableNames;

    /// <summary>
    /// Handles the output of primitive type names such as int or string.
    /// <br />
    /// Example: If set to <c>true</c>, <c>int</c> becomes <c>Int32</c>, otherweise it becomes <c>int</c>.
    /// </summary>
    public bool UseClrTypeNameForPrimitiveTypes { get; set; } = DefaultUseClrTypeNameForPrimitiveTypes;

    /// <summary>
    /// Handles the output of the type name of the outer most <see cref="Type"/> of a nested <see cref="Type"/>.
    /// <br />
    /// Example: If set to <c>true</c>, <c>MyClass+InnerClass</c> becomes <c>MyNamespace.MySubNamespace.MyClass+InnerClass</c>, otherwise it becomes <c>MyClass+InnerClass</c>.
    /// </summary>
    public bool FullQualifyOuterMostTypeNameOnNestedTypes { get; set; } = DefaultFullQualifyOuterMostTypeNameOnNestedTypes;

    /// <summary>
    /// Handles the output of the short form of a <see cref="Nullable{T}"/> type.
    /// <br />
    /// Example: If set to <c>true</c>, <c>Nullable{T}</c> becomes <c>T?</c>, otherwise it becomes  <c>Nullable{T}</c>.
    /// </summary>
    public bool UseNullableTypeShortForm { get; set; } = DefaultUseNullableTypeShortForm;

    /// <summary>
    /// Defines the names used for primitive types. Will only be applied if <see cref="UseClrTypeNameForPrimitiveTypes"/> is set to <c>true</c>.
    /// </summary>
    public IReadOnlyDictionary<Type, string> PrimitiveTypeNames { get; set; } = DefaultPrimitiveTypeNames;
}
