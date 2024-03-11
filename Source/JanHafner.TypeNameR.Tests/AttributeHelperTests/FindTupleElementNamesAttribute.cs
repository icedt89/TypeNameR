using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using NSubstitute;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AttributeHelperTests;

public sealed class FindTupleElementNamesAttribute
{
    [Fact]
    public void ReturnsTheStateMachineAttribute()
    {
        // Arrange
        var tupleElementNamesAttribute = new TupleElementNamesAttribute([]);

        // HINT: Using collection initializer feature will fail this test
        var parameter = Substitute.For<ParameterInfo>();
        parameter.GetCustomAttributes(typeof(TupleElementNamesAttribute), false).Returns(new[] { tupleElementNamesAttribute });

        // Act
        var returnedTupleElementNamesAttribute = parameter.FindTupleElementNamesAttribute();

        // Assert
        returnedTupleElementNamesAttribute.Should().BeSameAs(tupleElementNamesAttribute);
    }

    [Fact]
    public void ReturnsNull()
    {
        // Arrange
        var parameter = Substitute.For<ParameterInfo>();
        parameter.GetCustomAttributes(typeof(TupleElementNamesAttribute), false).Returns(Array.Empty<Attribute>());

        // Act
        var tupleElementNamesAttribute = parameter.FindTupleElementNamesAttribute();

        // Assert
        tupleElementNamesAttribute.Should().BeNull();
    }
}