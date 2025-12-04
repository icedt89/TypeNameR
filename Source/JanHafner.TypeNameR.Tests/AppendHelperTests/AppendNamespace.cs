using AwesomeAssertions;
using JanHafner.TypeNameR.Helper;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AppendHelperTests;

public sealed class AppendNamespace
{
    [Fact]
    public void AppendsCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        var @namespace = Guid.NewGuid().ToString();

        // Act
        stringBuilder.AppendNamespace(@namespace);

        // Assert
        stringBuilder.ToString().Should().Be($"{@namespace}{Constants.FullStop}");
    }
}