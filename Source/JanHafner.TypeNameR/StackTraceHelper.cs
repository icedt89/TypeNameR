using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks.Sources;

namespace JanHafner.TypeNameR;

internal static class StackTraceHelper
{
    public static bool IsHidden(this MethodBase methodBase)
    {
        if (methodBase.MethodImplementationFlags.HasFlag(MethodImplAttributes.AggressiveInlining))
        {
            return true;
        }

        if (methodBase.HasStackTraceHiddenAttribute())
        {
            return true;
        }

        if (methodBase.DeclaringType is null)
        {
            return false;
        }

        if (methodBase.DeclaringType.HasStackTraceHiddenAttribute())
        {
            return true;
        }

        if (methodBase.IsValueTaskSource())
        {
            return true;
        }

        return false;
    }

    private static bool IsValueTaskSource(this MethodBase methodBase)
    {
        var iValueTaskSourceType = typeof(IValueTaskSource);

        return methodBase.Name.StartsWith(iValueTaskSourceType.Namespace!) && methodBase.Name.EndsWith(nameof(IValueTaskSource.GetResult));
    }

    public static (StackFrame StackFrame, MethodBase Method, uint CallDepth)[] EliminateRecursion(this System.Diagnostics.StackTrace stackTrace)
    {
        var recursiveStackFrames = new Dictionary<MethodBase, (StackFrame StackFrame, MethodBase Method, uint CallDepth)>(stackTrace.FrameCount);
        for (var frameIndex = 0; frameIndex < stackTrace.FrameCount; frameIndex++)
        {
            var stackFrame = stackTrace.GetFrame(frameIndex);
            if (stackFrame is null)
            {
#if DEBUG
                Debug.Assert(true);

#endif
                continue;
            }

            var stackFrameMethod = stackFrame.GetMethod();
            if (stackFrameMethod is null)
            {
#if DEBUG
                Debug.Assert(true);

#endif
                continue;
            }

            if (!recursiveStackFrames.TryGetValue(stackFrameMethod, out var recursiveStackFrame))
            {
                recursiveStackFrames[stackFrameMethod] = (stackFrame, stackFrameMethod, 1);

                continue;
            }

            recursiveStackFrames[stackFrameMethod] = (recursiveStackFrame.StackFrame, recursiveStackFrame.Method, recursiveStackFrame.CallDepth + 1);
        }

        return recursiveStackFrames.Values.ToArray();
    }

    [ExcludeFromCodeCoverage]
    private static bool HasStackTraceHiddenAttribute(this ICustomAttributeProvider customAttributeProvider)
    {
        return customAttributeProvider.IsDefined(typeof(StackTraceHiddenAttribute), false);
    }
}
