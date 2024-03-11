using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AppendHelperTests;

public sealed class AppendParameterName
{
    [Fact]
    public void AppendsCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        var parameterName = Guid.NewGuid().ToString();

        // Act
        stringBuilder.AppendParameterName(parameterName);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.Space}{parameterName}");
    }
}