using FluentAssertions;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using JanHafner.TypeNameR.Helper;
using NSubstitute;
using System.Diagnostics;
using System.Reflection;
using Xunit;

namespace JanHafner.TypeNameR.Tests.StackTraceHelperTests;

public sealed class ProcessStackFrames
{
    [Fact]
    public void FlattensRecursiveStackFrames()
    {
        try
        {
            // Arrange
            StackTraceGenerator.RecursiveCall(10);

            Assert.Fail("That should not happen");
        }
        catch (StackOverflowException stackOverflowException)
        {
            var stackTrace = new System.Diagnostics.StackTrace(stackOverflowException, fNeedFileInfo: false);

            // Act
            var stackFrames = stackTrace.EnumerateRecursiveStackFrames(includeHiddenStackFrames: true).ToArray();

            // Assert
            stackFrames.Should().HaveCount(8);

            var stackFrame1 = stackFrames[0];
            stackFrame1.CallDepth.Should().Be(2);
            stackFrame1.Method.Should().Be(typeof(StackTraceGenerator).GetMethodOrThrow(nameof(StackTraceGenerator.RecursiveCallCore)));

            var stackFrame2 = stackFrames[1];
            stackFrame2.CallDepth.Should().Be(1);
            stackFrame2.Method.Should().Be(typeof(StackTraceGenerator).GetMethodOrThrow(nameof(StackTraceGenerator.RecursiveSubCall)));

            var stackFrame3 = stackFrames[2];
            stackFrame3.CallDepth.Should().Be(3);
            stackFrame3.Method.Should().Be(typeof(StackTraceGenerator).GetMethodOrThrow(nameof(StackTraceGenerator.RecursiveCallCore)));

            var stackFrame4 = stackFrames[3];
            stackFrame4.CallDepth.Should().Be(1);
            stackFrame4.Method.Should().Be(typeof(StackTraceGenerator).GetMethodOrThrow(nameof(StackTraceGenerator.RecursiveSubCall)));

            var stackFrame5 = stackFrames[4];
            stackFrame5.CallDepth.Should().Be(3);
            stackFrame5.Method.Should().Be(typeof(StackTraceGenerator).GetMethodOrThrow(nameof(StackTraceGenerator.RecursiveCallCore)));

            var stackFrame6 = stackFrames[5];
            stackFrame6.CallDepth.Should().Be(1);
            stackFrame6.Method.Should().Be(typeof(StackTraceGenerator).GetMethodOrThrow(nameof(StackTraceGenerator.RecursiveSubCall)));

            var stackFrame7 = stackFrames[6];
            stackFrame7.CallDepth.Should().Be(2);
            stackFrame7.Method.Should().Be(typeof(StackTraceGenerator).GetMethodOrThrow(nameof(StackTraceGenerator.RecursiveCallCore)));

            var stackFrame8 = stackFrames[7];
            stackFrame8.CallDepth.Should().Be(1);
            stackFrame8.Method.Should().Be(typeof(StackTraceGenerator).GetMethodOrThrow(nameof(StackTraceGenerator.RecursiveCall)));
        }
    }

    [Fact]
    public void SkipsTheStackFrameIfItHasSkippableMethod()
    {
        // Arrange
        var stackFrameMethod = Substitute.For<MethodBase>();
        stackFrameMethod.IsDefined(typeof(StackTraceHiddenAttribute), inherit: false).Returns(returnThis: true);

        var stackFrame = Substitute.For<StackFrame>();
        stackFrame.GetMethod().Returns(stackFrameMethod);

        var stackTrace = Substitute.For<System.Diagnostics.StackTrace>();
        stackTrace.GetFrame(index: 0).Returns(stackFrame);
        stackTrace.FrameCount.Returns(returnThis: 1);

        // Act
        var flattenedStackFrames = stackTrace.EnumerateRecursiveStackFrames(includeHiddenStackFrames: false).ToArray();

        // Assert
        flattenedStackFrames.Should().BeEmpty();
    }
}