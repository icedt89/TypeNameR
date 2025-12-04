using AwesomeAssertions;
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
        var actualGenericTypes = type.DetermineActualGenerics(ref masterGenericTypes);

        // Assert
        actualGenericTypes.ToArray().Should().BeEmpty();
        masterGenericTypes.Should().BeNull();
    }

    [Fact]
    public void ReturnsEmptyForCompletelyNonGenericNestedType()
    {
        // Arrange
        Type[]? masterGenericTypes = null;
        var type = typeof(TestClass.InnerTestClass.MostInnerTestClass);

        // Act
        var actualGenericTypes = type.DetermineActualGenerics(ref masterGenericTypes);

        // Assert
        actualGenericTypes.ToArray().Should().BeEmpty();
        masterGenericTypes.Should().BeNull();
    }

    [Fact]
    public void ReturnsOneGenericForNonNestedGenericType()
    {
        // Arrange
        Type[]? masterGenericTypes = null;
        var type = typeof(GenericTestClass<>);

        // Act
        var actualGenericTypes = type.DetermineActualGenerics(ref masterGenericTypes);

        // Assert
        masterGenericTypes.Should().NotBeNullOrEmpty().And.HaveCount(1);
        masterGenericTypes![0].Name.Should().Be("T");

        actualGenericTypes.ToArray().Should().NotBeEmpty().And.HaveCount(1);
        actualGenericTypes[0].Name.Should().Be("T");
    }

    [Fact]
    public void ReturnsNoGenericForNonGenericTypeNestedInGenericType()
    {
        // Arrange
        Type[]? masterGenericTypes = null;
        var type = typeof(GenericTestClass<>.InnerNonGenericTestClass);

        // Act
        var actualGenericTypes = type.DetermineActualGenerics(ref masterGenericTypes);

        // Assert
        masterGenericTypes.Should().NotBeNullOrEmpty().And.HaveCount(1);
        masterGenericTypes![0].Name.Should().Be("T");

        actualGenericTypes.ToArray().Should().BeEmpty();
    }

    [Fact]
    public void ReturnsTwoGenericsForGenericTypeNestedInGenericType()
    {
        // Arrange
        Type[]? masterGenericTypes = null;
        var type = typeof(GenericTestClass<>.InnerNonGenericTestClass.MostInnerGenericTestClass<,>);

        // Act
        var actualGenericTypes = type.DetermineActualGenerics(ref masterGenericTypes);

        // Assert
        masterGenericTypes.Should().NotBeNullOrEmpty().And.HaveCount(3);
        masterGenericTypes![0].Name.Should().Be("T");
        masterGenericTypes[1].Name.Should().Be("R");
        masterGenericTypes[2].Name.Should().Be("M");

        actualGenericTypes.ToArray().Should().NotBeEmpty().And.HaveCount(2);
        actualGenericTypes[0].Name.Should().Be("R");
        actualGenericTypes[1].Name.Should().Be("M");
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
        var actualGenericTypes1 = type1.DetermineActualGenerics(ref masterGenericTypes);
        var actualGenericTypes2 = type2.DetermineActualGenerics(ref masterGenericTypes);
        var actualGenericTypes3 = type3.DetermineActualGenerics(ref masterGenericTypes);

        // Assert
        masterGenericTypes.Should().NotBeNullOrEmpty().And.HaveCount(3);
        masterGenericTypes![0].Name.Should().Be("T");
        masterGenericTypes[1].Name.Should().Be("R");
        masterGenericTypes[2].Name.Should().Be("M");

        // type1
        actualGenericTypes1.ToArray().Should().NotBeEmpty().And.HaveCount(2);
        actualGenericTypes1[0].Name.Should().Be("R");
        actualGenericTypes1[1].Name.Should().Be("M");

        // type2
        actualGenericTypes2.ToArray().Should().BeEmpty();

        // type3
        actualGenericTypes3.ToArray().Should().NotBeEmpty().And.HaveCount(1);
        actualGenericTypes3[0].Name.Should().Be("T");
    }
}