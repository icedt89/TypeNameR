using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace JanHafner.TypeNameR.StackTrace;

[DebuggerDisplay("[{CallDepth}] {StackFrame}")]
[ExcludeFromCodeCoverage]
internal readonly struct RecursiveStackFrameMetadata
{
    public RecursiveStackFrameMetadata(StackFrame stackFrame, int callDepth = Constants.DefaultCallDepth, MethodBase? method = null)
    {
        StackFrame = stackFrame;
        CallDepth = callDepth;
        Method = method;
    }

    public readonly int CallDepth;

    public readonly StackFrame StackFrame;

    public readonly MethodBase? Method;

    public bool IsSameMethod(MethodBase? method)
    {
        if (Method is null || method is null)
        {
            return false;
        }

        return Method == method;
    }

    public RecursiveStackFrameMetadata IncrementCallDepth()
        => new(StackFrame, CallDepth + 1, Method);
}