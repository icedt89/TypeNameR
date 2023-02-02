using System.Diagnostics;
using System.Reflection;

namespace JanHafner.TypeNameR;

public static class StaticTypeNameR
{
    private static ITypeNameR defaultTypeNameR = new TypeNameR();

    /// <summary>
    /// Gets the default <see cref="ITypeNameR"/> used by extension methods in this class if no custom <see cref="ITypeNameR"/> is supplied.
    /// </summary>
    public static ITypeNameR DefaultTypeNameR => defaultTypeNameR;

    /// <summary>
    /// Sets the supplied <see cref="ITypeNameR"/> as default for <see cref="DefaultTypeNameR"/>.
    /// </summary>
    /// <param name="typeNameR">The <see cref="ITypeNameR"/> used as default.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="typeNameR"/> is <see langword="null"/>.</exception>
    public static void SetDefaultTypeNameR(ITypeNameR typeNameR)
    {
        if (typeNameR is null)
        {
            throw new ArgumentNullException(nameof(typeNameR));
        }

        defaultTypeNameR = typeNameR;
    }

    public static string ExtractReadable<T>(this ITypeNameR typeNameR, bool fullTypeName = false)
    {
        if (typeNameR is null)
        {
            throw new ArgumentNullException(nameof(typeNameR));
        }

        return typeNameR.ExtractReadable(typeof(T), fullTypeName);
    }

    public static string ExtractReadable(this Type type, bool fullTypeName = false)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        return defaultTypeNameR.ExtractReadable(type, fullTypeName);
    }

    public static string ExtractReadable(this MethodInfo methodInfo,
                                              NameRControlFlags nameRControlFlags = NameRControlFlags.None)
    {
        if (methodInfo is null)
        {
            throw new ArgumentNullException(nameof(methodInfo));
        }

        return defaultTypeNameR.ExtractReadable(methodInfo, nameRControlFlags);
    }

    public static string ExtractReadable(this ParameterInfo parameterInfo,
                                              NameRControlFlags nameRControlFlags = NameRControlFlags.None)
    {
        if (parameterInfo is null)
        {
            throw new ArgumentNullException(nameof(parameterInfo));
        }

        return defaultTypeNameR.ExtractReadable(parameterInfo, nameRControlFlags);
    }

    public static string ExtractReadable(StackFrame stackFrame,
                                         NameRControlFlags nameRControlFlags = NameRControlFlags.None)
    {
        if (stackFrame is null)
        {
            throw new ArgumentNullException(nameof(stackFrame));
        }

        return defaultTypeNameR.ExtractReadable(stackFrame, nameRControlFlags);
    }

    public static string ExtractReadable(this System.Diagnostics.StackTrace stackTrace,
                                              NameRControlFlags nameRControlFlags = NameRControlFlags.None)
    {
        if (stackTrace is null)
        {
            throw new ArgumentNullException(nameof(stackTrace));
        }

        return defaultTypeNameR.ExtractReadable(stackTrace, nameRControlFlags);
    }

    public static string ExtractReadableStackTrace(this Exception exception,
                                                        NameRControlFlags nameRControlFlags = NameRControlFlags.None)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        return defaultTypeNameR.ExtractReadableStackTrace(exception, nameRControlFlags);
    }

    public static TException RewriteStackTraces<TException>(this TException exception,
                                                                 NameRControlFlags nameRControlFlags = NameRControlFlags.None)
        where TException : notnull, Exception
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        return defaultTypeNameR.RewriteStackTrace(exception, nameRControlFlags);
    }
}
