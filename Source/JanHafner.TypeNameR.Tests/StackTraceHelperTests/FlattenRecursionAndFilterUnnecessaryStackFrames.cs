using FluentAssertions;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using JanHafner.TypeNameR.Helper;
using NSubstitute;
using System.Diagnostics;
using System.Reflection;
using Xunit;

namespace JanHafner.TypeNameR.Tests.StackTraceHelperTests;

public sealed class FlattenRecursionAndFilterUnnecessaryStackFrames
{
    [Fact]
    public void FlattensRecursiveStackFrames()
    {
        try
        {
            // Arrange
            Methods.RecursiveCall(10);

            Assert.Fail("That should not happen");
        }
        catch (StackOverflowException stackOverflowException)
        {
            var stackTrace = new System.Diagnostics.StackTrace(stackOverflowException, false);
            var originalStackFrames = stackTrace.GetFrames();

            // Act
            var nonRecursiveStackFrames =
                originalStackFrames.FlattenRecursionAndFilterUnnecessaryStackFrames(NameRControlFlags.None, Array.Empty<string>());

            // Assert
            nonRecursiveStackFrames.Should().HaveCount(4);

            var nonRecursiveStackFrame1 = nonRecursiveStackFrames[0];
            nonRecursiveStackFrame1.CallDepth.Should().Be(10);
            nonRecursiveStackFrame1.Method.Should().Be(typeof(Methods).GetMethodOrThrow(nameof(Methods.RecursiveCallCore)));
            originalStackFrames.Count(sf => sf.GetMethod() == nonRecursiveStackFrame1.StackFrame.GetMethod()).Should().Be(10);

            var nonRecursiveStackFrame2 = nonRecursiveStackFrames[1];
            nonRecursiveStackFrame2.CallDepth.Should().Be(9);
            nonRecursiveStackFrame2.Method.Should().Be(typeof(Methods).GetMethodOrThrow(nameof(Methods.RecursiveSubCall)));
            originalStackFrames.Count(sf => sf.GetMethod() == nonRecursiveStackFrame2.StackFrame.GetMethod()).Should().Be(9);

            var nonRecursiveStackFrame3 = nonRecursiveStackFrames[2];
            nonRecursiveStackFrame3.CallDepth.Should().Be(1);
            nonRecursiveStackFrame3.Method.Should().Be(typeof(Methods).GetMethodOrThrow(nameof(Methods.RecursiveCall)));
            originalStackFrames.Count(sf => sf.GetMethod() == nonRecursiveStackFrame3.StackFrame.GetMethod()).Should().Be(1);

            var nonRecursiveStackFrame4 = nonRecursiveStackFrames[3];
            nonRecursiveStackFrame4.CallDepth.Should().Be(1);
            nonRecursiveStackFrame4.Method.Should()
                .Be(typeof(FlattenRecursionAndFilterUnnecessaryStackFrames).GetMethodOrThrow(nameof(FlattensRecursiveStackFrames)));
            originalStackFrames.Count(sf => sf.GetMethod() == nonRecursiveStackFrame3.StackFrame.GetMethod()).Should().Be(1);
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

        // Act
        var flattenedStackFrames = new[] { stackFrame }.FlattenRecursionAndFilterUnnecessaryStackFrames(NameRControlFlags.None, Array.Empty<string>());

        // Assert
        flattenedStackFrames.Should().BeEmpty();
    }

    private static class Methods
    {
        public static void RecursiveCall(int maxRecursionDepth) => RecursiveCallCore(1, maxRecursionDepth);

        public static void RecursiveCallCore(int current, int maxRecursionDepth)
        {
            if (current == maxRecursionDepth)
            {
                throw new StackOverflowException();
            }

            RecursiveSubCall(current, maxRecursionDepth);
        }

        public static void RecursiveSubCall(int current, int maxRecursionDepth) => RecursiveCallCore(current + 1, maxRecursionDepth);
    }
}