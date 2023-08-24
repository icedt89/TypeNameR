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
    /// The name
    /// </summary>
    public const string ExceptionDisplayPropertyName = "ExceptionDisplay";

    private readonly ITypeNameR typeNameR;

    private readonly NameRControlFlags nameRControlFlags;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionDisplayEnricher"/> class.
    /// </summary>
    /// <param name="typeNameR">The <see cref="ITypeNameR"/> to use.</param>
    /// <param name="nameRControlFlags">The <see cref="NameRControlFlags"/> to use.</param>
    public ExceptionDisplayEnricher(ITypeNameR typeNameR, NameRControlFlags nameRControlFlags)
    {
        this.typeNameR = typeNameR;
        this.nameRControlFlags = nameRControlFlags;
    }

    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (logEvent.Exception is null)
        {
            return;
        }

        var exceptionDisplay = typeNameR.GenerateDisplay(logEvent.Exception, nameRControlFlags);

        var logEventProperty = propertyFactory.CreateProperty(ExceptionDisplayPropertyName, exceptionDisplay);

        logEvent.AddPropertyIfAbsent(logEventProperty);
    }
}