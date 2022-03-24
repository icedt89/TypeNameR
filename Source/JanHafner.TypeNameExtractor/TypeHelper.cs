using System;

namespace JanHafner.TypeNameExtractor;

public static class TypeHelper
{
    /// <summary>
    /// Removes the "generic arguments count"-delimiter from the type name.
    /// E.g. TestClass`1 becomes TestClass, or TestClass`22 becomes TestClass.
    /// </summary>
    public static ReadOnlySpan<char> RemoveGenericParametersCount(ReadOnlySpan<char> typeName, char genericTypeParameterCountDelimiter)
    {
        var lastIndexOfGenericParamterDelimiter = typeName.LastIndexOf(genericTypeParameterCountDelimiter);
        if (lastIndexOfGenericParamterDelimiter > 0)
        {
            return typeName[..lastIndexOfGenericParamterDelimiter];
        }

        return typeName;
    }
}