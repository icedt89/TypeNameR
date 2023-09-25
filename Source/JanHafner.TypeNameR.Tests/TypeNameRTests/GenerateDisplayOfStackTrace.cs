using FluentAssertions;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using System.Diagnostics;
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
            throw new NotImplementedException();
        }

        [Fact]
        public void GenerateTheSameStackTraceAsDemystifier()
        {
            // Arrange
            var typeNameR = GlobalTestSettings.TypeNameR ?? new TypeNameR();

            try
            {
                StackTraceGenerator.CallRecursiveGenericMethodWithAsyncStateMachineAttributeAsync<int>();
            }
            catch (StackOverflowException stackOverflowException)
            {
                var demystified = stackOverflowException.ToStringDemystified();

                // Act
                var generated = typeNameR.GenerateDisplay(stackOverflowException, NameRControlFlags.All
                                                                                  & ~NameRControlFlags.IncludeAccessModifier
                                                                                  & ~NameRControlFlags.IncludeParameterDefaultValue
                                                                                  & ~NameRControlFlags.IncludeStaticModifier
                                                                                  & ~NameRControlFlags.IncludeAsyncModifier
                                                                                  & ~NameRControlFlags.IncludeNullabilityInfo);

                // Assert
                generated.Should().Be(demystified);
            }
        }
    }
}