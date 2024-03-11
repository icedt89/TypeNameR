using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using NSubstitute;
using System.Diagnostics;
using System.Reflection;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AttributeHelperTests;

public sealed class HasStackTraceHiddenAttribute
{
    [Fact]
    public void IsTrue()
    {
        // Arrange
        var member = Substitute.For<MemberInfo>();
        member.IsDefined(typeof(StackTraceHiddenAttribute), false).Returns(true);

        // Act
        var result = member.HasStackTraceHiddenAttribute();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsFalse()
    {
        // Arrange
        var member = Substitute.For<MemberInfo>();
        member.IsDefined(typeof(StackTraceHiddenAttribute), false).Returns(false);

        // Act
        var result = member.HasStackTraceHiddenAttribute();

        // Assert
        result.Should().BeFalse();
    }
}