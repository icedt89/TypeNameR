using FluentAssertions;
using System;
using Xunit;

namespace JanHafner.TypeNameExtractor.Tests.TypeNameExtractorTests
{
    public sealed class RemoveGenericParametersCount
    {

        [Theory]
        [InlineData("")]
        [InlineData((string)null)]
        [InlineData("  ")]
        public void ThrowsArgumentExceptionIfTheSuppliedTypeNameIsNullOrWhiteSpace(string typeName)
        {
            // Act, Assert
            Assert.Throws<ArgumentException>(() => TypeNameExtractor.RemoveGenericParametersCount(typeName));
        }

        [Fact]
        public void ReturnsTheTypeNameIfNoDelimiterIsPresent()
        {
            // Arrange
            const string typeName = "TestClass";

            // Act
            var resultingTypeName = TypeNameExtractor.RemoveGenericParametersCount(typeName);

            // Assert
            resultingTypeName.Should().Be(typeName);
        }

        [Fact]
        public void ReturnsTheTypeNameIfTheDelimiterIsPresent()
        {
            // Arrange
            const string typeName = "TestClass`2";

            // Act
            var resultingTypeName = TypeNameExtractor.RemoveGenericParametersCount(typeName);

            // Assert
            resultingTypeName.Should().Be("TestClass");
        }
    }
}
