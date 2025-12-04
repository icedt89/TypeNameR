using AwesomeAssertions;
using JanHafner.TypeNameR.Helper;
using Xunit;

namespace JanHafner.TypeNameR.Tests.PredefinedTypeNamesTests;

public sealed class Default
{
    [Fact]
    public void CreatesDictionaryWithCommittedTypeNames()
    {
        // Act
        var @default = PredefinedTypeNames.Default;

        // Assert
        @default.Should().NotBeEmpty();
        @default.Count.Should().Be(18);
    }
}