using System.Diagnostics;

namespace JanHafner.TypeNameR.StackTrace;

/// <summary>
/// Defined methods for resolving <see cref="StackFrameMetadata"/>.
/// </summary>
public interface IStackFrameMetadataProvider
{
    /// <summary>
    /// Tries to provide <see cref="StackFrameMetadata"/> for the supplied <see cref="StackFrame"/>.
    /// </summary>
    /// <param name="stackFrame">The <see cref="StackFrame"/> from which to resolve <see cref="StackFrameMetadata"/>.</param>
    /// <returns>The <see cref="StackFrameMetadata"/> or <see langword="null"/>.</returns>
    StackFrameMetadata? GetStackFrameMetadata(StackFrame stackFrame);
}
