using FluentAssertions;
using Xunit;

namespace JanHafner.TypeNameR.Tests.TypeNameRTests;

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
    [InlineData(typeof(GenericTestClass<string[]>), "GenericTestClass<string[]>")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<string[]>")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[]>), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<string[], IReadOnlyList<double?>, object[]>")]
    [InlineData(typeof(GenericTestClass<string[]>[]), "GenericTestClass<string[]>[]")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass[]), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<string[]>[]")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[]>[]), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<string[], IReadOnlyList<double?>, object[]>[]")]
    [InlineData(typeof(GenericTestClass<string[,]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[,][]>[][,,]), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<string[,], IReadOnlyList<double?>, object[][,]>[,,][]")]
    
    [InlineData(typeof(TestStruct), "TestStruct")]
    [InlineData(typeof(TestStruct?), "TestStruct?")]
    [InlineData(typeof(TestStruct.InnerTestStruct?), "JanHafner.TypeNameR.Tests.TestStruct+InnerTestStruct?")]
    [InlineData(typeof(TestStruct.InnerTestStruct.MostInnerTestStruct?), "JanHafner.TypeNameR.Tests.TestStruct+InnerTestStruct+MostInnerTestStruct?")]
    [InlineData(typeof(GenericTestStruct<>), "GenericTestStruct<T>")]
    [InlineData(typeof(GenericTestStruct<TestClass>), "GenericTestStruct<TestClass>")]
    [InlineData(typeof(GenericTestStruct<>.InnerNonGenericTestStruct), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<T>")]
    [InlineData(typeof(GenericTestStruct<>.InnerNonGenericTestStruct.MostInnerGenericTestStruct<,>), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<T>+MostInnerGenericTestStruct<T, R, M>")]
    [InlineData(typeof(GenericTestStruct<double?>), "GenericTestStruct<double?>")]
    [InlineData(typeof(GenericTestStruct<double?>.InnerNonGenericTestStruct), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<double?>")]
    [InlineData(typeof(GenericTestStruct<double?>.InnerNonGenericTestStruct.MostInnerGenericTestStruct<double?, string>), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<T>+MostInnerGenericTestStruct<double?, double?, string>")]
    [InlineData(typeof(GenericTestStruct<string[]>), "GenericTestStruct<string[]>")]
    [InlineData(typeof(GenericTestStruct<string[]>.InnerNonGenericTestStruct), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<string[]>")]
    [InlineData(typeof(GenericTestStruct<string[]>.InnerNonGenericTestStruct.MostInnerGenericTestStruct<IReadOnlyList<double?>, object[]>), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<T>+MostInnerGenericTestStruct<string[], IReadOnlyList<double?>, object[]>")]
    [InlineData(typeof(GenericTestStruct<string[]>[]), "GenericTestStruct<string[]>[]")]
    [InlineData(typeof(GenericTestStruct<string[]>.InnerNonGenericTestStruct[]), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<string[]>[]")]
    [InlineData(typeof(GenericTestStruct<string[]>.InnerNonGenericTestStruct.MostInnerGenericTestStruct<IReadOnlyList<double?>, object[]>[]), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<T>+MostInnerGenericTestStruct<string[], IReadOnlyList<double?>, object[]>[]")]
    [InlineData(typeof(GenericTestStruct<string[,]>.InnerNonGenericTestStruct.MostInnerGenericTestStruct<IReadOnlyList<double?>, object[,][]>[][,,]), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<T>+MostInnerGenericTestStruct<string[,], IReadOnlyList<double?>, object[][,]>[,,][]")]
    
    [InlineData(typeof(string[]), "string[]")]
    [InlineData(typeof(string[][]), "string[][]")]
    [InlineData(typeof(string[,]), "string[,]")]
    [InlineData(typeof(string[,,]), "string[,,]")]
    [InlineData(typeof(string[,,][,]), "string[,][,,]")]
    [InlineData(typeof(string[,][,,]), "string[,,][,]")]
    [InlineData(typeof(string[][,,][,]), "string[,][,,][]")]
    public void ExtractReadableName(Type type, string expectedReadableName)
    {
        // Arrange
        var typeNameR = new TypeNameR();

        // Act
        var readableTypeName = typeNameR.ExtractReadable(type, false);

        // Assert
        readableTypeName.Should().Be(expectedReadableName);
    }
    
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
    
    [InlineData(typeof(TestClass), "JanHafner.TypeNameR.Tests.TestClass")]
    [InlineData(typeof(TestClass.InnerTestClass), "JanHafner.TypeNameR.Tests.TestClass+InnerTestClass")]
    [InlineData(typeof(TestClass.InnerTestClass.MostInnerTestClass), "JanHafner.TypeNameR.Tests.TestClass+InnerTestClass+MostInnerTestClass")]
    [InlineData(typeof(GenericTestClass<>), "JanHafner.TypeNameR.Tests.GenericTestClass<T>")]
    [InlineData(typeof(GenericTestClass<TestClass>), "JanHafner.TypeNameR.Tests.GenericTestClass<TestClass>")]
    [InlineData(typeof(GenericTestClass<>.InnerNonGenericTestClass), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>")]
    [InlineData(typeof(GenericTestClass<>.InnerNonGenericTestClass.MostInnerGenericTestClass<,>), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<T, R, M>")]
    [InlineData(typeof(GenericTestClass<double?>), "JanHafner.TypeNameR.Tests.GenericTestClass<double?>")]
    [InlineData(typeof(GenericTestClass<double?>.InnerNonGenericTestClass), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<double?>")]
    [InlineData(typeof(GenericTestClass<double?>.InnerNonGenericTestClass.MostInnerGenericTestClass<double?, string>), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<double?, double?, string>")]
    [InlineData(typeof(GenericTestClass<string[]>), "JanHafner.TypeNameR.Tests.GenericTestClass<string[]>")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<string[]>")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[]>), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<string[], IReadOnlyList<double?>, object[]>")]
    [InlineData(typeof(GenericTestClass<string[]>[]), "JanHafner.TypeNameR.Tests.GenericTestClass<string[]>[]")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass[]), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<string[]>[]")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[]>[]), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<string[], IReadOnlyList<double?>, object[]>[]")]
    [InlineData(typeof(GenericTestClass<string[,]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[,][]>[][,,]), "JanHafner.TypeNameR.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<string[,], IReadOnlyList<double?>, object[][,]>[,,][]")]
    
    [InlineData(typeof(TestStruct), "JanHafner.TypeNameR.Tests.TestStruct")]
    [InlineData(typeof(TestStruct?), "JanHafner.TypeNameR.Tests.TestStruct?")]
    [InlineData(typeof(TestStruct.InnerTestStruct?), "JanHafner.TypeNameR.Tests.TestStruct+InnerTestStruct?")]
    [InlineData(typeof(TestStruct.InnerTestStruct.MostInnerTestStruct?), "JanHafner.TypeNameR.Tests.TestStruct+InnerTestStruct+MostInnerTestStruct?")]
    [InlineData(typeof(GenericTestStruct<>), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>")]
    [InlineData(typeof(GenericTestStruct<TestClass>), "JanHafner.TypeNameR.Tests.GenericTestStruct<TestClass>")]
    [InlineData(typeof(GenericTestStruct<>.InnerNonGenericTestStruct), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<T>")]
    [InlineData(typeof(GenericTestStruct<>.InnerNonGenericTestStruct.MostInnerGenericTestStruct<,>), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<T>+MostInnerGenericTestStruct<T, R, M>")]
    [InlineData(typeof(GenericTestStruct<double?>), "JanHafner.TypeNameR.Tests.GenericTestStruct<double?>")]
    [InlineData(typeof(GenericTestStruct<double?>.InnerNonGenericTestStruct), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<double?>")]
    [InlineData(typeof(GenericTestStruct<double?>.InnerNonGenericTestStruct.MostInnerGenericTestStruct<double?, string>), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<T>+MostInnerGenericTestStruct<double?, double?, string>")]
    [InlineData(typeof(GenericTestStruct<string[]>), "JanHafner.TypeNameR.Tests.GenericTestStruct<string[]>")]
    [InlineData(typeof(GenericTestStruct<string[]>.InnerNonGenericTestStruct), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<string[]>")]
    [InlineData(typeof(GenericTestStruct<string[]>.InnerNonGenericTestStruct.MostInnerGenericTestStruct<IReadOnlyList<double?>, object[]>), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<T>+MostInnerGenericTestStruct<string[], IReadOnlyList<double?>, object[]>")]
    [InlineData(typeof(GenericTestStruct<string[]>?[]), "JanHafner.TypeNameR.Tests.GenericTestStruct<string[]>?[]")]
    [InlineData(typeof(GenericTestStruct<string[]>.InnerNonGenericTestStruct[]), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<string[]>[]")]
    [InlineData(typeof(GenericTestStruct<string[]>.InnerNonGenericTestStruct.MostInnerGenericTestStruct<IReadOnlyList<double?>, object[]>[]), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<T>+MostInnerGenericTestStruct<string[], IReadOnlyList<double?>, object[]>[]")]
    [InlineData(typeof(GenericTestStruct<string[,]>.InnerNonGenericTestStruct.MostInnerGenericTestStruct<IReadOnlyList<double?>, object[,][]>[][,,]), "JanHafner.TypeNameR.Tests.GenericTestStruct<T>+InnerNonGenericTestStruct<T>+MostInnerGenericTestStruct<string[,], IReadOnlyList<double?>, object[][,]>[,,][]")]
    
    [InlineData(typeof(string[]), "string[]")]
    [InlineData(typeof(string[][]), "string[][]")]
    [InlineData(typeof(string[,]), "string[,]")]
    [InlineData(typeof(string[,,]), "string[,,]")]
    [InlineData(typeof(string[,,][,]), "string[,][,,]")]
    [InlineData(typeof(string[,][,,]), "string[,,][,]")]
    [InlineData(typeof(string[][,,][,]), "string[,][,,][]")]
    public void ExtractReadableNameWithFullName(Type type, string expectedReadableName)
    {
        // Arrange
        var typeNameR = new TypeNameR();

        // Act
        var readableTypeName = typeNameR.ExtractReadable(type, true);

        // Assert
        readableTypeName.Should().Be(expectedReadableName);
    }
}
