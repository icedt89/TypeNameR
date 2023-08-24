using FluentAssertions;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using Xunit;

namespace JanHafner.TypeNameR.Tests.TypeNameRTests;

public sealed class GenerateDisplayOfType
{
    [Theory]
    [InlineData(typeof(string), "string")]
    [InlineData(typeof(object), "object")]
    [InlineData(typeof(bool), "bool")]
    [InlineData(typeof(double), "double")]
    [InlineData(typeof(decimal), "decimal")]
    [InlineData(typeof(float), "float")]
    [InlineData(typeof(sbyte), "sbyte")]
    [InlineData(typeof(byte), "byte")]
    [InlineData(typeof(void), "void")]
    [InlineData(typeof(ushort), "ushort")]
    [InlineData(typeof(short), "short")]
    [InlineData(typeof(uint), "uint")]
    [InlineData(typeof(int), "int")]
    [InlineData(typeof(ulong), "ulong")]
    [InlineData(typeof(long), "long")]
    [InlineData(typeof(char), "char")]
    [InlineData(typeof(nuint), "nuint")]
    [InlineData(typeof(nint), "nint")]
    [InlineData(typeof(bool?), "bool?")]
    [InlineData(typeof(double?), "double?")]
    [InlineData(typeof(decimal?), "decimal?")]
    [InlineData(typeof(float?), "float?")]
    [InlineData(typeof(sbyte?), "sbyte?")]
    [InlineData(typeof(byte?), "byte?")]
    [InlineData(typeof(ushort?), "ushort?")]
    [InlineData(typeof(short?), "short?")]
    [InlineData(typeof(uint?), "uint?")]
    [InlineData(typeof(int?), "int?")]
    [InlineData(typeof(ulong?), "ulong?")]
    [InlineData(typeof(long?), "long?")]
    [InlineData(typeof(char?), "char?")]
    [InlineData(typeof(nuint?), "nuint?")]
    [InlineData(typeof(nint?), "nint?")]
    [InlineData(typeof(TestClass), "TestClass")]
    [InlineData(typeof(TestClass.InnerTestClass), "JanHafner.TypeNameR.BenchmarkAndTestUtils.TestClass+InnerTestClass")]
    [InlineData(typeof(TestClass.InnerTestClass.MostInnerTestClass), "JanHafner.TypeNameR.BenchmarkAndTestUtils.TestClass+InnerTestClass+MostInnerTestClass")]
    [InlineData(typeof(GenericTestClass<>), "GenericTestClass<T>")]
    [InlineData(typeof(GenericTestClass<TestClass>), "GenericTestClass<TestClass>")]
    [InlineData(typeof(GenericTestClass<>.InnerNonGenericTestClass),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestClass<T>+InnerNonGenericTestClass<T>")]
    [InlineData(typeof(GenericTestClass<>.InnerNonGenericTestClass.MostInnerGenericTestClass<,>),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<T, R, M>")]
    [InlineData(typeof(GenericTestClass<double?>), "GenericTestClass<double?>")]
    [InlineData(typeof(GenericTestClass<double?>.InnerNonGenericTestClass),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestClass<T>+InnerNonGenericTestClass<double?>")]
    [InlineData(typeof(GenericTestClass<double?>.InnerNonGenericTestClass.MostInnerGenericTestClass<double?, string>),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<double?, double?, string>")]
    [InlineData(typeof(GenericTestClass<string[]>), "GenericTestClass<string[]>")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestClass<T>+InnerNonGenericTestClass<string[]>")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[]>),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<string[], IReadOnlyList<double?>, object[]>")]
    [InlineData(typeof(GenericTestClass<string[]>[]), "GenericTestClass<string[]>[]")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass[]),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestClass<T>+InnerNonGenericTestClass<string[]>[]")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[]>[]),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<string[], IReadOnlyList<double?>, object[]>[]")]
    [InlineData(typeof(GenericTestClass<string[,]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[,][]>[][,,]),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<string[,], IReadOnlyList<double?>, object[][,]>[,,][]")]
    [InlineData(typeof(TestStruct), "TestStruct")]
    [InlineData(typeof(TestStruct?), "TestStruct?")]
    [InlineData(typeof(TestStruct.InnerTestStruct?), "JanHafner.TypeNameR.BenchmarkAndTestUtils.TestStruct+InnerTestStruct?")]
    [InlineData(typeof(TestStruct.InnerTestStruct.MostInnerTestStruct?),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.TestStruct+InnerTestStruct+MostInnerTestStruct?")]
    [InlineData(typeof(GenericTestStruct<>), "GenericTestStruct<T>")]
    [InlineData(typeof(GenericTestStruct<TestClass>), "GenericTestStruct<TestClass>")]
    [InlineData(typeof(GenericTestStruct<>.InnerNonGenericTestStruct),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestStruct<T>+InnerNonGenericTestStruct<T>")]
    [InlineData(typeof(GenericTestStruct<>.InnerNonGenericTestStruct.MostInnerGenericTestStruct<,>),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestStruct<T>+InnerNonGenericTestStruct<T>+MostInnerGenericTestStruct<T, R, M>")]
    [InlineData(typeof(GenericTestStruct<double?>), "GenericTestStruct<double?>")]
    [InlineData(typeof(GenericTestStruct<double?>.InnerNonGenericTestStruct),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestStruct<T>+InnerNonGenericTestStruct<double?>")]
    [InlineData(typeof(GenericTestStruct<double?>.InnerNonGenericTestStruct.MostInnerGenericTestStruct<double?, string>),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestStruct<T>+InnerNonGenericTestStruct<T>+MostInnerGenericTestStruct<double?, double?, string>")]
    [InlineData(typeof(GenericTestStruct<string[]>), "GenericTestStruct<string[]>")]
    [InlineData(typeof(GenericTestStruct<string[]>.InnerNonGenericTestStruct),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestStruct<T>+InnerNonGenericTestStruct<string[]>")]
    [InlineData(typeof(GenericTestStruct<string[]>.InnerNonGenericTestStruct.MostInnerGenericTestStruct<IReadOnlyList<double?>, object[]>),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestStruct<T>+InnerNonGenericTestStruct<T>+MostInnerGenericTestStruct<string[], IReadOnlyList<double?>, object[]>")]
    [InlineData(typeof(GenericTestStruct<string[]>[]), "GenericTestStruct<string[]>[]")]
    [InlineData(typeof(GenericTestStruct<string[]>.InnerNonGenericTestStruct[]),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestStruct<T>+InnerNonGenericTestStruct<string[]>[]")]
    [InlineData(typeof(GenericTestStruct<string[]>.InnerNonGenericTestStruct.MostInnerGenericTestStruct<IReadOnlyList<double?>, object[]>[]),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestStruct<T>+InnerNonGenericTestStruct<T>+MostInnerGenericTestStruct<string[], IReadOnlyList<double?>, object[]>[]")]
    [InlineData(typeof(GenericTestStruct<string[,]>.InnerNonGenericTestStruct.MostInnerGenericTestStruct<IReadOnlyList<double?>, object[,][]>[][,,]),
        "JanHafner.TypeNameR.BenchmarkAndTestUtils.GenericTestStruct<T>+InnerNonGenericTestStruct<T>+MostInnerGenericTestStruct<string[,], IReadOnlyList<double?>, object[][,]>[,,][]")]
    [InlineData(typeof(string[]), "string[]")]
    [InlineData(typeof(string[][]), "string[][]")]
    [InlineData(typeof(string[,]), "string[,]")]
    [InlineData(typeof(string[,,]), "string[,,]")]
    [InlineData(typeof(string[,,][,]), "string[,][,,]")] // Array type is reversed
    [InlineData(typeof(string[,][,,]), "string[,,][,]")] // Array type is reversed
    [InlineData(typeof(string[][,,][,]), "string[,][,,][]")] // Array type is reversed
    public void GenerateTypeDisplay(Type type, string expectedReadableName)
    {
        // Arrange
        var typeNameR = new TypeNameR();

        // Act
        var generated = typeNameR.GenerateDisplay(type, false, null);

        // Assert
        generated.Should().Be(expectedReadableName);
    }

    [Theory]
    [InlineData(typeof(GenerateDisplayOfType[]), "ArrayOfExtractReadableTypeName")]
    [InlineData(typeof(int?), "NullableOfInt")]
    public void GenerateTypeDisplayUsingPredefinedTypeName(Type type, string expectedReadableName)
    {
        // Arrange
        var typeNameR = new TypeNameR(typeNameROptions: new TypeNameROptions(new Dictionary<Type, string> { { type, expectedReadableName } }));

        // Act
        var generated = typeNameR.GenerateDisplay(type, false, null);

        // Assert
        generated.Should().Be(expectedReadableName);
    }

    public static IEnumerable<object[]> GetDoesNotThrowAnExceptionTests(int take)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).SelectMany(a => a.GetExportedTypes()).Take(take);

        foreach (var type in types)
        {
            yield return new[] { type };
        }
    }

    [Theory]
    [MemberData(nameof(GetDoesNotThrowAnExceptionTests), 500)]
    public void DoesNotThrowAnException(Type type)
    {
        // Arrange
        var typeNameR = new TypeNameR();

        // Act, Assert
        var generated = typeNameR.GenerateDisplay(type, false, null);

        generated.Should().NotBeNullOrWhiteSpace();
    }
}