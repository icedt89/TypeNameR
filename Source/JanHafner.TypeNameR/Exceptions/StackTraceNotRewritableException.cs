using System.Diagnostics.CodeAnalysis;
#if !NET8_0_OR_GREATER
using System.Runtime.Serialization;
#endif

namespace JanHafner.TypeNameR.Exceptions;

/// <summary>
/// This Exception is thrown if the StackTrace of an <see cref="Exception"/> can not be set.
/// </summary>
[Serializable]
[ExcludeFromCodeCoverage]
public class StackTraceNotRewritableException : TypeNameRException
{
    public StackTraceNotRewritableException()
    {
    }

    public StackTraceNotRewritableException(string message)
        : base(message)
    {
    }

    public StackTraceNotRewritableException(string message, Exception inner)
        : base(message, inner)
    {
    }
#if !NET8_0_OR_GREATER
    
    protected StackTraceNotRewritableException(SerializationInfo info,
                                               StreamingContext context)
        : base(info, context)
    {
    }
#endif
}