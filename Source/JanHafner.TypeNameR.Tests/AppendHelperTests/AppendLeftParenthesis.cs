using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AppendHelperTests;

public sealed class AppendLeftParenthesis
{
    [Fact]
    public void AppendsCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();

        // Act
        stringBuilder.AppendLeftParenthesis();

        // Assert
        stringBuilder.ToString().Should().Be(Constants.LeftParenthesis.ToString());
    }
}