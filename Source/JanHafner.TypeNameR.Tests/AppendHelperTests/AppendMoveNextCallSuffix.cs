using AwesomeAssertions;
using JanHafner.TypeNameR.Helper;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AppendHelperTests;

public sealed class AppendMoveNextCallSuffix
{
    [Fact]
    public void AppendsCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();

        // Act
        stringBuilder.AppendMoveNextCallSuffix();

        // Assert
        stringBuilder.ToString().Should().Be(Constants.MoveNextCallSuffix);
    }
}