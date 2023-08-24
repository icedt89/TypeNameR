using JanHafner.TypeNameR;
using NSubstitute;
using Serilog.Core;
using Serilog.Events;
using Xunit;

namespace Serilog.Extensions.TypeNameR.Tests;

public sealed class EnrichesTheLog
{
    private readonly ILogger logger;

    private readonly ILogEventSink logEventSinkSubstitute;

    public EnrichesTheLog()
    {
        logEventSinkSubstitute = Substitute.For<ILogEventSink>();
        var typeNameRSubstitute = Substitute.For<ITypeNameR>();

        logger = new LoggerConfiguration()
            .Enrich.WithExceptionDisplay(typeNameRSubstitute)
            .WriteTo.Sink(logEventSinkSubstitute)
            .CreateLogger();
    }

    [Fact]
    public void WritesTheExceptionDisplayToTheLog()
    {
        // Arrange
        var exception = new Exception();

        // Act
        logger.Error(exception, "Test");

        // Assert
        logEventSinkSubstitute.Received(1).Emit(Arg.Is<LogEvent>(l
            => l.Exception == exception
               && l.Properties.ContainsKey(ExceptionDisplayEnricher.ExceptionDisplayPropertyName)
        ));
    }
}