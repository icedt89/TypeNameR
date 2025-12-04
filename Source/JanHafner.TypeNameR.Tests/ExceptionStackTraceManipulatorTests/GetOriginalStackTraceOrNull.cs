using AwesomeAssertions;
using JanHafner.TypeNameR.StackTrace;
using System.Diagnostics;
using Xunit;

namespace JanHafner.TypeNameR.Tests.ExceptionStackTraceManipulatorTests;

public sealed class GetOriginalStackTraceOrNull
{
    private Exception exception = new();

    [Fact]
    public void ThrowsArgumentNullExceptionIfExceptionIsNull()
    {
        // Arrange
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        exception = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        // Act, Assert
        var argumentNullException = Assert.Throws<ArgumentNullException>(() => Call());
        argumentNullException.ParamName.Should().Be(nameof(exception));
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