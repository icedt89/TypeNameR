using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using NSubstitute;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AttributeHelperTests;

public sealed class FindStateMachineAttribute
{
    [Fact]
    public void ReturnsTheStateMachineAttribute()
    {
        // Arrange
        var stateMachineAttribute = Substitute.For<StateMachineAttribute>(typeof(object));

        // HINT: Using collection initializer feature will fail this test
        var method = Substitute.For<MethodInfo>();
        method.GetCustomAttributes(typeof(StateMachineAttribute), false).Returns(new[] { stateMachineAttribute });

        // Act
        var returnedStateMachineAttribute = method.FindStateMachineAttribute();

        // Assert
        returnedStateMachineAttribute.Should().BeSameAs(stateMachineAttribute);
    }

    [Fact]
    public void ReturnsNull()
    {
        // Arrange
        var method = Substitute.For<MethodInfo>();
        method.GetCustomAttributes(typeof(StateMachineAttribute), false).Returns(Array.Empty<Attribute>());

        // Act
        var stateMachineAttribute = method.FindStateMachineAttribute();

        // Assert
        stateMachineAttribute.Should().BeNull();
    }
}