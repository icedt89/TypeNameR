using System.Diagnostics;
using System.Reflection;
using RecursiveStackFrameMetadata = JanHafner.TypeNameR.Experimental.StackTrace.RecursiveStackFrameMetadata;
using StackFrameMetadata = JanHafner.TypeNameR.Experimental.StackTrace.StackFrameMetadata;

namespace JanHafner.TypeNameR.Experimental.Helper;

internal static class StackTraceHelper
{
    public static bool IsHidden(this MethodBase method)
        => method.MethodImplementationFlags.HasFlag(MethodImplAttributes.AggressiveInlining)
           || method.HasStackTraceHiddenAttribute()
           || (method.DeclaringType?.HasStackTraceHiddenAttribute() ?? false);

    public static IEnumerable<RecursiveStackFrameMetadata> ProcessStackFrames(this System.Diagnostics.StackTrace stackTrace, NameRControlFlags nameRControlFlags)
    {
        var previousRecursiveStackFrameMetadata = new RecursiveStackFrameMetadata();
        for (var stackFrameIndex = 0; stackFrameIndex < stackTrace.FrameCount; stackFrameIndex++)
        {
            var stackFrame = stackTrace.GetFrame(stackFrameIndex);
            var stackFrameMethod = stackFrame?.GetMethod();
            if (stackFrame is null
                || (stackFrameMethod is not null
                    && !nameRControlFlags.HasFlag(NameRControlFlags.IncludeHiddenStackFrames)
                    && stackFrameMethod.IsHidden()))
            {
                continue;
            }

            if (nameRControlFlags.HasFlag(NameRControlFlags.DontEliminateRecursiveStackFrames))
            {
                yield return new RecursiveStackFrameMetadata(stackFrame)
                {
                    Method = stackFrameMethod
                };

                continue;
            }

            if (previousRecursiveStackFrameMetadata.IsNotEmpty)
            {
                if (previousRecursiveStackFrameMetadata.Method == stackFrameMethod)
                {
                    previousRecursiveStackFrameMetadata.CallDepth++;

                    continue;
                }

                yield return previousRecursiveStackFrameMetadata;
            }

            previousRecursiveStackFrameMetadata = new RecursiveStackFrameMetadata(stackFrame)
            {
                Method = stackFrameMethod
            };
        }
    }

    public static StackFrameMetadata GetStackFrameMetadata(this StackFrame stackFrame)
    {
        var fileName = stackFrame.GetFileName();
        if (fileName is null)
        {
            return default;
        }

        var lineNumber = stackFrame.GetFileLineNumber();
        var columnNumber = stackFrame.GetFileColumnNumber();

        return new StackFrameMetadata(fileName, lineNumber, columnNumber);
    }
}