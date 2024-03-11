using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AppendHelperTests;

public sealed class AppendExceptionMessage
{
    [Fact]
    public void AppendsCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        var exceptionMessage = Guid.NewGuid().ToString();

        // Act
        stringBuilder.AppendExceptionMessage(exceptionMessage);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.ColonWithEndingSpace}{exceptionMessage}");
    }
}