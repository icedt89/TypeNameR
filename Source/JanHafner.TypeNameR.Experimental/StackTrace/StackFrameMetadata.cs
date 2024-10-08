using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace JanHafner.TypeNameR.Experimental.StackTrace;

/// <summary>
/// Contains source information for a <see cref="StackFrame"/>.
/// </summary>
[DebuggerDisplay("{FileName}:{LineNumber}:{ColumnNumber}")]
[ExcludeFromCodeCoverage]
public readonly ref struct StackFrameMetadata
{
    public static StackFrameMetadata Empty => new(ReadOnlySpan<char>.Empty, lineNumber: 0, columnNumber: 0);

    public StackFrameMetadata(ReadOnlySpan<char> fileName, int lineNumber, int columnNumber)
    {
        LineNumber = lineNumber;
        ColumnNumber = columnNumber;
        FileName = fileName;
        IsEmpty = fileName.IsEmpty;
    }

    /// <summary>
    /// The line number of the code from the source file.
    /// </summary>
    public readonly int LineNumber;

    /// <summary>
    /// The column number of the code from the source file.
    /// </summary>
    public readonly int ColumnNumber;

    /// <summary>
    /// The file name (or path) from the source file.
    /// </summary>
    public readonly ReadOnlySpan<char> FileName;

    /// <summary>
    /// Returns <see langword="true"/> if FileName is empty.
    /// </summary>
    public readonly bool IsEmpty;
}