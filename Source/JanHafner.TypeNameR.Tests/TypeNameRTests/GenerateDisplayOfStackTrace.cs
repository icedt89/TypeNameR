using AwesomeAssertions;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using Xunit;

namespace JanHafner.TypeNameR.Tests.TypeNameRTests
{
    public sealed class GenerateDisplayOfStackTrace
    {
        [Fact]
        public void GenerateStackTraceDisplay()
        {
            // Arrange
            System.Diagnostics.StackTrace stackTrace = null!;
            try
            {
                StackTraceGenerator.CallRecursiveGenericMethodWithAsyncStateMachineAttributeAsync<int>();
            }
            catch (StackOverflowException stackOverflowException)
            {
                stackTrace = new System.Diagnostics.StackTrace(stackOverflowException, true);
            }

            var typeNameR = GlobalTestSettings.TypeNameR ?? new TypeNameR();

            // Act
            var generated = typeNameR.GenerateDisplay(stackTrace, NameRControlFlags.All);

            // Assert
            generated.Should().Be(
                @"  at public static Task<TResult?> JanHafner.TypeNameR.BenchmarkAndTestUtils.StackTraceGenerator.CallRecursiveGenericMethodAsync<TResult>(int? recursionDepth = 1, int stopAt = 10) x 9 in X:\icedt89\TypeNameR\Source\JanHafner.TypeNameR.BenchmarkAndTestUtils\StackTraceGenerator.cs:line 37:13
  at public static async Task<TResult?> JanHafner.TypeNameR.BenchmarkAndTestUtils.StackTraceGenerator.CallRecursiveGenericMethodWithAsyncStateMachineAttributeAsync<TResult>(int? recursionDepth = 1, int stopAt = 10) in X:\icedt89\TypeNameR\Source\JanHafner.TypeNameR.BenchmarkAndTestUtils\StackTraceGenerator.cs:line 52:9");
        }
    }
}