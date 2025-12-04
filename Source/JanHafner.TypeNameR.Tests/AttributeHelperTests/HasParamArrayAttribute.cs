using AwesomeAssertions;
using JanHafner.TypeNameR.Helper;
using NSubstitute;
using System.Reflection;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AttributeHelperTests;

public sealed class HasParamArrayAttribute
{
    [Fact]
    public void IsTrue()
    {
        // Arrange
        var parameter = Substitute.For<ParameterInfo>();
        parameter.IsDefined(typeof(ParamArrayAttribute), false).Returns(true);

        // Act
        var result = parameter.HasParamArrayAttribute();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsFalse()
    {
        // Arrange
        var parameter = Substitute.For<ParameterInfo>();
        parameter.IsDefined(typeof(ParamArrayAttribute), false).Returns(false);

        // Act
        var result = parameter.HasParamArrayAttribute();

        // Assert
        result.Should().BeFalse();
    }
}