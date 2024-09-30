using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AppendHelperTests;

public sealed class AppendEqualsQuotedValue
{
    [Fact]
    public void AppendsReadOnlySpanOfCharCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        ReadOnlySpan<char> value = "2";

        // Act
        stringBuilder.AppendEqualsQuotedValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.Space}{Constants.EqualsSignWithEndingSpace}{Constants.QuotationMark}{value}{Constants.QuotationMark}");
    }
}