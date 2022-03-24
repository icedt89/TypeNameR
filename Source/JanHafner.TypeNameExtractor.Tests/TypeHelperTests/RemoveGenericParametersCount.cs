using FluentAssertions;
using System;
using Xunit;

namespace JanHafner.TypeNameExtractor.Tests.TypeHelperTests;

public sealed class RemoveGenericParametersCount
{
    [Theory]
    [InlineData("")]
    [InlineData((string?)null)]
    [InlineData("  ")]
    public void ThrowsArgumentExceptionIfTheSuppliedTypeNameIsNullOrWhiteSpace(string? typeName)
    {
        // Act, Assert
        Assert.Throws<ArgumentException>(() => TypeHelper.RemoveGenericParametersCount(typeName!));
    }

    [Fact]
    public void ReturnsTheTypeNameIfNoDelimiterIsPresent()
    {
        // Arrange
        const string typeName = "TestClass";

        // Act
        var resultingTypeName = TypeHelper.RemoveGenericParametersCount(typeName);

        // Assert
        resultingTypeName.Should().Be(typeName);
    }

    [Fact]
    public void ReturnsTheTypeNameIfTheDelimiterIsPresent()
    {
        // Arrange
        const string typeName = "TestClass`2";

        // Act
        var resultingTypeName = TypeHelper.RemoveGenericParametersCount(typeName);

        // Assert
        resultingTypeName.Should().Be("TestClass");
    }
}
