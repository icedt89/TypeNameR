using JanHafner.TypeNameR.Exceptions;
using System.Reflection;
using System.Text;

namespace JanHafner.TypeNameR;

/// <summary>
/// Contains helper methods for manipulating an <see cref="Exception"/> stacktrace.
/// </summary>
public static class ExceptionStackTraceManipulator
{
    public const string OriginalStackTraceKey = "OriginalStackTrace";

#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
    // This internal field stores the string of the stacktrace internally.
    private static readonly FieldInfo? StackTraceBackingField = typeof(Exception).GetField("_stackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields

    public static bool StackTraceBackingFieldFound => StackTraceBackingField is not null;

    /// <summary>
    /// Gets the original stacktrace if available.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/>.</param>
    /// <returns>The original stacktrace or <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> is <see langword="null"/>.</exception>
    public static string? GetOriginalStackTraceOrNull(this Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return (string?)exception.Data[OriginalStackTraceKey];
    }

    /// <summary>
    /// Restores the original stacktrace of the <see cref="Exception"/> if available.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/>.</param>
    /// <returns>Returns <see langword="true"/> if the original stacktrace was restored.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> is <see langword="null"/>.</exception>
    public static bool TryRestoreOriginalStackTrace(this Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var originalStackTrace = (string?)exception.Data[OriginalStackTraceKey];
        if (originalStackTrace is null)
        {
            return false;
        }

        exception.SetStackTraceCore(new StringBuilder(originalStackTrace, originalStackTrace.Length), storeOriginalStackTrace: false);

        exception.Data.Remove(OriginalStackTraceKey);

        return true;
    }

    /// <summary>
    /// Sets the supplied stacktrace as <see cref="Exception.StackTrace"/> and optional stores the original stacktrace in <see cref="Exception.Data"/>.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/>.</param>
    /// <param name="stackTrace">The stacktrace.</param>
    /// <param name="storeOriginalStackTrace">If <see langword="true"/> the original stacktrace will be stored in <see cref="Exception.Data"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> or <paramref name="stackTrace"/> is <see langword="null"/>.</exception>
    public static void SetStackTrace(this Exception exception, StringBuilder stackTrace, bool storeOriginalStackTrace = true)
    {
        ArgumentNullException.ThrowIfNull(exception);
        ArgumentNullException.ThrowIfNull(stackTrace);

        exception.SetStackTraceCore(stackTrace, storeOriginalStackTrace);
    }

    private static void SetStackTraceCore(this Exception exception, StringBuilder stackTrace, bool storeOriginalStackTrace)
    {
        if (storeOriginalStackTrace)
        {
            exception.Data[OriginalStackTraceKey] = exception.StackTrace;
        }

        exception.SetStackTraceCore(stackTrace);
    }

    private static void SetStackTraceCore(this Exception exception, StringBuilder stackTrace)
    {
        try
        {
            StackTraceBackingField.SetValue(exception, stackTrace.ToString());
        }
        catch (FieldAccessException fieldAccessException)
        {
            throw new StackTraceNotRewritableException("The field is not accessible", fieldAccessException);
        }
        catch (TargetException targetException)
        {
            throw new StackTraceNotRewritableException("Unable to set value to field", targetException);
        }
    }
}
