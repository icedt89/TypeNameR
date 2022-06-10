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
    private static readonly FieldInfo? StackTraceBackingField = typeof(Exception).GetField("_stackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields

    public static bool WasStackTraceBackingFieldFound => ExceptionStackTraceManipulator.StackTraceBackingField is not null;

    /// <summary>
    /// Gets the original stacktrace if available.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/>.</param>
    /// <returns>The original stacktrace or <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> is <see langword="null"/>.</exception>
    public static string? GetOriginalStackTraceOrNull(this Exception exception)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        if (!exception.Data.Contains(ExceptionStackTraceManipulator.OriginalStackTraceKey))
        {
            return null;
        }

        return (string?)exception.Data[ExceptionStackTraceManipulator.OriginalStackTraceKey];
    }

    /// <summary>
    /// Restores the original stacktrace of the <see cref="Exception"/> if available.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/>.</param>
    /// <param name="removeStoredOriginalStackTrace">If <see langword="true"/> the original stacktrace will be removed from <see cref="Exception.Data"/>.</param>
    /// <returns>Returns <see langword="true"/> if the original stacktrace was restored.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> is <see langword="null"/>.</exception>
    public static bool TryRestoreOriginalStackTrace(this Exception exception, bool removeStoredOriginalStackTrace = true)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        var originalStackTrace = exception.GetOriginalStackTraceOrNull();
        if (originalStackTrace is null)
        {
            return false;
        }

        exception.SetStackTrace(new StringBuilder(originalStackTrace, originalStackTrace.Length));

        if (removeStoredOriginalStackTrace)
        {
            exception.Data.Remove(ExceptionStackTraceManipulator.OriginalStackTraceKey);
        }

        return true;
    }

    /// <summary>
    /// Sets the supplied stacktrace as <see cref="Exception.StackTrace"/> and optional stores the original stacktrace in <see cref="Exception.Data"/>.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/>.</param>
    /// <param name="stackTrace">The stacktrace.</param>
    /// <param name="storeOriginalStackTrace">If <see langword="true"/> the original stracktrace will be stored in <see cref="Exception.Data"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> or <paramref name="stackTrace"/> is <see langword="null"/>.</exception>
    public static void SetStackTrace(this Exception exception, StringBuilder stackTrace, bool storeOriginalStackTrace = true)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        if (storeOriginalStackTrace)
        {
            exception.Data[ExceptionStackTraceManipulator.OriginalStackTraceKey] = exception.StackTrace;
        }

        exception.SetStackTraceCore(stackTrace);
    }

    private static void SetStackTraceCore(this Exception exception, StringBuilder stackTrace)
    {
        try
        {
            ExceptionStackTraceManipulator.StackTraceBackingField!.SetValue(exception, stackTrace.ToString());
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
