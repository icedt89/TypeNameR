using FluentAssertions;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using JanHafner.TypeNameR.Helper;
using Xunit;

namespace JanHafner.TypeNameR.Tests.TypeHelperTests;

public sealed class DetermineActualGenerics
{
    [Fact]
    public void ReturnsEmptyForNonNestedNonGenericType()
    {
        // Arrange
        Type[]? masterGenericTypes = null;
        var type = typeof(TestClass);

        // Act
        var hasGenericsDetermined = type.DetermineActualGenerics(ref masterGenericTypes, out var actualStartGenericParameterIndex, out var actualGenericParametersCount);

        // Assert
        hasGenericsDetermined.Should().BeFalse();
        masterGenericTypes.Should().BeNull();
        actualStartGenericParameterIndex.Should().Be(-1);
        actualGenericParametersCount.Should().Be(-1);
    }

    [Fact]
    public void ReturnsEmptyForCompletelyNonGenericNestedType()
    {
        // Arrange
        Type[]? masterGenericTypes = null;
        var type = typeof(TestClass.InnerTestClass.MostInnerTestClass);

        // Act
        var hasGenericsDetermined = type.DetermineActualGenerics(ref masterGenericTypes, out var actualStartGenericParameterIndex, out var actualGenericParametersCount);

        // Assert
        hasGenericsDetermined.Should().BeFalse();
        masterGenericTypes.Should().BeNull();
        actualStartGenericParameterIndex.Should().Be(-1);
        actualGenericParametersCount.Should().Be(-1);
    }

    [Fact]
    public void ReturnsOneGenericForNonNestedGenericType()
    {
        // Arrange
        Type[]? masterGenericTypes = null;
        var type = typeof(GenericTestClass<>);

        // Act
        var hasGenericsDetermined = type.DetermineActualGenerics(ref masterGenericTypes, out var actualStartGenericParameterIndex, out var actualGenericParametersCount);

        // Assert
        hasGenericsDetermined.Should().BeTrue();
        masterGenericTypes.Should().HaveCount(1);
        actualStartGenericParameterIndex.Should().Be(0);
        actualGenericParametersCount.Should().Be(1);
    }

    [Fact]
    public void ReturnsNoGenericForNonGenericTypeNestedInGenericType()
    {
        // Arrange
        Type[]? masterGenericTypes = null;
        var type = typeof(GenericTestClass<>.InnerNonGenericTestClass);

        // Act
        var hasGenericsDetermined = type.DetermineActualGenerics(ref masterGenericTypes, out var actualStartGenericParameterIndex, out var actualGenericParametersCount);

        // Assert
        hasGenericsDetermined.Should().BeFalse();
        masterGenericTypes.Should().HaveCount(1);
        actualStartGenericParameterIndex.Should().Be(1);
        actualGenericParametersCount.Should().Be(1);
    }

    [Fact]
    public void ReturnsTwoGenericsForGenericTypeNestedInGenericType()
    {
        // Arrange
        Type[]? masterGenericTypes = null;
        var type = typeof(GenericTestClass<>.InnerNonGenericTestClass.MostInnerGenericTestClass<,>);

        // Act
        var hasGenericsDetermined = type.DetermineActualGenerics(ref masterGenericTypes, out var actualStartGenericParameterIndex, out var actualGenericParametersCount);

        // Assert
        hasGenericsDetermined.Should().BeTrue();
        masterGenericTypes.Should().HaveCount(3);
        actualStartGenericParameterIndex.Should().Be(1);
        actualGenericParametersCount.Should().Be(3);
    }
}