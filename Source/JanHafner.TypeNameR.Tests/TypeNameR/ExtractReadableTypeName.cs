using FluentAssertions;
using Xunit;

namespace JanHafner.TypeNameR.Tests.TypeNameR;

public sealed class ExtractReadableTypeName
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

    [InlineData(typeof(TestClass), "TestClass")]
    [InlineData(typeof(TestClass.InnerTestClass), "JanHafner.TypeNameR.Tests.TestClass+InnerTestClass")]
    [InlineData(typeof(TestClass.InnerTestClass.MostInnerTestClass), "JanHafner.TypeNameR.Tests.TestClass+InnerTestClass+MostInnerTestClass")]
    [InlineData(typeof(GenericTestClass<>), "GenericTestClass<T>")]
    [InlineData(typeof(GenericTestClass<TestClass>), "GenericTestClass<TestClass>")]
    [InlineData(typeof(GenericTestClass<>.InnerNonGenericTestClass), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>")]
    [InlineData(typeof(GenericTestClass<>.InnerNonGenericTestClass.MostInnerGenericTestClass<,>), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<T, R, M>")]

    [InlineData(typeof(GenericTestClass<double?>), "GenericTestClass<double?>")]
    [InlineData(typeof(GenericTestClass<double?>.InnerNonGenericTestClass), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<double?>")]
    [InlineData(typeof(GenericTestClass<double?>.InnerNonGenericTestClass.MostInnerGenericTestClass<double?, string>), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<double?, double?, string>")]

    [InlineData(typeof(string[]), "string[]")]
    [InlineData(typeof(GenericTestClass<string[]>), "GenericTestClass<string[]>")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<string[]>")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[]>), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<string[], IReadOnlyList<double?>, object[]>")]

    [InlineData(typeof(GenericTestClass<string[]>[]), "GenericTestClass<string[]>[]")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass[]), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<string[]>[]")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[]>[]), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<string[], IReadOnlyList<double?>, object[]>[]")]

    [InlineData(typeof(string[][]), "string[][]")]
    [InlineData(typeof(string[,]), "string[,]")]
    [InlineData(typeof(string[,,]), "string[,,]")]
    [InlineData(typeof(string[,,][,]), "string[,][,,]")]
    [InlineData(typeof(string[,][,,]), "string[,,][,]")]
    [InlineData(typeof(string[][,,][,]), "string[,][,,][]")]
    [InlineData(typeof(GenericTestClass<string[,]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[,][]>[][,,]), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<string[,], IReadOnlyList<double?>, object[][,]>[,,][]")]
    public void ExtractReadableName(Type type, string expectedReadableName)
    {
        // Arrange
        var typeNameR = new JanHafner.TypeNameR.TypeNameR();

        // Act
        var readableTypeName = typeNameR.ExtractReadable(type);

        // Assert
        readableTypeName.Should().Be(expectedReadableName);
    }
}
