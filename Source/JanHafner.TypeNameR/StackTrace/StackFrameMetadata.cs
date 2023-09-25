using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace JanHafner.TypeNameR.StackTrace;

/// <summary>
/// Contains source information from a <see cref="StackFrame"/>.
/// </summary>
[DebuggerDisplay("{FileName}:{LineNumber}:{ColumnNumber}")]
[ExcludeFromCodeCoverage]
public readonly record struct StackFrameMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StackFrameMetadata"/> type.
    /// </summary>
    /// <param name="fileName">The file name (or path) from the original source file.</param>
    /// <param name="lineNumber">The line number of the code from the original source file.</param>
    /// <param name="columnNumber">The column number of the code from the original source file.</param>
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
}