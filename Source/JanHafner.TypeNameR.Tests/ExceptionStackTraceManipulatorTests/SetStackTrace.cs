using FluentAssertions;
using System.Diagnostics;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.ExceptionStackTraceManipulatorTests;

public sealed class SetStackTrace
{
    private Exception exception;

    private StringBuilder stackTrace;

    private bool storeOriginalStackTrace;

    public SetStackTrace()
    {
        exception = new();
        stackTrace = new();
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
