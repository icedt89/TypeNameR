using System;
using System.Collections.Generic;

namespace JanHafner.TypeNameExtractor;

public sealed class TypeNameExtractorOptions
{
    public const bool DefaultOutputTypeVariableNames = true;

    public const bool DefaultUseClrTypeNameForPrimitiveTypes = true;

    public const bool DefaultFullQualifyOuterMostTypeNameOnNestedTypes = true;

    public const bool DefaultUseNullableTypeShortForm = true;

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

    public bool OutputTypeVariableNames { get; set; } = DefaultOutputTypeVariableNames;

    public bool UseClrTypeNameForPrimitiveTypes { get; set; } = DefaultUseClrTypeNameForPrimitiveTypes;

    public bool FullQualifyOuterMostTypeNameOnNestedTypes { get; set; } = DefaultFullQualifyOuterMostTypeNameOnNestedTypes;

    public bool UseNullableTypeShortForm { get; set; } = DefaultUseNullableTypeShortForm;

    public IReadOnlyDictionary<Type, string> PredefinedTypeNames { get; set; } = DefaultPredefinedTypeNames;
}
