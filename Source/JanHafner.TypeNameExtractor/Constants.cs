namespace JanHafner.TypeNameExtractor;

/// <summary>
/// Defines constants used during building the human readable name of a <see cref="Type"/>.
/// </summary>
public static class Constants
{
    /// <summary>
    /// The <see cref="char"/> used to mark the start of generic parameters.
    /// </summary>
    public const char GenericTypeOpeningBracket = '<';

    /// <summary>
    /// The <see cref="char"/> used to mark the end of generic parameters.
    /// </summary>
    public const char GenericTypeClosingBracket = '>';

    /// <summary>
    /// The <see cref="char"/> used to mark the start of the count of generic parameters retrieved from the <see cref="Type"/>.Name.
    /// </summary>
    public const char GenericParameterCountDelimiter = '`';

    /// <summary>
    /// The <see cref="char"/> used to join generic parameters.
    /// </summary>
    public const char GenericParameterDelimiter = ',';

    /// <summary>
    /// The <see cref="char"/> used to join array rank.
    /// </summary>
    public const char ArrayRankDelimiter = ',';

    /// <summary>
    /// The <see cref="char"/> used mark <see cref="Nullable{T}"/>-Types in their short form.
    /// </summary>
    public const char NullableTypeMarker = '?';

    /// <summary>
    /// The <see cref="char"/> used to join nested types.
    /// </summary>
    public const char NestedTypeDelimiter = '+';

    /// <summary>
    /// The <see cref="char"/> used to mark the start of an <see cref="Array"/>.
    /// </summary>
    public const char ArrayOpeningBracket = '[';

    /// <summary>
    /// The <see cref="char"/> used to mark the end of an <see cref="Array"/>.
    /// </summary>
    public const char ArrayClosingBracket = ']';
}
