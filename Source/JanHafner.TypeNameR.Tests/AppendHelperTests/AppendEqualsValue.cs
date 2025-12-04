using AwesomeAssertions;
using JanHafner.TypeNameR.Helper;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AppendHelperTests;

public sealed class AppendEqualsValue
{
    [Fact]
    public void AppendsReadOnlySpanOfCharCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        ReadOnlySpan<char> value = "true";

        // Act
        stringBuilder.AppendEqualsValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.Space}{Constants.EqualsSignWithEndingSpace}{value}");
    }

    [Fact]
    public void AppendsBoolCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        const bool value = true;

        // Act
        stringBuilder.AppendEqualsValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.Space}{Constants.EqualsSignWithEndingSpace}{value}");
    }

    [Fact]
    public void AppendsCharCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        const char value = 'c';

        // Act
        stringBuilder.AppendEqualsValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.Space}{Constants.EqualsSignWithEndingSpace}{value}");
    }

    [Fact]
    public void AppendsSByteCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        const sbyte value = 2;

        // Act
        stringBuilder.AppendEqualsValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.Space}{Constants.EqualsSignWithEndingSpace}{value}");
    }

    [Fact]
    public void AppendsByteCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        const byte value = 2;

        // Act
        stringBuilder.AppendEqualsValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.Space}{Constants.EqualsSignWithEndingSpace}{value}");
    }

    [Fact]
    public void AppendsShortCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        const short value = 2;

        // Act
        stringBuilder.AppendEqualsValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.Space}{Constants.EqualsSignWithEndingSpace}{value}");
    }

    [Fact]
    public void AppendsIntCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        const int value = 2;

        // Act
        stringBuilder.AppendEqualsValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.Space}{Constants.EqualsSignWithEndingSpace}{value}");
    }

    [Fact]
    public void AppendsLongCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        const long value = 2;

        // Act
        stringBuilder.AppendEqualsValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.Space}{Constants.EqualsSignWithEndingSpace}{value}");
    }

    [Fact]
    public void AppendsFloatCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        const float value = 2;

        // Act
        stringBuilder.AppendEqualsValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.Space}{Constants.EqualsSignWithEndingSpace}{value}");
    }

    [Fact]
    public void AppendsDoubleCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        const double value = 2;

        // Act
        stringBuilder.AppendEqualsValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.Space}{Constants.EqualsSignWithEndingSpace}{value}");
    }

    [Fact]
    public void AppendsDecimalCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        const decimal value = 2;

        // Act
        stringBuilder.AppendEqualsValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.Space}{Constants.EqualsSignWithEndingSpace}{value}");
    }

    [Fact]
    public void AppendsUShortCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        const ushort value = 2;

        // Act
        stringBuilder.AppendEqualsValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.Space}{Constants.EqualsSignWithEndingSpace}{value}");
    }

    [Fact]
    public void AppendsUIntCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        const uint value = 2;

        // Act
        stringBuilder.AppendEqualsValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.Space}{Constants.EqualsSignWithEndingSpace}{value}");
    }

    [Fact]
    public void AppendsULongCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        const ulong value = 2;

        // Act
        stringBuilder.AppendEqualsValue(value);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.Space}{Constants.EqualsSignWithEndingSpace}{value}");
    }
}