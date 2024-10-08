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
        
        masterGenericTypes.Should().NotBeNullOrEmpty().And.HaveCount(1);
        masterGenericTypes![0].Name.Should().Be("T");
        
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
        
        masterGenericTypes.Should().NotBeNullOrEmpty().And.HaveCount(1);
        masterGenericTypes![0].Name.Should().Be("T");
        
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
        
        masterGenericTypes.Should().NotBeNullOrEmpty().And.HaveCount(3);
        masterGenericTypes![0].Name.Should().Be("T");
        masterGenericTypes[1].Name.Should().Be("R");
        masterGenericTypes[2].Name.Should().Be("M");
        
        actualStartGenericParameterIndex.Should().Be(1);
        actualGenericParametersCount.Should().Be(3);
    }
    
    [Fact]
    public void ReturnsCorrectGenericsForFlow()
    {
        // Arrange
        Type[]? masterGenericTypes = null;
        var type1 = typeof(GenericTestClass<>.InnerNonGenericTestClass.MostInnerGenericTestClass<,>);
        var type2 = type1.DeclaringType!;
        var type3 = type1.DeclaringType!.DeclaringType!;

        // Act
        var hasGenericsDetermined1 = type1.DetermineActualGenerics(ref masterGenericTypes, out var actualStartGenericParameterIndex1, out var actualGenericParametersCount1);
        var hasGenericsDetermined2 = type2.DetermineActualGenerics(ref masterGenericTypes, out var actualStartGenericParameterIndex2, out var actualGenericParametersCount2);
        var hasGenericsDetermined3 = type3.DetermineActualGenerics(ref masterGenericTypes, out var actualStartGenericParameterIndex3, out var actualGenericParametersCount3);

        // Assert
        masterGenericTypes.Should().NotBeNullOrEmpty().And.HaveCount(3);
        masterGenericTypes![0].Name.Should().Be("T");
        masterGenericTypes[1].Name.Should().Be("R");
        masterGenericTypes[2].Name.Should().Be("M");
        
        // type1
        hasGenericsDetermined1.Should().BeTrue();
        actualStartGenericParameterIndex1.Should().Be(1);
        actualGenericParametersCount1.Should().Be(3);
        
        // type2
        hasGenericsDetermined2.Should().BeFalse();
        actualStartGenericParameterIndex2.Should().Be(1);
        actualGenericParametersCount2.Should().Be(1);
        
        // type3
        hasGenericsDetermined3.Should().BeTrue();
        actualStartGenericParameterIndex3.Should().Be(0);
        actualGenericParametersCount3.Should().Be(1);
    }
}