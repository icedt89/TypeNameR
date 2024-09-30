using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace JanHafner.TypeNameR.Experimental.StackTrace;

/// <summary>
/// Contains source information for a <see cref="StackFrame"/>.
/// </summary>
[DebuggerDisplay("{FileName}:{LineNumber}:{ColumnNumber}")]
[ExcludeFromCodeCoverage]
public readonly ref struct StackFrameMetadata(string? fileName, int lineNumber, int columnNumber)
{
    /// <summary>
    /// The line number of the code from the source file.
    /// </summary>
    public int LineNumber { get; } = lineNumber;

    /// <summary>
    /// The column number of the code from the source file.
    /// </summary>
    public int ColumnNumber { get; } = columnNumber;

    /// <summary>
    /// Returns <see langword="true"/> if FileName is null.
    /// </summary>
    [MemberNotNullWhen(false, nameof(IsEmpty))]
    public bool IsEmpty => FileName is null;

    /// <summary>
    /// The file name (or path) from the source file.
    /// </summary>
    public string? FileName { get; } = fileName;
}