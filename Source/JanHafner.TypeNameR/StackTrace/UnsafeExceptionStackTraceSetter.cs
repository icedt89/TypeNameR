#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace JanHafner.TypeNameR.StackTrace;

internal static class UnsafeExceptionStackTraceSetter
{
    public static void SetValue(Exception exception, string stackTrace)
    {
#if NET8_0_OR_GREATER
        ref var stackTraceBackingFieldReference = ref GetStackTraceBackingField(exception);

        stackTraceBackingFieldReference = stackTrace;
#else
        throw new NotSupportedException("This method needs .net 8 and above to work");
#endif
    }
#if NET8_0_OR_GREATER
    
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = Constants.ExceptionStackTraceBackingFieldName)]
    private static extern ref string? GetStackTraceBackingField(Exception exception);
#endif
}