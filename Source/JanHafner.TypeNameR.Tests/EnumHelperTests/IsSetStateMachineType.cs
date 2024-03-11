using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using Xunit;

namespace JanHafner.TypeNameR.Tests.EnumHelperTests;

public sealed class IsSetStateMachineType
{
    [Fact]
    public void FlagIsSet()
    {
        // Arrange
        var flags = StateMachineType.AsyncIterator;
        var flag = StateMachineType.Async;

        // Act
        var isSet = flags.IsSet(flag);

        // Assert
        isSet.Should().BeTrue();
    }

    [Fact]
    public void FlagIsNotSet()
    {
        // Arrange
        var flags = StateMachineType.Async;
        var flag = StateMachineType.Iterator;

        // Act
        var isSet = flags.IsSet(flag);

        // Assert
        isSet.Should().BeFalse();
    }
}