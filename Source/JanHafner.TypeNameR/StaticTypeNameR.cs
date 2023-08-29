namespace JanHafner.TypeNameR;

/// <summary>
/// Public static available <see cref="ITypeNameR"/> (must be set up first!).
/// </summary>
public static class StaticTypeNameR
{
    private static ITypeNameR? instance;

    /// <summary>
    /// Gets the default <see cref="ITypeNameR"/> used by extension methods in this class if no custom <see cref="ITypeNameR"/> is supplied.
    /// </summary>
    public static ITypeNameR Instance => instance ?? throw new InvalidOperationException("StaticTypeNameR not set up");

    /// <summary>
    /// Sets the supplied <see cref="ITypeNameR"/> as default for <see cref="Instance"/>.
    /// </summary>
    /// <param name="typeNameR">The <see cref="ITypeNameR"/> used as default.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="typeNameR"/> is <see langword="null"/>.</exception>
    public static void SetDefaultTypeNameR(ITypeNameR typeNameR) => instance = typeNameR ?? throw new ArgumentNullException(nameof(typeNameR));
}
