using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AppendHelperTests;

public sealed class AppendConstructor
{
    [Fact]
    public void AppendsCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();

        // Act
        stringBuilder.AppendConstructor();

        // Assert
        stringBuilder.ToString().Should().Be(Constants.Constructor);
    }
}