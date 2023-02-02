using FluentAssertions;
using System.Diagnostics;
using Xunit;

namespace JanHafner.TypeNameR.Tests.ExceptionStackTraceManipulatorTests;

public sealed class GetOriginalStackTraceOrNull
{
    private Exception exception;

    public GetOriginalStackTraceOrNull()
    {
        exception = new Exception();
    }

    [Fact]
    public void ReturnsNullIfOriginalStacktraceIsNotAvailable()
    {
        // Act
        var result = Call();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ReturnsTheOriginalStackTrace()
    {
        // Arrange
        var expected = Guid.NewGuid().ToString();

        exception.Data.Add(ExceptionStackTraceManipulator.OriginalStackTraceKey, expected);

        // Act
        var result = Call();

        // Assert
        result.Should().Be(expected);
    }

    [DebuggerStepThrough]
    private string? Call() => exception.GetOriginalStackTraceOrNull();
}
