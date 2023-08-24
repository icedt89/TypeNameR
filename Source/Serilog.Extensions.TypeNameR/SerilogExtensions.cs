using JanHafner.TypeNameR;
using Serilog.Configuration;

namespace Serilog.Extensions.TypeNameR;

/// <summary>
/// Defines extension methods for Serilog <see cref="ILogger"/>.
/// </summary>
public static class SerilogExtensions
{
    /// <summary>
    /// Uses the readable name of the <see cref="Type"/> defined by <paramref name="instance"/> as SourceContext.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> defined by <paramref name="instance"/>.</typeparam>
    /// <param name="logger">An instance of the <see cref="ILogger"/> provided by Serilog.</param>
    /// <param name="instance">The instance from which the readable type name should be used as SourceContext.</param>
    /// <param name="typeNameR">The <see cref="ITypeNameR"/> instance to use. If <see langword="null"/>, StaticTypeNameR.Instance will be used instead.</param>
    /// <param name="fullTypeName">Controls whether the full <see cref="Type"/> name should be used.</param>
    /// <param name="throwExceptions">If set to <see langword="true"/> methods in this class will throw exceptions.</param>
    /// <param name="selfLog">If set to <see langword="true"/> minimal logging on error will also take action.</param>
    /// <returns>The <see cref="ILogger"/> instance after the call to <see cref="ILogger.ForContext(string, object, bool)"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="throwExceptions"/> is set to <see langword="true"/>, exceptions will be thrown if <paramref name="logger"/> or <paramref name="instance"/> are <see langword="null"/>.</exception>
    public static ILogger For<T>(this ILogger logger, T instance, ITypeNameR? typeNameR = null, bool fullTypeName = false, bool throwExceptions = false,
        bool selfLog = false)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(instance);

        return logger.ForTypeCore(instance.GetType(), typeNameR ?? StaticTypeNameR.Instance, fullTypeName, throwExceptions, selfLog);
    }

    /// <summary>
    /// Uses the readable name of the supplied <see cref="Type"/>.
    /// </summary>
    /// <param name="logger">An instance of the <see cref="ILogger"/> provided by Serilog.</param>
    /// <param name="type">The <see cref="Type"/>.</param>
    /// <param name="typeNameR">The <see cref="ITypeNameR"/> instance to use. If <see langword="null"/>, StaticTypeNameR.Instance will be used instead.</param>
    /// <param name="fullTypeName">Controls whether the full <see cref="Type"/> name should be used.</param>
    /// <param name="throwExceptions">If set to <see langword="true"/> methods in this class will throw exceptions.</param>
    /// <param name="selfLog">If set to <see langword="true"/> minimal logging on error will also take action.</param>
    /// <returns>The <see cref="ILogger"/> instance after the call to <see cref="ILogger.ForContext(string, object, bool)"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="throwExceptions"/> is set to <see langword="true"/>, exceptions will be thrown if <paramref name="logger"/> is <see langword="null"/> or the type could not be retrieved from the parent stack frame.</exception>
    public static ILogger For(this ILogger logger, Type type, ITypeNameR? typeNameR = null, bool fullTypeName = false, bool throwExceptions = false,
        bool selfLog = false)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(type);

        return logger.ForTypeCore(type, typeNameR ?? StaticTypeNameR.Instance, fullTypeName, throwExceptions, selfLog);
    }

    private static ILogger ForTypeCore(this ILogger logger, Type type, ITypeNameR typeNameR, bool fullTypeName, bool throwExceptions, bool selfLog)
    {
        try
        {
            var typeDisplay = typeNameR.GenerateDisplay(type, fullTypeName, nullabilityInfo: null);

            return logger.ForContext(Core.Constants.SourceContextPropertyName, typeDisplay);
        }
        catch (Exception exception)
        {
            if (selfLog)
            {
                logger.Verbose(exception, "Display name of type could not be generated: {TypeFullName}", type.FullName);
            }

            if (throwExceptions)
            {
                throw;
            }
        }

        return logger.ForContext(type);
    }
    
    /// <summary>
    /// Enriches the <see cref="LoggerConfiguration"/> with the <see cref="ExceptionDisplayEnricher"/> using the supplied <see cref="ITypeNameR"/> and <see cref="NameRControlFlags"/>.
    /// </summary>
    /// <param name="configuration">The <see cref="LoggerEnrichmentConfiguration"/>.</param>
    /// <param name="typeNameR">The <see cref="ITypeNameR"/> instance to use. If <see langword="null"/>, StaticTypeNameR.Instance will be used instead.</param>
    /// <param name="nameRControlFlags">The <see cref="NameRControlFlags"/> to use.</param>
    /// <returns>The <see cref="LoggerConfiguration"/> which can be used for further configuration.</returns>
    public static LoggerConfiguration WithExceptionDisplay(this LoggerEnrichmentConfiguration configuration, ITypeNameR? typeNameR = null,
        NameRControlFlags nameRControlFlags = NameRControlFlags.All)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var exceptionDisplayEnricher = new ExceptionDisplayEnricher(typeNameR ?? StaticTypeNameR.Instance, nameRControlFlags);

        return configuration.With(exceptionDisplayEnricher);
    }
}