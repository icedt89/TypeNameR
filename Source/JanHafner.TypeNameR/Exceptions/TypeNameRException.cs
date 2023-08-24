using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace JanHafner.TypeNameR.Exceptions;

/// <summary>
/// Base class for all <see cref="Exception"/>s thrown by TypeNameR.
/// </summary>
[Serializable]
[ExcludeFromCodeCoverage]
public class TypeNameRException : ApplicationException
{
    public TypeNameRException() 
    { 
    }
    
    public TypeNameRException(string message)
        : base(message) 
    {
    }
    
    public TypeNameRException(string message, Exception inner)
        : base(message, inner) 
    { 
    }
    
    protected TypeNameRException(SerializationInfo info,
        StreamingContext context)
        : base(info, context) 
    {
    }
}