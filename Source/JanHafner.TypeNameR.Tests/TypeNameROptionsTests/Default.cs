using FluentAssertions;
using Xunit;

namespace JanHafner.TypeNameR.Tests.TypeNameROptionsTests;

public sealed class Default
{
    [Fact]
    public void CreatesDefaultInstanceCorrectly()
    {
        // Act
        var @default = TypeNameROptions.Default;

        // Assert
        @default.PredefinedTypeNames.Should().NotBeEmpty();
    }
}