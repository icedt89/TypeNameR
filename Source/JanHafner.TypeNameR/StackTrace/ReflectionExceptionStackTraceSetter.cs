using System.Reflection;

namespace JanHafner.TypeNameR.StackTrace;

internal static class ReflectionExceptionStackTraceSetter
{
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
    // This internal field stores the string of the stacktrace internally.
    private static readonly FieldInfo? StackTraceBackingField = typeof(Exception).GetField(Constants.ExceptionStackTraceBackingFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields

    public static void SetValue(Exception exception, string stackTrace) 
        => StackTraceBackingField?.SetValue(exception, stackTrace);
}