using System;

namespace JanHafner.TypeNameExtractor;

public static class TypeHelper
{
    /// <summary>
    /// Removes the "generic arguments count"-delimiter from the type name.
    /// E.g. TestClass`1 becomes TestClass, or TestClass`22 becomes TestClass.
    /// </summary>
    public static string RemoveGenericParametersCount(string typeName, string? genericTypeParameterCountDelimiter = null)
    {
        if (string.IsNullOrWhiteSpace(typeName))
        {
            throw new ArgumentException($"'{nameof(typeName)}' cannot be null or whitespace.", nameof(typeName));
        }

        var lastIndexOfGenericParamterDelimiter = typeName.LastIndexOf(genericTypeParameterCountDelimiter ?? TypeNameExtractorOptions.DefaultGenericParameterCountDelimiter);
        if (lastIndexOfGenericParamterDelimiter > 0)
        {
            return typeName[..lastIndexOfGenericParamterDelimiter];
        }

        return typeName;
    }
}
