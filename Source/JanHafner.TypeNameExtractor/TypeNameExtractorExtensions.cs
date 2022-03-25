namespace JanHafner.TypeNameExtractor;

/// <summary>
/// Defines extension methods to simplify calling the <see cref="ITypeNameExtractor"/>.
/// </summary>
public static class TypeNameExtractorExtensions
{
    /// <summary>
    /// Extracts the human readable name of the <see cref="Type"/> defined by <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Any type you want.</typeparam>
    /// <param name="typeNameExtractor">The <see cref="ITypeNameExtractor"/> which is used to extract the human readable name.</param>
    /// <returns>The human readable name of the <see cref="Type"/> supplied by <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="typeNameExtractor"/> is <c> null</c>.</exception>
    public static HumanReadableTypeName ExtractReadableName<T>(this ITypeNameExtractor typeNameExtractor)
    {
        if (typeNameExtractor is null)
        {
            throw new ArgumentNullException(nameof(typeNameExtractor));
        }

        return typeNameExtractor.ExtractReadableName(typeof(T));
    }
}
