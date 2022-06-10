using JanHafner.TypeNameR;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Serilog.Extensions.TypeNameR;

/// <summary>
/// Defines extension methods for Serilogs <see cref="ILogger"/>.
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
    /// <param name="selfLog">If set to <see langword="true"/> minimal loggin on verbose will also take action.</param>
    /// <returns>The <see cref="ILogger"/> instance after the call to <see cref="ILogger.ForContext(string, object, bool)"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="throwExceptions"/> is set to <see langword="true"/>, exceptions will be thrown if <paramref name="logger"/> or <paramref name="instance"/> are <see langword="null"/>.</exception>
    public static ILogger ForMe<T>(this ILogger logger, T instance, ITypeNameR? typeNameR = null, bool throwExceptions = false, bool selfLog = false)
    {
        if (logger is null)
        {
            if (throwExceptions)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            return logger!;
        }

        if (instance is null)
        {
            if (throwExceptions)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return logger;
        }

        return logger.ForTypeCore(instance.GetType(), typeNameR ?? StaticTypeNameR.DefaultTypeNameR, throwExceptions, selfLog);
    }

    /// <summary>
    /// Uses the readable name of the <see cref="Type"/> retrieved via <c>StackFrame(1).GetMethod().DeclaringType</c>.
    /// </summary>
    /// <param name="logger">An instance of the <see cref="ILogger"/> provided by Serilog.</param>
    /// <param name="typeNameR">The <see cref="ITypeNameR"/> instance to use. If <see langword="null"/>, <see cref="StaticTypeNameR.DefaultTypeNameR"/> will be used.</param>
    /// <param name="throwExceptions">If set to <see langword="true"/> methods in this class will throw exceptions.</param>
    /// <param name="selfLog">If set to <see langword="true"/> minimal loggin on verbose will also take action.</param>
    /// <returns>The <see cref="ILogger"/> instance after the call to <see cref="ILogger.ForContext(string, object, bool)"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="throwExceptions"/> is set to <see langword="true"/>, exceptions will be thrown if <paramref name="logger"/> is <see langword="null"/> or the type could not be retrived from the parent stack frame.</exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static ILogger ForMe(this ILogger logger, ITypeNameR? typeNameR = null, bool throwExceptions = false, bool selfLog = false)
    {
        if (logger is null)
        {
            if (throwExceptions)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            return logger!;
        }

        var type = new StackFrame(1).GetMethod()?.DeclaringType;

        if (type is null)
        {
            if (selfLog)
            {
                logger.Verbose("Type could not be retrieved from the parent stack frame");
            }

            if (throwExceptions)
            {
                throw new InvalidOperationException("Type could not be retrieved from the parent stack frame");
            }

            return logger;
        }

        return logger.ForTypeCore(type, typeNameR ?? StaticTypeNameR.DefaultTypeNameR, throwExceptions, selfLog);
    }

    private static ILogger ForTypeCore(this ILogger logger, Type type, ITypeNameR typeNameR, bool throwExceptions = false, bool selfLog = false)
    {
        try
        {
            var callerName = typeNameR.ExtractReadable(type);

            return logger.ForContext(Core.Constants.SourceContextPropertyName, callerName);
        }
        catch (Exception exception)
        {
            if (selfLog)
            {
                logger.Verbose(exception, "Readable name of type could not be determined");
            }

            if (throwExceptions)
            {
                throw;
            }
        }

        return logger;
    }
}
