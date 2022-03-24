using System;
using System.Collections.Generic;

namespace JanHafner.TypeNameExtractor;

public sealed class TypeNameExtractorOptions
{
    public static readonly string DefaultGenericTypeOpeningBracket = "<";

    public static readonly string DefaultGenericTypeClosingBracket = ">";

    public static readonly string DefaultGenericParameterCountDelimiter = "`";

    public static readonly string DefaultGenericParameterDelimiter = ",";

    public static readonly string DefaultNullableTypeMarker = "?";

    public static readonly string DefaultNestedTypeDelimiter = "+";

    public static readonly string DefaultArrayOpeningBracket = "[";

    public static readonly string DefaultArrayClosingBracket = "]";

    public static readonly bool DefaultOutputTypeVariableNames = true;

    public static readonly bool DefaultUseClrTypeNameForPrimitiveTypes = true;

    public static readonly bool DefaultFullQualifyOuterMostTypeNameOnNestedTypes = true;

    public static readonly bool DefaultUseNullableTypeShortForm = true;

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

    public string GenericTypeOpeningBracket { get; set; } = DefaultGenericTypeOpeningBracket;

    public string GenericTypeClosingBracket { get; set; } = DefaultGenericTypeClosingBracket;

    public string GenericParameterCountDelimiter { get; set; } = DefaultGenericParameterCountDelimiter;

    public string GenericParameterDelimiter { get; set; } = DefaultGenericParameterDelimiter;

    public string NullableTypeMarker { get; set; } = DefaultNullableTypeMarker;

    public string NestedTypeDelimiter { get; set; } = DefaultNestedTypeDelimiter;

    public string ArrayOpeningBracket { get; set; } = DefaultArrayOpeningBracket;

    public string ArrayClosingBracket { get; set; } = DefaultArrayClosingBracket;

    public bool OutputTypeVariableNames { get; set; } = DefaultOutputTypeVariableNames;

    public bool UseClrTypeNameForPrimitiveTypes { get; set; } = DefaultUseClrTypeNameForPrimitiveTypes;

    public bool FullQualifyOuterMostTypeNameOnNestedTypes { get; set; } = DefaultFullQualifyOuterMostTypeNameOnNestedTypes;

    public bool UseNullableTypeShortForm { get; set; } = DefaultUseNullableTypeShortForm;

    public IReadOnlyDictionary<Type, string> PredefinedTypeNames { get; set; } = DefaultPredefinedTypeNames;
}
