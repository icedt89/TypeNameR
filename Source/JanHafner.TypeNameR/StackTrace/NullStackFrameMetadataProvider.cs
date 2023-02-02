using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace JanHafner.TypeNameR.StackTrace;

/// <summary>
/// Provides an implementation which does nothing.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class NullStackFrameMetadataProvider : IStackFrameMetadataProvider
{
    /// <summary>
    /// Does nothing.
    /// </summary>
    /// <param name="stackFrame">Is unused.</param>
    /// <returns>Always <see langword="null"/>.</returns>
    public StackFrameMetadata? GetStackFrameMetadata(StackFrame? stackFrame) => null;
}
