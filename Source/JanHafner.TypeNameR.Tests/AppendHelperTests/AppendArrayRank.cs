using AwesomeAssertions;
using JanHafner.TypeNameR.Helper;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AppendHelperTests;

public sealed class AppendArrayRank
{
    [Fact]
    public void AppendsCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        var arrayRank = 2;

        // Act
        stringBuilder.AppendArrayRank(arrayRank);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.LeftSquareBracket}{Constants.Comma}{Constants.RightSquareBracket}");
    }
}