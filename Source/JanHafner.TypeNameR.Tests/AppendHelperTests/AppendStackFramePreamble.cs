using AwesomeAssertions;
using JanHafner.TypeNameR.Helper;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AppendHelperTests;

public sealed class AppendStackFramePreamble
{
    [Fact]
    public void AppendsCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();

        // Act
        stringBuilder.AppendStackFramePreamble();

        // Assert
        stringBuilder.ToString().Should().Be(Constants.Indent + Constants.AtWithEndingSpace);
    }
}