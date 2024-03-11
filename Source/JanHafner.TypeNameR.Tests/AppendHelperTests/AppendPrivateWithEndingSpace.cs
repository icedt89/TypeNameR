using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AppendHelperTests;

public sealed class AppendPrivateWithEndingSpace
{
    [Fact]
    public void AppendsCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();

        // Act
        stringBuilder.AppendPrivateWithEndingSpace();

        // Assert
        stringBuilder.ToString().Should().Be(Constants.PrivateWithEndingSpace);
    }
}