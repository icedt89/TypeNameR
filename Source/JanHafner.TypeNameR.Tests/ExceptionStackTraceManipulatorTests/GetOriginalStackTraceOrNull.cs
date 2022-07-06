using FluentAssertions;
using Xunit;

namespace JanHafner.TypeNameR.Tests.ExceptionStackTraceManipulatorTests;

public sealed class GetOriginalStackTraceOrNull
{
    private Exception exception;

    public GetOriginalStackTraceOrNull()
    {
        this.exception = new Exception();
    }

    [Fact]
    public void ReturnsNullIfOriginalStacktraceIsNotAvailable()
    {
        // Act
        var result = this.Call();

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
        var result = this.Call();

        // Assert
        result.Should().Be(expected);
    }

    private string? Call()
    {
        return this.exception.GetOriginalStackTraceOrNull();
    }
}
