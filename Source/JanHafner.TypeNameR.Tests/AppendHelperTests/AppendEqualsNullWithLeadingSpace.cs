using AwesomeAssertions;
using JanHafner.TypeNameR.Helper;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AppendHelperTests;

public sealed class AppendEqualsNullWithLeadingSpace
{
    [Fact]
    public void AppendsCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();

        // Act
        stringBuilder.AppendEqualsNullWithLeadingSpace();

        // Assert
        stringBuilder.ToString().Should().Be(Constants.EqualsNullWithLeadingSpace);
    }
}