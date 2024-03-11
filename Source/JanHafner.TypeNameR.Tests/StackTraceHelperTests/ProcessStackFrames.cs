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
            var stackTrace = new System.Diagnostics.StackTrace(stackOverflowException, false);
            var originalStackFrames = stackTrace.GetFrames();

            // Act
            var stackFrames =
                stackTrace.ProcessStackFrames(NameRControlFlags.None);

            // Assert
            stackFrames.Should().HaveCount(4);

            var nonRecursiveStackFrame1 = stackFrames[0];
            nonRecursiveStackFrame1.CallDepth.Should().Be(10);
            nonRecursiveStackFrame1.Method.Should().Be(typeof(StackTraceGenerator).GetMethodOrThrow(nameof(StackTraceGenerator.RecursiveCallCore)));
            originalStackFrames.Count(sf => sf.GetMethod() == nonRecursiveStackFrame1.StackFrame.GetMethod()).Should().Be(10);

            var nonRecursiveStackFrame2 = stackFrames[1];
            nonRecursiveStackFrame2.CallDepth.Should().Be(9);
            nonRecursiveStackFrame2.Method.Should().Be(typeof(StackTraceGenerator).GetMethodOrThrow(nameof(StackTraceGenerator.RecursiveSubCall)));
            originalStackFrames.Count(sf => sf.GetMethod() == nonRecursiveStackFrame2.StackFrame.GetMethod()).Should().Be(9);

            var nonRecursiveStackFrame3 = stackFrames[2];
            nonRecursiveStackFrame3.CallDepth.Should().Be(1);
            nonRecursiveStackFrame3.Method.Should().Be(typeof(StackTraceGenerator).GetMethodOrThrow(nameof(StackTraceGenerator.RecursiveCall)));
            originalStackFrames.Count(sf => sf.GetMethod() == nonRecursiveStackFrame3.StackFrame.GetMethod()).Should().Be(1);

            var nonRecursiveStackFrame4 = stackFrames[3];
            nonRecursiveStackFrame4.CallDepth.Should().Be(1);
            nonRecursiveStackFrame4.Method.Should()
                .Be(typeof(ProcessStackFrames).GetMethodOrThrow(nameof(FlattensRecursiveStackFrames)));
            originalStackFrames.Count(sf => sf.GetMethod() == nonRecursiveStackFrame3.StackFrame.GetMethod()).Should().Be(1);
        }
    }

    [Fact]
    public void DoesNotFlattenRecursiveStackFrames()
    {
        try
        {
            // Arrange
            StackTraceGenerator.RecursiveCall(10);

            Assert.Fail("That should not happen");
        }
        catch (StackOverflowException stackOverflowException)
        {
            var stackTrace = new System.Diagnostics.StackTrace(stackOverflowException, false);
            var originalStackFrames = stackTrace.GetFrames();

            // Act
            var stackFrames =
                stackTrace.ProcessStackFrames(NameRControlFlags.DontEliminateRecursiveStackFrames);

            // Assert
            stackFrames.Should().HaveCount(originalStackFrames.Length);
        }
    }

    [Fact]
    public void SkipsTheStackFrameIfItHasSkippableMethod()
    {
        // Arrange
        var stackFrameMethod = Substitute.For<MethodBase>();
        stackFrameMethod.IsDefined(typeof(StackTraceHiddenAttribute), false).Returns(true);

        var stackFrame = Substitute.For<StackFrame>();
        stackFrame.GetMethod().Returns(stackFrameMethod);

        var stackTrace = Substitute.For<System.Diagnostics.StackTrace>();
        stackTrace.GetFrame(0).Returns(stackFrame);
        stackTrace.FrameCount.Returns(1);

        // Act
        var flattenedStackFrames = stackTrace.ProcessStackFrames(NameRControlFlags.None);

        // Assert
        flattenedStackFrames.Should().BeEmpty();
    }
}