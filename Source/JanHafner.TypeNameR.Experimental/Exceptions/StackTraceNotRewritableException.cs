using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace JanHafner.TypeNameR.Experimental.Exceptions;

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

    protected StackTraceNotRewritableException(SerializationInfo info,
                                               StreamingContext context)
        : base(info, context)
    {
    }
}