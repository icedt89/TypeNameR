using System.Diagnostics;

namespace JanHafner.TypeNameR.StackTrace;

/// <summary>
/// Contains source information from a <see cref="StackFrame"/>.
/// </summary>
[DebuggerDisplay("{FileName}:{LineNumber}:{ColumnNumber}")]
public readonly struct StackFrameMetadata : IEquatable<StackFrameMetadata>
{
    public StackFrameMetadata(string fileName, int lineNumber, int columnNumber)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
        }

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
    public override bool Equals(object? obj)
    {
        return obj is StackFrameMetadata metadata && this.Equals(metadata);
    }

    /// <inheritdoc/>
    public bool Equals(StackFrameMetadata other)
    {
        return this.FileName == other.FileName &&
               this.LineNumber == other.LineNumber &&
               this.ColumnNumber == other.ColumnNumber;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.FileName, this.LineNumber, this.ColumnNumber);
    }

    /// <inheritdoc/>
    public static bool operator ==(StackFrameMetadata left, StackFrameMetadata right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc/>
    public static bool operator !=(StackFrameMetadata left, StackFrameMetadata right)
    {
        return !(left == right);
    }
}
