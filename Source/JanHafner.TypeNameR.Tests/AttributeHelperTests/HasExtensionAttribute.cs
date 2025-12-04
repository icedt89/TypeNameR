using AwesomeAssertions;
using JanHafner.TypeNameR.Helper;
using NSubstitute;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AttributeHelperTests;

public sealed class HasExtensionAttribute
{
    [Fact]
    public void IsTrue()
    {
        // Arrange
        var member = Substitute.For<MemberInfo>();
        member.IsDefined(typeof(ExtensionAttribute), false).Returns(true);

        // Act
        var result = member.HasExtensionAttribute();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsFalse()
    {
        // Arrange
        var member = Substitute.For<MemberInfo>();
        member.IsDefined(typeof(ExtensionAttribute), false).Returns(false);

        // Act
        var result = member.HasExtensionAttribute();

        // Assert
        result.Should().BeFalse();
    }
}