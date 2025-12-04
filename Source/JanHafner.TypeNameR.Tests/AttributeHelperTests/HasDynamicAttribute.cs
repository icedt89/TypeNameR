using AwesomeAssertions;
using JanHafner.TypeNameR.Helper;
using NSubstitute;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AttributeHelperTests;

public sealed class HasDynamicAttribute
{
    [Fact]
    public void IsTrue()
    {
        // Arrange
        var parameter = Substitute.For<ParameterInfo>();
        parameter.IsDefined(typeof(DynamicAttribute), false).Returns(true);

        // Act
        var result = parameter.HasDynamicAttribute();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsFalse()
    {
        // Arrange
        var parameter = Substitute.For<ParameterInfo>();
        parameter.IsDefined(typeof(DynamicAttribute), false).Returns(false);

        // Act
        var result = parameter.HasDynamicAttribute();

        // Assert
        result.Should().BeFalse();
    }
}