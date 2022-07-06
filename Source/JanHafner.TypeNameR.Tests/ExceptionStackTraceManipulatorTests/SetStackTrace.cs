using FluentAssertions;
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
        this.exception = new();
        this.stackTrace = new();
    }

    [Fact]
    public void SetTheStacktraceWithoutStoringTheOriginal()
    {
        // Arrange
        var content = Guid.NewGuid().ToString();
        this.stackTrace.Append(content);

        // Act
        this.Call();

        // Assert
        this.exception.StackTrace.Should().Be(content);
    }

    [Fact]
    public void SetTheStacktraceWithStoringTheOriginal()
    {
        // Arrange
        this.storeOriginalStackTrace = true;

        var content = Guid.NewGuid().ToString();
        this.stackTrace.Append(content);

        // Act
        this.Call();

        // Assert
        this.exception.StackTrace.Should().Be(content);
        this.exception.Data.Contains(ExceptionStackTraceManipulator.OriginalStackTraceKey).Should().BeTrue();
    }

    private void Call()
    {
        this.exception.SetStackTrace(this.stackTrace, this.storeOriginalStackTrace);
    }
}
