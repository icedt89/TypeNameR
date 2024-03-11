using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AppendHelperTests;

public sealed class AppendEqualsValue
{
    [Fact]
    public void AppendsCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        var value = 2;

        // Act
        stringBuilder.AppendEqualsValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.EqualsSignWithEndingSpace}{value}");
    }
}