using JanHafner.TypeNameExtractor;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Serilog.Extensions.TypeNameExtractor;

/// <summary>
/// Defines extensions methods for Serilogs <see cref="ILogger"/>.
/// </summary>
public static class SerilogTypeNameExtractorExtensions
{
    /// <summary>
    /// Defines the Serilog property name of the SourceContext
    /// </summary>
    public const string SourceContext = "SourceContext";

    private static ITypeNameExtractor defaultTypeNameExtractor = new JanHafner.TypeNameExtractor.TypeNameExtractor();

    /// <summary>
    /// Gets the default <see cref="ITypeNameExtractor"/> used by extension methods in this class if no custom <see cref="ITypeNameExtractor"/> is supplied.
    /// </summary>
    public static ITypeNameExtractor DefaultTypeNameExtractor => SerilogTypeNameExtractorExtensions.defaultTypeNameExtractor;

    /// <summary>
    /// Sets the supplied <see cref="ITypeNameExtractor"/> as default for <see cref="DefaultTypeNameExtractor"/>.
    /// </summary>
    /// <param name="typeNameExtractor">The <see cref="ITypeNameExtractor"/> used as default.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="typeNameExtractor"/> is <c>null</c>.</exception>
    public static void SetDefaultTypeNameExtractor(ITypeNameExtractor typeNameExtractor)
    {
        if (typeNameExtractor is null)
        {
            throw new ArgumentNullException(nameof(typeNameExtractor));
        }

        SerilogTypeNameExtractorExtensions.defaultTypeNameExtractor = typeNameExtractor;
    }

    /// <summary>
    /// Uses the human readable name of the <see cref="Type"/> defined by <paramref name="instance"/> as SourceContext.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> defined by <paramref name="instance"/>.</typeparam>
    /// <param name="logger">An instance of the <see cref="ILogger"/> provided by Serilog.</param>
    /// <param name="instance">The instance from which the human readable type name should be used as SourceContext.</param>
    /// <param name="typeNameExtractor">The <see cref="ITypeNameExtractor"/> used to extract the human readable name.</param>
    /// <param name="throwExceptions">If set to <c>true</c> methods in this class will throw exceptions.</param>
    /// <param name="selfLog">If set to <c>true</c> minimal loggin on verbose will also take action.</param>
    /// <returns>The <see cref="ILogger"/> instance after the call to <see cref="ILogger.ForContext(string, object, bool)"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="typeNameExtractor"/> is set to <c>true</c>, exceptions will be thrown if <paramref name="logger"/> or <paramref name="instance"/> are <c>null</c>.</exception>
    public static ILogger ForMe<T>(this ILogger logger, T instance, ITypeNameExtractor? typeNameExtractor = null, bool throwExceptions = false, bool selfLog = false)
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

        return logger.ForTypeCore(instance.GetType(), typeNameExtractor, throwExceptions, selfLog);
    }

    /// <summary>
    /// Uses the human readable name of the <see cref="Type"/> retrieved via <c>StackFrame(1).GetMethod().DeclaringType</c>.
    /// </summary>
    /// <param name="logger">An instance of the <see cref="ILogger"/> provided by Serilog.</param>
    /// <param name="typeNameExtractor">The <see cref="ITypeNameExtractor"/> used to extract the human readable name.</param>
    /// <param name="throwExceptions">If set to <c>true</c> methods in this class will throw exceptions.</param>
    /// <param name="selfLog">If set to <c>true</c> minimal loggin on verbose will also take action.</param>
    /// <returns>The <see cref="ILogger"/> instance after the call to <see cref="ILogger.ForContext(string, object, bool)"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="typeNameExtractor"/> is set to <c>true</c>, exceptions will be thrown if <paramref name="logger"/> is <c>null</c> or the type could not be retrived from the parent stack frame.</exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static ILogger ForMe(this ILogger logger, ITypeNameExtractor? typeNameExtractor = null, bool throwExceptions = false, bool selfLog = false)
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

        return logger.ForTypeCore(type, typeNameExtractor, throwExceptions, selfLog);
    }

    private static ILogger ForTypeCore(this ILogger logger, Type type, ITypeNameExtractor? typeNameExtractor = null, bool throwExceptions = false, bool selfLog = false)
    {
        try
        {
            var callerName = (typeNameExtractor ?? SerilogTypeNameExtractorExtensions.DefaultTypeNameExtractor).ExtractReadableName(type).Name;

            return logger.ForContext(SerilogTypeNameExtractorExtensions.SourceContext, callerName);
        }
        catch (Exception exception)
        {
            if (selfLog)
            {
                logger.Verbose(exception, "Human readable name of type could not be determined");
            }

            if (throwExceptions)
            {
                throw;
            }
        }

        return logger;
    }
}
