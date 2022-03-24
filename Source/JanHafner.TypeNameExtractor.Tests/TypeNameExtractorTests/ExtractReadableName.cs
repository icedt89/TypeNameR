using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace JanHafner.TypeNameExtractor.Tests.TypeNameExtractorTests;

public sealed class ExtractReadableName
{
    [Theory]
    [InlineData(typeof(string), true, false, true, true, "string")]
    [InlineData(typeof(object), true, false, true, true, "object")]
    [InlineData(typeof(bool), true, false, true, true, "bool")]
    [InlineData(typeof(double), true, false, true, true, "double")]
    [InlineData(typeof(decimal), true, false, true, true, "decimal")]
    [InlineData(typeof(float), true, false, true, true, "float")]
    [InlineData(typeof(sbyte), true, false, true, true, "sbyte")]
    [InlineData(typeof(byte), true, false, true, true, "byte")]
    [InlineData(typeof(void), true, false, true, true, "void")]
    [InlineData(typeof(ushort), true, false, true, true, "ushort")]
    [InlineData(typeof(short), true, false, true, true, "short")]
    [InlineData(typeof(uint), true, false, true, true, "uint")]
    [InlineData(typeof(int), true, false, true, true, "int")]
    [InlineData(typeof(ulong), true, false, true, true, "ulong")]
    [InlineData(typeof(long), true, false, true, true, "long")]
    [InlineData(typeof(char), true, false, true, true, "char")]

    [InlineData(typeof(bool?), true, false, true, true, "bool?")]
    [InlineData(typeof(double?), true, false, true, true, "double?")]
    [InlineData(typeof(decimal?), true, false, true, true, "decimal?")]
    [InlineData(typeof(float?), true, false, true, true, "float?")]
    [InlineData(typeof(sbyte?), true, false, true, true, "sbyte?")]
    [InlineData(typeof(byte?), true, false, true, true, "byte?")]
    [InlineData(typeof(ushort?), true, false, true, true, "ushort?")]
    [InlineData(typeof(short?), true, false, true, true, "short?")]
    [InlineData(typeof(uint?), true, false, true, true, "uint?")]
    [InlineData(typeof(int?), true, false, true, true, "int?")]
    [InlineData(typeof(ulong?), true, false, true, true, "ulong?")]
    [InlineData(typeof(long?), true, false, true, true, "long?")]
    [InlineData(typeof(char?), true, false, true, true, "char?")]

    [InlineData(typeof(string), true, true, true, true, "String")]
    [InlineData(typeof(object), true, true, true, true, "Object")]
    [InlineData(typeof(bool), true, true, true, true, "Boolean")]
    [InlineData(typeof(double), true, true, true, true, "Double")]
    [InlineData(typeof(decimal), true, true, true, true, "Decimal")]
    [InlineData(typeof(float), true, true, true, true, "Single")]
    [InlineData(typeof(sbyte), true, true, true, true, "SByte")]
    [InlineData(typeof(byte), true, true, true, true, "Byte")]
    [InlineData(typeof(void), true, true, true, true, "Void")]
    [InlineData(typeof(ushort), true, true, true, true, "UInt16")]
    [InlineData(typeof(short), true, true, true, true, "Int16")]
    [InlineData(typeof(uint), true, true, true, true, "UInt32")]
    [InlineData(typeof(int), true, true, true, true, "Int32")]
    [InlineData(typeof(ulong), true, true, true, true, "UInt64")]
    [InlineData(typeof(long), true, true, true, true, "Int64")]
    [InlineData(typeof(char), true, true, true, true, "Char")]

    [InlineData(typeof(bool?), true, true, true, false, "Nullable<Boolean>")]
    [InlineData(typeof(double?), true, true, true, false, "Nullable<Double>")]
    [InlineData(typeof(decimal?), true, true, true, false, "Nullable<Decimal>")]
    [InlineData(typeof(float?), true, true, true, false, "Nullable<Single>")]
    [InlineData(typeof(sbyte?), true, true, true, false, "Nullable<SByte>")]
    [InlineData(typeof(byte?), true, true, true, false, "Nullable<Byte>")]
    [InlineData(typeof(ushort?), true, true, true, false, "Nullable<UInt16>")]
    [InlineData(typeof(short?), true, true, true, false, "Nullable<Int16>")]
    [InlineData(typeof(uint?), true, true, true, false, "Nullable<UInt32>")]
    [InlineData(typeof(int?), true, true, true, false, "Nullable<Int32>")]
    [InlineData(typeof(ulong?), true, true, true, false, "Nullable<UInt64>")]
    [InlineData(typeof(long?), true, true, true, false, "Nullable<Int64>")]
    [InlineData(typeof(char?), true, true, true, false, "Nullable<Char>")]

