using FluentAssertions;
using Xunit;

namespace JanHafner.TypeNameR.Tests.TypeHelperTests;

public sealed class RemoveGenericParametersCount
{
    [Fact]
    public void ReturnsTheTypeNameIfNoDelimiterIsPresent()
    {
        // Arrange
        const string typeName = "TestClass";

        // Act
        var resultingTypeName = TypeHelper.RemoveGenericParametersCount(typeName);

        // Assert
        resultingTypeName.ToString().Should().Be(typeName);
    }

    [Fact]
    public void ReturnsTheTypeNameIfTheDelimiterIsPresent()
    {
        // Arrange
        const string typeName = "TestClass`2";

        // Act
        var resultingTypeName = TypeHelper.RemoveGenericParametersCount(typeName);

        // Assert
        resultingTypeName.ToString().Should().Be("TestClass");
    }
}
