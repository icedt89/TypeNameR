using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using JanHafner.TypeNameR;
using NSubstitute;
using Serilog.Core;
using Serilog.Events;
using Serilog.Parsing;
using System.Diagnostics;
using Xunit;

namespace Serilog.Extensions.TypeNameR.Tests.ExceptionDisplayEnricherTests;

public sealed class Enrich
{
    private readonly IFixture fixture;

    private readonly ITypeNameR typeNameRSubstitute;

    private readonly ILogEventPropertyFactory logEventPropertyFactorySubstitute;

    private readonly ExceptionDisplayEnricher exceptionDisplayEnricher;

    private LogEvent logEvent;

    public Enrich()
    {
        fixture = new Fixture();
        fixture.Customize(new AutoNSubstituteCustomization());

        typeNameRSubstitute = Substitute.For<ITypeNameR>();
        fixture.Inject(typeNameRSubstitute);

        fixture.Inject(NameRControlFlags.All);

        fixture.Inject(ExceptionDisplayEnricher.DefaultExceptionDisplayPropertyName);

        logEventPropertyFactorySubstitute = Substitute.For<ILogEventPropertyFactory>();

        exceptionDisplayEnricher = fixture.Create<ExceptionDisplayEnricher>();
    }

    [Fact]
    public void DoesNotEnrichIfThereIsNoExceptionLogged()
    {
        // Arrange
        logEvent = new LogEvent(
            DateTimeOffset.Now,
            LogEventLevel.Error,
            null,
            new MessageTemplate(string.Empty, Enumerable.Empty<MessageTemplateToken>()),
            Array.Empty<LogEventProperty>());

        // Act
        Call();

        // Assert
        logEvent.Properties.Should().NotContainKey(ExceptionDisplayEnricher.DefaultExceptionDisplayPropertyName);

        logEventPropertyFactorySubstitute.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public void AddsTheExceptionDisplayPropertyToTheLogEvent()
    {
        // Arrange
        var exception = new Exception();

        logEvent = new LogEvent(
            DateTimeOffset.Now,
            LogEventLevel.Error,
            exception,
            new MessageTemplate(string.Empty, Enumerable.Empty<MessageTemplateToken>()),
            Array.Empty<LogEventProperty>());

        var exceptionDisplay = fixture.Create<string>();

        typeNameRSubstitute.GenerateDisplay(exception, NameRControlFlags.All).Returns(exceptionDisplay);

        var logEventProperty = new LogEventProperty(
            ExceptionDisplayEnricher.DefaultExceptionDisplayPropertyName,
            new ScalarValue(exceptionDisplay)
        );

        logEventPropertyFactorySubstitute.CreateProperty(
                ExceptionDisplayEnricher.DefaultExceptionDisplayPropertyName,
                exceptionDisplay,
                false).Returns(logEventProperty);

        // Act
        Call();

        // Assert
        logEvent.Properties.Should()
            .ContainKey(ExceptionDisplayEnricher.DefaultExceptionDisplayPropertyName)
            .WhoseValue.ToString().Should().Contain(exceptionDisplay);
    }

    [DebuggerStepThrough]
    private void Call() => exceptionDisplayEnricher.Enrich(logEvent, logEventPropertyFactorySubstitute);
}