    [InlineData(typeof(TestClass), true, false, true, true, "TestClass")]
    [InlineData(typeof(TestClass.InnerTestClass), true, false, false, true, "TestClass+InnerTestClass")]
    [InlineData(typeof(TestClass.InnerTestClass), true, false, true, true, "JanHafner.TypeNameExtractor.Tests.TestClass+InnerTestClass")]
    [InlineData(typeof(TestClass.InnerTestClass.MostInnerTestClass), true, false, false, true, "TestClass+InnerTestClass+MostInnerTestClass")]
    [InlineData(typeof(TestClass.InnerTestClass.MostInnerTestClass), true, false, true, true, "JanHafner.TypeNameExtractor.Tests.TestClass+InnerTestClass+MostInnerTestClass")]
    [InlineData(typeof(GenericTestClass<>), true, false, true, true, "GenericTestClass<T>")]
    [InlineData(typeof(GenericTestClass<>), false, false, true, true, "GenericTestClass<>")]
    [InlineData(typeof(GenericTestClass<TestClass>), true, false, true, true, "GenericTestClass<TestClass>")]
    [InlineData(typeof(GenericTestClass<>.InnerNonGenericTestClass), false, false, false, true, "GenericTestClass<>+InnerNonGenericTestClass<>")]
    [InlineData(typeof(GenericTestClass<>.InnerNonGenericTestClass), false, false, true, true, "JanHafner.TypeNameExtractor.Tests.GenericTestClass<>+InnerNonGenericTestClass<>")]
    [InlineData(typeof(GenericTestClass<>.InnerNonGenericTestClass), true, false, false, true, "GenericTestClass<T>+InnerNonGenericTestClass<T>")]
    [InlineData(typeof(GenericTestClass<>.InnerNonGenericTestClass), true, false, true, true, "JanHafner.TypeNameExtractor.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>")]
    [InlineData(typeof(GenericTestClass<>.InnerNonGenericTestClass.MostInnerGenericTestClass<,>), true, false, false, true, "GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<T, R, M>")]
    [InlineData(typeof(GenericTestClass<>.InnerNonGenericTestClass.MostInnerGenericTestClass<,>), true, false, true, true, "JanHafner.TypeNameExtractor.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<T, R, M>")]
    [InlineData(typeof(GenericTestClass<>.InnerNonGenericTestClass.MostInnerGenericTestClass<,>), false, false, false, true, "GenericTestClass<>+InnerNonGenericTestClass<>+MostInnerGenericTestClass<,,>")]
    [InlineData(typeof(GenericTestClass<>.InnerNonGenericTestClass.MostInnerGenericTestClass<,>), false, false, true, true, "JanHafner.TypeNameExtractor.Tests.GenericTestClass<>+InnerNonGenericTestClass<>+MostInnerGenericTestClass<,,>")]

    [InlineData(typeof(GenericTestClass<double?>), true, true, true, false, "GenericTestClass<Nullable<Double>>")]
    [InlineData(typeof(GenericTestClass<double?>), true, false, true, true, "GenericTestClass<double?>")]
    [InlineData(typeof(GenericTestClass<double?>.InnerNonGenericTestClass), true, true, false, false, "GenericTestClass<T>+InnerNonGenericTestClass<Nullable<Double>>")]
    [InlineData(typeof(GenericTestClass<double?>.InnerNonGenericTestClass), true, false, false, true, "GenericTestClass<T>+InnerNonGenericTestClass<double?>")]
    [InlineData(typeof(GenericTestClass<double?>.InnerNonGenericTestClass), true, true, true, false, "JanHafner.TypeNameExtractor.Tests.GenericTestClass<T>+InnerNonGenericTestClass<Nullable<Double>>")]
    [InlineData(typeof(GenericTestClass<double?>.InnerNonGenericTestClass), true, false, true, true, "JanHafner.TypeNameExtractor.Tests.GenericTestClass<T>+InnerNonGenericTestClass<double?>")]
    [InlineData(typeof(GenericTestClass<double?>.InnerNonGenericTestClass.MostInnerGenericTestClass<double?, string>), true, false, true, true, "JanHafner.TypeNameExtractor.Tests.GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<double?, double?, string>")]

    [InlineData(typeof(string[]), true, false, true, true, "string[]")]
    [InlineData(typeof(GenericTestClass<string[]>), true, false, true, true, "GenericTestClass<string[]>")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass), true, false, false, true, "GenericTestClass<T>+InnerNonGenericTestClass<string[]>")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[]>), true, false, false, true, "GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<string[], IReadOnlyList<double?>, object[]>")]

    [InlineData(typeof(GenericTestClass<string[]>[]), true, false, true, true, "GenericTestClass<string[]>[]")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass[]), true, false, false, true, "GenericTestClass<T>+InnerNonGenericTestClass<string[]>[]")]
    [InlineData(typeof(GenericTestClass<string[]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[]>[]), true, false, false, true, "GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<string[], IReadOnlyList<double?>, object[]>[]")]

    [InlineData(typeof(string[][]), true, false, true, true, "string[][]")]
    [InlineData(typeof(string[,]), true, false, true, true, "string[,]")]
    [InlineData(typeof(string[,,]), true, false, true, true, "string[,,]")]
    [InlineData(typeof(string[,,][,]), true, false, true, true, "string[,][,,]")]
    [InlineData(typeof(string[,][,,]), true, false, true, true, "string[,,][,]")]
    [InlineData(typeof(string[][,,][,]), true, false, true, true, "string[,][,,][]")]
    [InlineData(typeof(GenericTestClass<string[,]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[,][]>[][,,]), true, false, false, true, "GenericTestClass<T>+InnerNonGenericTestClass<T>+MostInnerGenericTestClass<string[,], IReadOnlyList<double?>, object[][,]>[,,][]")]
    public void ExtractsReadableTypeName(Type type, bool outputTypeVariableNames, bool useClrTypeNameForPrimitiveTypes, bool fullQualifyOuterMostTypeNameOnNestedTypes, bool useNullableTypeShortForm, string expected)
    {
        // Arrange
        var typeNameExtractorOptions = new TypeNameExtractorOptions
        {
            FullQualifyOuterMostTypeNameOnNestedTypes = fullQualifyOuterMostTypeNameOnNestedTypes,
            OutputTypeVariableNames = outputTypeVariableNames,
            UseClrTypeNameForPrimitiveTypes = useClrTypeNameForPrimitiveTypes,
            UseNullableTypeShortForm = useNullableTypeShortForm,
        };
        var typeNameExtractor = new TypeNameExtractor(typeNameExtractorOptions);

        // Act
        var readableName = typeNameExtractor.ExtractReadableName(type);

        // Assert
        readableName.Should().Be(expected);
    }
}
