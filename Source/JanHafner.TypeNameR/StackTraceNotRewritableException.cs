using System.Runtime.Serialization;

namespace JanHafner.TypeNameR;

/// <summary>
/// This Exception is thrown if the StackTrace of an <see cref="Exception"/> can not be set.
/// </summary>
[Serializable]
public class StackTraceNotRewritableException : Exception
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
