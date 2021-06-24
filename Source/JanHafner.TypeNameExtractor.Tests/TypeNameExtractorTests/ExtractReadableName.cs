using FluentAssertions;
using System;
using Xunit;

namespace JanHafner.TypeNameExtractor.Tests.TypeNameExtractorTests
{
    public sealed class ExtractReadableName
    {
        [Fact]
        public void ExtractsTheReadableNameOfASimpleTypeCorrectly()
        {
            // Arrange
            var type = typeof(TestClass);

            var readableTypeNameExtractor = new TypeNameExtractor();

            // Act
            var typeName = readableTypeNameExtractor.ExtractReadableName(type);

            // Assert
            typeName.Should().Be("TestClass");
        }

        [Fact]
        public void ExtractsTheReadableNameOfAnOpenGenericTypeCorrectly()
        {
            // Arrange
            var type = typeof(GenericTestClass<,>);

            var readableTypeNameExtractor = new TypeNameExtractor();

            // Act
            var typeName = readableTypeNameExtractor.ExtractReadableName(type);

            // Assert
            typeName.Should().Be("GenericTestClass<,>");
        }

        [Fact]
        public void ExtractsTheReadableNameOfAClosedGenericTypeCorrectly()
        {
            // Arrange
            var type = typeof(GenericTestClass<TestClass, GenericTestClass<TestClass, TestClass>>);

            var readableTypeNameExtractor = new TypeNameExtractor();

            // Act
            var typeName = readableTypeNameExtractor.ExtractReadableName(type);

            // Assert
            typeName.Should().Be("GenericTestClass<TestClass, GenericTestClass<TestClass, TestClass>>");
        }

        [Fact]
        public void ExtractsTheReadableNameOfAnOpenGenericTypeCorrectlyButOutputGenericTypeVariableNames()
        {
            // Arrange
            var type = typeof(GenericTestClass<,>);

            var readableTypeNameExtractor = new TypeNameExtractor(true);

            // Act
            var typeName = readableTypeNameExtractor.ExtractReadableName(type);

            // Assert
            typeName.Should().Be("GenericTestClass<T, R>");
        }

        [Fact]
        public void ThrowsExceptionIfTheSuppliedTypeIsNull()
        {
            // Arrange
            Type type = null;

            var readableTypeNameExtractor = new TypeNameExtractor();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => readableTypeNameExtractor.ExtractReadableName(type));
        }
    }
}
