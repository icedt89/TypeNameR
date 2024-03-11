using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using Xunit;

namespace JanHafner.TypeNameR.Tests.TypeHelperTests;

public sealed class IsGenericValueTuple
{
    [Fact]
    public void IsTrueForExplicitGenericValueTuple()
    {
        // Arrange
        var theObject = new ValueTuple<string, int>("", 0);
        var type = theObject.GetType();

        // Act
        var isValueTuple = type.IsGenericValueTuple();

        // Assert
        isValueTuple.Should().BeTrue();
    }

    [Fact]
    public void IsTrueForImplicitGenericValueTuple()
    {
        // Arrange
        var theObject = ("", 0);
        var type = theObject.GetType();

        // Act
        var isValueTuple = type.IsGenericValueTuple();

        // Assert
        isValueTuple.Should().BeTrue();
    }

    [Fact]
    public void IsFalseForGenericTuple()
    {
        // Arrange
        var theObject = new Tuple<string, int>("", 0);
        var type = theObject.GetType();

        // Act
        var isValueTuple = type.IsGenericValueTuple();

        // Assert
        isValueTuple.Should().BeFalse();
    }

    [Fact]
    public void IsFalseForValueTuple()
    {
        // Arrange
        var theObject = new ValueTuple();
        var type = theObject.GetType();

        // Act
        var isValueTuple = type.IsGenericValueTuple();

        // Assert
        isValueTuple.Should().BeFalse();
    }
}