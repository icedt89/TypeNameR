using JanHafner.TypeNameR.StackTrace;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace JanHafner.TypeNameR.Helper;

internal static class StackTraceHelper
{
    public static bool IsHidden(this MethodBase method)
        => method.MethodImplementationFlags.HasFlag(MethodImplAttributes.AggressiveInlining)
           || method.HasStackTraceHiddenAttribute()
           || (method.DeclaringType?.HasStackTraceHiddenAttribute() ?? false);

    public static bool TryGetStackFrame(this System.Diagnostics.StackTrace stackTrace, int stackFrameIndex, bool includeHiddenStackFrames,
        [NotNullWhen(true)] out StackFrame? stackFrame,
        out MethodBase? stackFrameMethod)
    {
        stackFrame = stackTrace.GetFrame(stackFrameIndex);
        stackFrameMethod = stackFrame?.GetMethod();

        return stackFrame is not null
               && (stackFrameMethod is null
                   || includeHiddenStackFrames
                   || !stackFrameMethod.IsHidden());
    }

    public static IEnumerable<RecursiveStackFrameMetadata> EnumerateRecursiveStackFrames(this System.Diagnostics.StackTrace stackTrace, bool includeHiddenStackFrames)
    {
        RecursiveStackFrameMetadata? previousRecursiveStackFrameMetadata = null;
        for (var stackFrameIndex = 0; stackFrameIndex < stackTrace.FrameCount; stackFrameIndex++)
        {
            if (!stackTrace.TryGetStackFrame(stackFrameIndex, includeHiddenStackFrames, out var stackFrame, out var stackFrameMethod))
            {
                continue;
            }

            if (previousRecursiveStackFrameMetadata is not null)
            {
                if (previousRecursiveStackFrameMetadata.Value.IsSameMethod(stackFrameMethod))
                {
                    previousRecursiveStackFrameMetadata = previousRecursiveStackFrameMetadata.Value.IncrementCallDepth();

                    continue;
                }

                yield return previousRecursiveStackFrameMetadata.Value;
            }

            previousRecursiveStackFrameMetadata = new RecursiveStackFrameMetadata(stackFrame, method: stackFrameMethod);
        }
    }

    public static StackFrameMetadata GetStackFrameMetadata(this StackFrame stackFrame)
    {
        var fileName = stackFrame.GetFileName();
        if (fileName is null)
        {
            return StackFrameMetadata.Empty;
        }

        var lineNumber = stackFrame.GetFileLineNumber();
        var columnNumber = stackFrame.GetFileColumnNumber();

        return new StackFrameMetadata(fileName, lineNumber, columnNumber);
    }
}