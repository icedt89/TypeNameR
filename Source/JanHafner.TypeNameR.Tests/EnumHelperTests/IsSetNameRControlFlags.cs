using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using Xunit;

namespace JanHafner.TypeNameR.Tests.EnumHelperTests;

public sealed class IsSetNameRControlFlags
{
    [Fact]
    public void FlagIsSet()
    {
        // Arrange
        var flags = NameRControlFlags.IncludeSourceInformation | NameRControlFlags.IncludeAccessModifier;
        var flag = NameRControlFlags.IncludeSourceInformation;

        // Act
        var isSet = flags.IsSet(flag);

        // Assert
        isSet.Should().BeTrue();
    }

    [Fact]
    public void FlagIsNotSet()
    {
        // Arrange
        var flags = NameRControlFlags.IncludeSourceInformation | NameRControlFlags.IncludeAccessModifier;
        var flag = NameRControlFlags.IncludeParamsKeyword;

        // Act
        var isSet = flags.IsSet(flag);

        // Assert
        isSet.Should().BeFalse();
    }
}