using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AppendHelperTests;

public sealed class AppendQuotedParameterValue
{
    [Fact]
    public void AppendsCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        var value = Guid.NewGuid().ToString();

        // Act
        stringBuilder.AppendQuotedParameterValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.EqualsSignWithEndingSpace}{Constants.QuotationMark}{value}{Constants.QuotationMark}");
    }
}