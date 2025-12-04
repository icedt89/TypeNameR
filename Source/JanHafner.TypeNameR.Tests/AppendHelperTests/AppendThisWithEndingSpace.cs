using AwesomeAssertions;
using JanHafner.TypeNameR.Helper;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AppendHelperTests;

public sealed class AppendThisWithEndingSpace
{
    [Fact]
    public void AppendsCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();

        // Act
        stringBuilder.AppendThisWithEndingSpace();

        // Assert
        stringBuilder.ToString().Should().Be(Constants.ThisWithEndingSpace);
    }
}