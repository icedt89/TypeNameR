using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using Xunit;

namespace JanHafner.TypeNameR.Tests.TypeHelperTests;

public sealed class IsGenericValueTuple
{
    [Fact]
    public void IsTrueForExplicitGenericValueTupleT1()
    {
        // Arrange
        var theObject = new ValueTuple<string>("");
        var type = theObject.GetType();

        // Act
        var isValueTuple = type.IsGenericValueTuple();

        // Assert
        isValueTuple.Should().BeTrue();
    }
    
    [Fact]
    public void IsTrueForExplicitGenericValueTupleT2()
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
    public void IsTrueForExplicitGenericValueTupleT3()
    {
        // Arrange
        var theObject = new ValueTuple<string, int, bool>("", 0, false);
        var type = theObject.GetType();

        // Act
        var isValueTuple = type.IsGenericValueTuple();

        // Assert
        isValueTuple.Should().BeTrue();
    }
    
    [Fact]
    public void IsTrueForImplicitGenericValueTupleT2()
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
    public void IsTrueForImplicitGenericValueTupleT3()
    {
        // Arrange
        var theObject = ("", 0, false);
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