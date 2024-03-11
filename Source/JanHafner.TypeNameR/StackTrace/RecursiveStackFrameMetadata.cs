using System.Diagnostics;
using System.Reflection;

namespace JanHafner.TypeNameR.StackTrace;

[DebuggerDisplay("[CallDepth = {CallDepth}] {StackFrame}")]
internal struct RecursiveStackFrameMetadata(StackFrame stackFrame)
{
    public StackFrame StackFrame { get; set; } = stackFrame;

    public MethodBase? Method { get; set; }

    public uint CallDepth { get; set; } = Constants.DefaultCallDepth;
}