using JanHafner.TypeNameR.StackTrace;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace JanHafner.TypeNameR.Helper;

internal static class StackTraceHelper
{
    public static bool IsHidden(this MethodBase method)
        => method.MethodImplementationFlags.IsSet(MethodImplAttributes.AggressiveInlining)
           || method.HasStackTraceHiddenAttribute()
           || (method.DeclaringType?.HasStackTraceHiddenAttribute() ?? false);

    public static RecursiveStackFrameMetadata[] ProcessStackFrames(this System.Diagnostics.StackTrace stackTrace, NameRControlFlags nameRControlFlags)
    {
        var resultStackFrames = new Dictionary<object, RecursiveStackFrameMetadata>(stackTrace.FrameCount);
        for (var stackFrameIndex = 0; stackFrameIndex < stackTrace.FrameCount; stackFrameIndex++)
        {
            var stackFrame = stackTrace.GetFrame(stackFrameIndex);
            if (stackFrame is null)
            {
                continue;
            }

            object key = stackFrame;

            var stackFrameMethod = stackFrame.GetMethod();
            if (stackFrameMethod is not null)
            {
                if (!nameRControlFlags.IsSet(NameRControlFlags.IncludeHiddenStackFrames) && stackFrameMethod.IsHidden())
                {
                    continue;
                }

                if (!nameRControlFlags.IsSet(NameRControlFlags.DontEliminateRecursiveStackFrames))
                {
                    key = stackFrameMethod;
                }
            }

            ref var valOrNew = ref CollectionsMarshal.GetValueRefOrAddDefault(resultStackFrames, key, out var exists);
            if (!exists)
            {
                valOrNew.StackFrame = stackFrame;
                valOrNew.Method = stackFrameMethod;
                valOrNew.CallDepth = 1;
            }
            else
            {
                valOrNew.CallDepth++;
            }
        }

        return resultStackFrames.Values.ToArray();
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