using JanHafner.TypeNameR;

namespace Serilog.Extensions.TypeNameR;

/// <summary>
/// Defines extension methods for Serilog <see cref="ILogger"/>.
/// </summary>
public static class SerilogTypeNameRExtensions
{
    /// <summary>
    /// Uses the readable name of the <see cref="Type"/> defined by <paramref name="instance"/> as SourceContext.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> defined by <paramref name="instance"/>.</typeparam>
    /// <param name="logger">An instance of the <see cref="ILogger"/> provided by Serilog.</param>
    /// <param name="instance">The instance from which the readable type name should be used as SourceContext.</param>
    /// <param name="typeNameR">The <see cref="ITypeNameR"/> instance to use. If <see langword="null"/>, <see cref="StaticTypeNameR.DefaultTypeNameR"/> will be used.</param>
    /// <param name="throwExceptions">If set to <see langword="true"/> methods in this class will throw exceptions.</param>
    /// <param name="selfLog">If set to <see langword="true"/> minimal logging on error will also take action.</param>
    /// <returns>The <see cref="ILogger"/> instance after the call to <see cref="ILogger.ForContext(string, object, bool)"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="throwExceptions"/> is set to <see langword="true"/>, exceptions will be thrown if <paramref name="logger"/> or <paramref name="instance"/> are <see langword="null"/>.</exception>
    public static ILogger? For<T>(this ILogger? logger, T instance, ITypeNameR? typeNameR = null, bool throwExceptions = false, bool selfLog = false)
    {
        if (logger is null)
        {
            return logger;
        }

        if (instance is null)
        {
            throw new ArgumentNullException(nameof(instance));
        }

        return logger.ForTypeCore(instance.GetType(), typeNameR ?? StaticTypeNameR.DefaultTypeNameR, throwExceptions, selfLog);
    }

    /// <summary>
    /// Uses the readable name of the supplied <see cref="Type"/>.
    /// </summary>
    /// <param name="logger">An instance of the <see cref="ILogger"/> provided by Serilog.</param>
    /// <param name="type">The <see cref="Type"/>.</param>
    /// <param name="typeNameR">The <see cref="ITypeNameR"/> instance to use. If <see langword="null"/>, <see cref="StaticTypeNameR.DefaultTypeNameR"/> will be used.</param>
    /// <param name="throwExceptions">If set to <see langword="true"/> methods in this class will throw exceptions.</param>
    /// <param name="selfLog">If set to <see langword="true"/> minimal logging on error will also take action.</param>
    /// <returns>The <see cref="ILogger"/> instance after the call to <see cref="ILogger.ForContext(string, object, bool)"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="throwExceptions"/> is set to <see langword="true"/>, exceptions will be thrown if <paramref name="logger"/> is <see langword="null"/> or the type could not be retrived from the parent stack frame.</exception>
    public static ILogger? For(this ILogger? logger, Type type, ITypeNameR? typeNameR = null, bool throwExceptions = false, bool selfLog = false)
    {
        if (logger is null)
        {
            return logger;
        }

        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        return logger.ForTypeCore(type, typeNameR ?? StaticTypeNameR.DefaultTypeNameR, throwExceptions, selfLog);
    }

    private static ILogger ForTypeCore(this ILogger logger, Type type, ITypeNameR typeNameR, bool throwExceptions, bool selfLog)
    {
        try
        {
            throw new NotImplementedException();
            var callerName = typeNameR.ExtractReadable(type, true);

            return logger.ForContext(Core.Constants.SourceContextPropertyName, callerName);
        }
        catch (Exception exception)
        {
            if (selfLog)
            {
                logger.Error(exception, "Readable name of type could not be determined: {@Type}", type);
            }

            if (throwExceptions)
            {
                throw;
            }
        }

        return logger;
    }
}
