namespace JanHafner.TypeNameExtractor;
/// <summary>
/// Defines helper methods used during building the human readable name of a <see cref="Type"/>. 
/// </summary>
public static class TypeHelper
{
    /// <summary>
    /// Removes the generic arguments count from the supplied <see cref="ReadOnlySpan{T}"/>.
    /// <br />
    /// Example: MyGenericType`2 becomes MyGenericType.
    /// </summary>
    /// <param name="typeName">The name of the <see cref="Type"/>.</param>
    /// <param name="genericTypeParameterCountDelimiter">The <see cref="char"/> which is used as delimiter.</param>
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