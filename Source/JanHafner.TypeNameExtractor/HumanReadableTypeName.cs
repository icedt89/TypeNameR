using System.Diagnostics;
using System.Text;

namespace JanHafner.TypeNameExtractor;

/// <summary>
/// Encapsulates the <see cref="System.Type"/> and human readable name.
/// </summary>
[DebuggerDisplay("{Name}")]
public sealed class HumanReadableTypeName
{
    private readonly StringBuilder typeNameBuilder;

    /// <summary>
    /// Initializes a new instance of the <see cref="HumanReadableTypeName"/> class.
    /// </summary>
    /// <param name="type">The <see cref="System.Type"/> for which this <see cref="HumanReadableTypeName"/> instance was created.</param>
    /// <param name="typeNameBuilder">Contains the human readable name of the supplied <see cref="System.Type"/>.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="type"/> or <paramref name="typeNameBuilder"/> is <c>null</c>.</exception>
    public HumanReadableTypeName(Type type, StringBuilder typeNameBuilder)
    {
        this.Type = type ?? throw new ArgumentNullException(nameof(type));
        this.typeNameBuilder = typeNameBuilder ?? throw new ArgumentNullException(nameof(typeNameBuilder));
    }

    /// <summary>
    /// Gets the <see cref="System.Type"/> for which this <see cref="HumanReadableTypeName"/> instance was created.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Gets the human readable name of the <see cref="System.Type"/>.
    /// </summary>
    public string Name => this.typeNameBuilder.ToString();

    /// <summary>
    /// Gets the human readable name of the <see cref="System.Type"/>.
    /// </summary>
    /// <returns>The value of <see cref="Name"/>.</returns>
    public override string ToString()
    {
        return this.Name;
    }
}