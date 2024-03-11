using FluentAssertions;
using JanHafner.TypeNameR.StackTrace;
using System.Diagnostics;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.ExceptionStackTraceManipulatorTests;

public sealed class SetStackTrace
{
    private Exception exception = new();

    private StringBuilder stackTrace = new();

    private bool storeOriginalStackTrace;

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
    public void ThrowsArgumentNullExceptionIfStringBuilderIsNull()
    {
        // Arrange
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        stackTrace = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        // Act, Assert
        var argumentNullException = Assert.Throws<ArgumentNullException>(() => Call());
        argumentNullException.ParamName.Should().Be(nameof(stackTrace));
    }

    [Fact]
    public void SetTheStacktraceWithoutStoringTheOriginal()
    {
        // Arrange
        var content = Guid.NewGuid().ToString();
        stackTrace.Append(content);

        // Act
        Call();

        // Assert
        exception.Data.Keys.Count.Should().Be(0);
        exception.StackTrace.Should().Be(content);
    }

    [Fact]
    public void SetTheStacktraceWithStoringTheOriginal()
    {
        // Arrange
        storeOriginalStackTrace = true;

        var content = Guid.NewGuid().ToString();
        stackTrace.Append(content);

        // Act
        Call();

        // Assert
        exception.StackTrace.Should().Be(content);
        exception.Data.Contains(ExceptionStackTraceManipulator.OriginalStackTraceKey).Should().BeTrue();
    }

    [DebuggerStepThrough]
    private void Call() => exception.SetStackTrace(stackTrace, storeOriginalStackTrace);
}