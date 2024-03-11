using JanHafner.TypeNameR;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Extensions.TypeNameR;

/// <summary>
/// Defines a <see cref="ILogEventEnricher"/> which generates a display of any occured <see cref="Exception"/> using a <see cref="ITypeNameR"/>.
/// </summary>
public sealed class ExceptionDisplayEnricher : ILogEventEnricher
{
    /// <summary>
    /// The default name of the log property.
    /// </summary>
    public const string DefaultExceptionDisplayPropertyName = "ExceptionDisplay";

    private readonly ITypeNameR typeNameR;

    private readonly NameRControlFlags nameRControlFlags;

    private readonly string exceptionDisplayPropertyName;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionDisplayEnricher"/> class.
    /// </summary>
    /// <param name="typeNameR">The <see cref="ITypeNameR"/> to use.</param>
    /// <param name="nameRControlFlags">The <see cref="NameRControlFlags"/> to use.</param>
    /// <param name="exceptionDisplayPropertyName">The name of the log property.</param>
    public ExceptionDisplayEnricher(ITypeNameR typeNameR, NameRControlFlags nameRControlFlags, string exceptionDisplayPropertyName = DefaultExceptionDisplayPropertyName)
    {
        this.typeNameR = typeNameR;
        this.nameRControlFlags = nameRControlFlags;
        this.exceptionDisplayPropertyName = exceptionDisplayPropertyName;
    }

    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (logEvent.Exception is null)
        {
            return;
        }

        var exceptionDisplay = typeNameR.GenerateDisplay(logEvent.Exception, nameRControlFlags);

        var logEventProperty = propertyFactory.CreateProperty(exceptionDisplayPropertyName, exceptionDisplay);

        logEvent.AddPropertyIfAbsent(logEventProperty);
    }
}