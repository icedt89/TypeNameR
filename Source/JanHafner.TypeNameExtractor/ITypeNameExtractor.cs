namespace JanHafner.TypeNameExtractor;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public interface ITypeNameExtractor
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
{
    /// <summary>
    /// Extracts the human readable form of the supplied <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> from which to extract the human readable name.</param>
    /// <returns>The human readable name of the <see cref="Type"/> supplied via <paramref name="type"/>.</returns>
    HumanReadableTypeName ExtractReadableName(Type type);
}