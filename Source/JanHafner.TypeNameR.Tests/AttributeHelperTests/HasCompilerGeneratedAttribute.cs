using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using NSubstitute;
using System.Runtime.CompilerServices;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AttributeHelperTests;

public sealed class HasCompilerGeneratedAttribute
{
    [Fact]
    public void IsTrue()
    {
        // Arrange
        var type = Substitute.For<Type>();
        type.IsDefined(typeof(CompilerGeneratedAttribute), false).Returns(true);

        // Act
        var result = type.HasCompilerGeneratedAttribute();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsFalse()
    {
        // Arrange
        var type = Substitute.For<Type>();
        type.IsDefined(typeof(CompilerGeneratedAttribute), false).Returns(false);

        // Act
        var result = type.HasCompilerGeneratedAttribute();

        // Assert
        result.Should().BeFalse();
    }
}