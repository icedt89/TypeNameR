using System.Diagnostics;
using System.Reflection;

namespace JanHafner.TypeNameR.StackTrace;

/// <summary>
/// Defined methods for resolving <see cref="StackFrameMetadata"/>.
/// </summary>
public interface IStackFrameMetadataProvider
{
    /// <summary>
    /// Tries to provide <see cref="StackFrameMetadata"/> for the supplied <see cref="StackFrame"/> and <see cref="MethodBase"/>.
    /// </summary>
    /// <param name="stackFrame">The <see cref="StackFrame"/> from which to resolve <see cref="StackFrameMetadata"/>.</param>
    /// <param name="method">The <see cref="MethodBase"/>, this is mostly the method of the <see cref="StackFrame"/>.</param>
    /// <returns>The <see cref="StackFrameMetadata"/> or <see langword="null"/>.</returns>
    StackFrameMetadata? ProvideStackFrameMetadata(StackFrame stackFrame, MethodBase method);
}
