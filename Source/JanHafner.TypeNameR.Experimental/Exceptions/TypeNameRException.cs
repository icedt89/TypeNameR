using System.Diagnostics.CodeAnalysis;
#if !NET8_0_OR_GREATER
using System.Runtime.Serialization;
#endif

namespace JanHafner.TypeNameR.Experimental.Exceptions;

/// <summary>
/// Base class for all <see cref="Exception"/>s thrown by TypeNameR.
/// </summary>
[Serializable]
[ExcludeFromCodeCoverage]
public class TypeNameRException : Exception
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
#if !NET8_0_OR_GREATER

    protected TypeNameRException(SerializationInfo info,
        StreamingContext context)
        : base(info, context)
    {
    }
#endif
}