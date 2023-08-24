using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace JanHafner.TypeNameR.StackTrace;

/// <summary>
/// Contains source information from a <see cref="StackFrame"/>.
/// </summary>
[DebuggerDisplay("{FileName}:{LineNumber}:{ColumnNumber}")]
[ExcludeFromCodeCoverage]
public readonly struct StackFrameMetadata : IEquatable<StackFrameMetadata>
{
    public StackFrameMetadata(string fileName, int lineNumber, int columnNumber)
    {
        FileName = fileName;
        LineNumber = lineNumber;
        ColumnNumber = columnNumber;
    }

    /// <summary>
    /// The file name (or path) from the original source file.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// The line number of the code from the original source file.
    /// </summary>
    public int LineNumber { get; }

    /// <summary>
    /// The column number of the code from the original source file.
    /// </summary>
    public int ColumnNumber { get; }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is StackFrameMetadata metadata && Equals(metadata);

    /// <inheritdoc/>
    public bool Equals(StackFrameMetadata other) => string.Equals(FileName, other.FileName, StringComparison.OrdinalIgnoreCase)
                                                    && LineNumber == other.LineNumber
                                                    && ColumnNumber == other.ColumnNumber;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(FileName, LineNumber, ColumnNumber);

    /// <inheritdoc/>
    public static bool operator ==(StackFrameMetadata left, StackFrameMetadata right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(StackFrameMetadata left, StackFrameMetadata right) => !(left == right);
}
