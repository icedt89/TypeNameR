using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using System.Reflection;
using Xunit;

namespace JanHafner.TypeNameR.Tests.EnumHelperTests;

public sealed class IsSetMethodImplAttributes
{
    [Fact]
    public void FlagIsSet()
    {
        // Arrange
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        var flags = MethodImplAttributes.AggressiveInlining | MethodImplAttributes.AggressiveOptimization;
        var flag = MethodImplAttributes.AggressiveInlining;

        // Act
        var isSet = flags.IsSet(flag);

        // Assert
        isSet.Should().BeTrue();
    }

    [Fact]
    public void FlagIsNotSet()
    {
        // Arrange
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        var flags = MethodImplAttributes.AggressiveInlining | MethodImplAttributes.AggressiveOptimization;
        var flag = MethodImplAttributes.NoInlining;

        // Act
        var isSet = flags.IsSet(flag);

        // Assert
        isSet.Should().BeFalse();
    }
}