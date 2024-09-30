using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace JanHafner.TypeNameR.Experimental.StackTrace;

[DebuggerDisplay("[CallDepth = {CallDepth}] {StackFrame}")]
internal struct RecursiveStackFrameMetadata
{
    public RecursiveStackFrameMetadata(StackFrame stackFrame)
    {
        StackFrame = stackFrame;
    }

    public StackFrame StackFrame { get; init; }

    public MethodBase? Method { get; set; }

    public uint CallDepth { get; set; } = Constants.DefaultCallDepth;

    [MemberNotNullWhen(true, nameof(StackFrame))]
    public bool IsNotEmpty => StackFrame is not null;
}