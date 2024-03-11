using FluentAssertions;
using JanHafner.TypeNameR.StackTrace;
using System.Diagnostics;
using Xunit;

namespace JanHafner.TypeNameR.Tests.ExceptionStackTraceManipulatorTests;

public sealed class TryRestoreOriginalStackTrace
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
    public void ReturnsFalseIfOriginalStacktraceIsNotStored()
    {
        // Act
        var result = Call();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrueAndRestoresTheOriginalStackTrace()
    {
        // Arrange
        var originalStackTrace = Guid.NewGuid().ToString();

        exception.Data[ExceptionStackTraceManipulator.OriginalStackTraceKey] = originalStackTrace;

        // Act
        var result = Call();

        // Assert
        result.Should().BeTrue();
        exception.Data.Keys.Count.Should().Be(0);
        exception.StackTrace.Should().Be(originalStackTrace);
    }

    [DebuggerStepThrough]
    private bool Call() => exception.TryRestoreOriginalStackTrace();
}