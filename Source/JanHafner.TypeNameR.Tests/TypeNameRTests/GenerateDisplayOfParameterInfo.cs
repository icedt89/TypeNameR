using FluentAssertions;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using System.Reflection;
using Xunit;

namespace JanHafner.TypeNameR.Tests.TypeNameRTests
{
    public sealed class GenerateDisplayOfParameterInfo
    {
        public static IEnumerable<object[]> GetParameterTests(Type type)
        {
            var methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                foreach (var parameter in parameters)
                {
                    var expectation = ExpectsAttribute.GetExpectation(parameters[0]);
                    if (expectation is not null)
                    {
                        yield return new object[] { method.Name, parameter.Position, expectation };
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetParameterTests), typeof(StringMethods))]
        public void GenerateStringParameterDisplay(string methodName, int parameterIndex, string expected)
        {
            // Arrange
            var typeNameR = new TypeNameR();

            var parameter = typeof(StringMethods).GetParameter(methodName, parameterIndex);

            // Act
            var generated = typeNameR.GenerateDisplay(parameter, NameRControlFlags.All);

            // Assert
            generated.Should().Be(expected);
        }

        [Theory]
        [InlineData(nameof(StringMethods.RefReturnValue), "ref string")]
        [InlineData(nameof(StringMethods.RefNullableReturnValue), "ref string?")]
        public void GenerateRefStringReturnParameterDisplay(string methodName, string expected)
        {
            // Arrange
            var typeNameR = new TypeNameR();

            var parameter = typeof(StringMethods).GetMethodOrThrow(methodName).ReturnParameter;

            // Act
            var generated = typeNameR.GenerateDisplay(parameter, NameRControlFlags.All);

            // Assert
            generated.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(GetParameterTests), typeof(IntMethods))]
        public void GenerateIntParameterDisplay(string methodName, int parameterIndex, string expected)
        {
            // Arrange
            var typeNameR = new TypeNameR();

            var parameter = typeof(IntMethods).GetParameter(methodName, parameterIndex);

            // Act
            var generated = typeNameR.GenerateDisplay(parameter, NameRControlFlags.All);

            // Assert
            generated.Should().Be(expected);
        }

        [Theory]
        [InlineData(nameof(IntMethods.RefReturnValue), "ref int")]
        [InlineData(nameof(IntMethods.RefNullableReturnValue), "ref int?")]
        public void GenerateRefIntReturnParameterDisplay(string methodName, string expected)
        {
            // Arrange
            var typeNameR = new TypeNameR();

            var parameter = typeof(IntMethods).GetMethodOrThrow(methodName).ReturnParameter;

            // Act
            var generated = typeNameR.GenerateDisplay(parameter, NameRControlFlags.All);

            // Assert
            generated.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(GetParameterTests), typeof(TestClassMethods))]
        public void GenerateTestClassParameterDisplay(string methodName, int parameterIndex, string expected)
        {
            // Arrange
            var typeNameR = new TypeNameR();

            var parameter = typeof(TestClassMethods).GetParameter(methodName, parameterIndex);

            // Act
            var generated = typeNameR.GenerateDisplay(parameter, NameRControlFlags.All);

            // Assert
            generated.Should().Be(expected);
        }

        [Theory]
        [InlineData(nameof(TestClassMethods.RefReturnValue), "ref TestClass")]
        [InlineData(nameof(TestClassMethods.RefNullableReturnValue), "ref TestClass?")]
        public void GenerateRefTestClassReturnParameterDisplay(string methodName, string expected)
        {
            // Arrange
            var typeNameR = new TypeNameR();

            var parameter = typeof(TestClassMethods).GetMethodOrThrow(methodName).ReturnParameter;

            // Act
            var generated = typeNameR.GenerateDisplay(parameter, NameRControlFlags.All);

            // Assert
            generated.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(GetParameterTests), typeof(TestStructMethods))]
        public void GenerateTestStructParameterDisplay(string methodName, int parameterIndex, string expected)
        {
            // Arrange
            var typeNameR = new TypeNameR();

            var parameter = typeof(TestStructMethods).GetParameter(methodName, parameterIndex);

            // Act
            var generated = typeNameR.GenerateDisplay(parameter, NameRControlFlags.All);

            // Assert
            generated.Should().Be(expected);
        }

        [Theory]
        [InlineData(nameof(TestStructMethods.RefReturnValue), "ref TestStruct")]
        [InlineData(nameof(TestStructMethods.RefNullableReturnValue), "ref TestStruct?")]
        public void GenerateRefTestStructReturnParameterDisplay(string methodName, string expected)
        {
            // Arrange
            var typeNameR = new TypeNameR();

            var parameter = typeof(TestStructMethods).GetMethodOrThrow(methodName).ReturnParameter;

            // Act
            var generated = typeNameR.GenerateDisplay(parameter, NameRControlFlags.All);

            // Assert
            generated.Should().Be(expected);
        }

        [Fact]
        public void GenerateThisPrefix()
        {
            // Arrange
            var typeNameR = new TypeNameR();

            var method = typeof(ExtensionMethodsClass).GetMethodOrThrow(nameof(ExtensionMethodsClass.This));
            var parameter = method.GetParameters()[0];

            var expected = ExpectsAttribute.GetExpectation(parameter);

            // Act
            var generated = typeNameR.GenerateDisplay(parameter, NameRControlFlags.All);

            // Assert
            generated.Should().Be(expected);
        }

        public static IEnumerable<object[]> GetDoesNotThrowAnExceptionTests(int take)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).SelectMany(a => a.GetExportedTypes()).ToArray();

            var parameters = types.SelectMany(t => t.GetMethods(BindingFlags.Instance
                                                                | BindingFlags.Public
                                                                | BindingFlags.Static
                                                                | BindingFlags.NonPublic
                                                                | BindingFlags.DeclaredOnly))
                .SelectMany(m => m.GetParameters()).Take(take)
                .Concat(types.SelectMany(t => t.GetMethods(BindingFlags.Instance
                                                           | BindingFlags.Public
                                                           | BindingFlags.Static
                                                           | BindingFlags.NonPublic
                                                           | BindingFlags.DeclaredOnly))
                    .SelectMany(m => m.GetParameters()).Take(take));

            foreach (var parameter in parameters)
            {
                yield return new[] { parameter };
            }
        }

        [Theory]
        [MemberData(nameof(GetDoesNotThrowAnExceptionTests), 500)]
        public void DoesNotThrowAnException(ParameterInfo parameter)
        {
            // Arrange
            var typeNameR = new TypeNameR();

            // Act, Assert
            var generated = typeNameR.GenerateDisplay(parameter, NameRControlFlags.All);

            generated.Should().NotBeNullOrWhiteSpace();
        }

#pragma warning disable CS8500
        private static unsafe class StringMethods
        {
            public static ref string RefReturnValue() => throw new NotImplementedException();

            public static ref string? RefNullableReturnValue() => throw new NotImplementedException();

            public static void Item([Expects("string param1")] string param1) => throw new NotImplementedException();

            // 'default' will be replaced with the types default value, here 'null'
            public static void ItemWithDefault([Expects("string param1 = null")] string param1 = default) => throw new NotImplementedException();

            public static void ItemWithExplicitDefault([Expects("string param1 = \"param2\"")] string param1 = "param2") => throw new NotImplementedException();

            public static void NullableItem([Expects("string? param1")] string? param1) => throw new NotImplementedException();

            // 'default' will be replaced with the types default value, here 'null'
            public static void NullableItemWithDefault([Expects("string? param1 = null")] string? param1 = default) => throw new NotImplementedException();

            // 'default' will be replaced with the types default value, here 'null'
            public static void NullableItemWithExplicitDefault([Expects("string? param1 = \"param2\"")] string? param1 = "param2")
                => throw new NotImplementedException();

            public static void PointerItem([Expects("string* param1")] string* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void PointerNullableItem([Expects("string* param1")] string?* param1) => throw new NotImplementedException();

            public static void RefItem([Expects("ref string param1")] ref string param1) => throw new NotImplementedException();

            public static void RefNullableItem([Expects("ref string? param1")] ref string? param1) => throw new NotImplementedException();

            public static void RefPointerItem([Expects("ref string* param1")] ref string* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void RefPointerNullableItem([Expects("ref string* param1")] ref string?* param1) => throw new NotImplementedException();

            public static void InItem([Expects("in string param1")] in string param1) => throw new NotImplementedException();

            public static void InNullableItem([Expects("in string? param1")] in string? param1) => throw new NotImplementedException();

            public static void InPointerItem([Expects("in string* param1")] in string* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void InPointerNullableItem([Expects("in string* param1")] in string?* param1) => throw new NotImplementedException();

            public static void OutItem([Expects("out string param1")] out string param1) => throw new NotImplementedException();

            public static void OutNullableItem([Expects("out string? param1")] out string? param1) => throw new NotImplementedException();

            public static void OutPointerItem([Expects("out string* param1")] out string* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void OutPointerNullableItem([Expects("out string* param1")] out string?* param1) => throw new NotImplementedException();

            public static void ParamsArray([Expects("params string[] param1")] params string[] param1) => throw new NotImplementedException();

            public static void Array([Expects("string[] param1")] string[] param1) => throw new NotImplementedException();

            public static void NullableArray([Expects("string[]? param1")] string[]? param1) => throw new NotImplementedException();

            public static void NullableParamsArray([Expects("params string[]? param1")] params string[]? param1) => throw new NotImplementedException();

            public static void ArrayWithNullableItem([Expects("string?[] param1")] string?[] param1) => throw new NotImplementedException();

            public static void NullableArrayWithNullableItem([Expects("string?[]? param1")] string?[]? param1) => throw new NotImplementedException();

            public static void RefArray([Expects("ref string[] param1")] ref string[] param1) => throw new NotImplementedException();

            public static void RefNullableArray([Expects("ref string[]? param1")] ref string[]? param1) => throw new NotImplementedException();

            public static void RefArrayWithNullableItem([Expects("ref string?[] param1")] ref string?[] param1) => throw new NotImplementedException();

            public static void RefNullableArrayWithNullableItem([Expects("ref string?[]? param1")] ref string?[]? param1)
                => throw new NotImplementedException();

            public static void InArray([Expects("in string[] param1")] in string[] param1) => throw new NotImplementedException();

            public static void InNullableArray([Expects("in string[]? param1")] in string[]? param1) => throw new NotImplementedException();

            public static void InArrayWithNullableItem([Expects("in string?[] param1")] in string?[] param1) => throw new NotImplementedException();

            public static void InNullableArrayWithNullableItem([Expects("in string?[]? param1")] in string?[]? param1) => throw new NotImplementedException();

            public static void OutArray([Expects("out string[] param1")] out string[] param1) => throw new NotImplementedException();

            public static void OutNullableArray([Expects("out string[]? param1")] out string[]? param1) => throw new NotImplementedException();

            public static void OutArrayWithNullableItem([Expects("out string?[] param1")] out string?[] param1) => throw new NotImplementedException();

            public static void OutNullableArrayWithNullableItem([Expects("out string?[]? param1")] out string?[]? param1)
                => throw new NotImplementedException();

            // Does not compile
            // public static void PointerOfArray(string[]* param1) => throw new NotImplementedException();

            // Does not compile
            // public static void RefPointerOfArray(ref string[]* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void PointerOfNullableArray([Expects("string?[]* param1")] string[]?* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void PointerOfNullableArrayWithNullableItem([Expects("string?[]* param1")] string?[]?* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void RefPointerOfNullableArray([Expects("ref string[]* param1")] ref string[]?* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void RefPointerOfNullableArrayWithNullableItem([Expects("ref string[]* param1")] ref string?[]?* param1)
                => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void InPointerOfNullableArray([Expects("in string[]* param1")] in string[]?* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void InPointerOfNullableArrayWithNullableItem([Expects("in string[]* param1")] in string?[]?* param1)
                => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void OutPointerOfNullableArray([Expects("out string[]* param1")] out string[]?* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void OutPointerOfNullableArrayWithNullableItem([Expects("out string[]* param1")] out string?[]?* param1)
                => throw new NotImplementedException();

            // Does not compile
            // public static void PointerOfArrayWithNullableItem(string?[]* param1) => throw new NotImplementedException();

            // Does not compile
            // public static void RefPointerOfArrayWithNullableItem(ref string?[]* param1) => throw new NotImplementedException();
        }

        private static unsafe class IntMethods
        {
            public static ref int RefReturnValue() => throw new NotImplementedException();

            public static ref int? RefNullableReturnValue() => throw new NotImplementedException();

            public static void Item([Expects("int param1")] int param1) => throw new NotImplementedException();

            // 'default' will be replaced with the types default value, here '0'
            public static void ItemWithDefault([Expects("int param1 = 0")] int param1 = default) => throw new NotImplementedException();

            public static void ItemWithExplicitDefault([Expects("int param1 = 2")] int param1 = 2) => throw new NotImplementedException();

            public static void NullableItem([Expects("int? param1")] int? param1) => throw new NotImplementedException();

            // 'default' will be replaced with the types default value, here '0'
            public static void NullableItemWithDefault([Expects("int? param1 = null")] int? param1 = default) => throw new NotImplementedException();

            public static void PointerItem([Expects("int* param1")] int* param1) => throw new NotImplementedException();

            public static void PointerNullableItem([Expects("int?* param1")] int?* param1) => throw new NotImplementedException();

            public static void RefItem([Expects("ref int param1")] ref int param1) => throw new NotImplementedException();

            public static void RefNullableItem([Expects("ref int? param1")] ref int? param1) => throw new NotImplementedException();

            public static void RefPointerItem([Expects("ref int* param1")] ref int* param1) => throw new NotImplementedException();

            public static void RefPointerNullableItem([Expects("ref int?* param1")] ref int?* param1) => throw new NotImplementedException();

            public static void InItem([Expects("in int param1")] in int param1) => throw new NotImplementedException();

            public static void InNullableItem([Expects("in int? param1")] in int? param1) => throw new NotImplementedException();

            public static void InPointerItem([Expects("in int* param1")] in int* param1) => throw new NotImplementedException();

            public static void InPointerNullableItem([Expects("in int?* param1")] in int?* param1) => throw new NotImplementedException();

            public static void OutItem([Expects("out int param1")] out int param1) => throw new NotImplementedException();

            public static void OutNullableItem([Expects("out int? param1")] out int? param1) => throw new NotImplementedException();

            public static void OutPointerItem([Expects("out int* param1")] out int* param1) => throw new NotImplementedException();

            public static void OutPointerNullableItem([Expects("out int?* param1")] out int?* param1) => throw new NotImplementedException();

            public static void Array([Expects("int[] param1")] int[] param1) => throw new NotImplementedException();

            public static void ParamsArray([Expects("params int[] param1")] params int[] param1) => throw new NotImplementedException();

            public static void NullableArray([Expects("int[]? param1")] int[]? param1) => throw new NotImplementedException();

            public static void NullableParamsArray([Expects("params int[]? param1")] params int[]? param1) => throw new NotImplementedException();

            public static void ArrayWithNullableItem([Expects("int?[] param1")] int?[] param1) => throw new NotImplementedException();

            public static void NullableArrayWithNullableItem([Expects("int?[]? param1")] int?[]? param1) => throw new NotImplementedException();

            public static void RefArray([Expects("ref int[] param1")] ref int[] param1) => throw new NotImplementedException();

            public static void RefNullableArray([Expects("ref int[]? param1")] ref int[]? param1) => throw new NotImplementedException();

            public static void RefArrayWithNullableItem([Expects("ref int?[] param1")] ref int?[] param1) => throw new NotImplementedException();

            public static void RefNullableArrayWithNullableItem([Expects("ref int?[]? param1")] ref int?[]? param1) => throw new NotImplementedException();

            public static void InArray([Expects("in int[] param1")] in int[] param1) => throw new NotImplementedException();

            public static void InNullableArray([Expects("in int[]? param1")] in int[]? param1) => throw new NotImplementedException();

            public static void InArrayWithNullableItem([Expects("in int?[] param1")] in int?[] param1) => throw new NotImplementedException();

            public static void InNullableArrayWithNullableItem([Expects("in int?[]? param1")] in int?[]? param1) => throw new NotImplementedException();

            public static void OutArray([Expects("out int[] param1")] out int[] param1) => throw new NotImplementedException();

            public static void OutNullableArray([Expects("out int[]? param1")] out int[]? param1) => throw new NotImplementedException();

            public static void OutArrayWithNullableItem([Expects("out int?[] param1")] out int?[] param1) => throw new NotImplementedException();

            public static void OutNullableArrayWithNullableItem([Expects("out int?[]? param1")] out int?[]? param1) => throw new NotImplementedException();

            // Does not compile
            // public static void PointerOfArray(int[]* param1) => throw new NotImplementedException();

            // Does not compile
            // public static void RefPointerOfArray(ref int[]* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void PointerOfNullableArray([Expects("int[]* param1")] int[]?* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void PointerOfNullableArrayWithNullableItem([Expects("int?[]* param1")] int?[]?* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void RefPointerOfNullableArray([Expects("ref int[]* param1")] ref int[]?* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void RefPointerOfNullableArrayWithNullableItem([Expects("ref int?[]* param1")] ref int?[]?* param1)
                => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void InPointerOfNullableArray([Expects("in int[]* param1")] in int[]?* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void InPointerOfNullableArrayWithNullableItem([Expects("in int?[]* param1")] in int?[]?* param1)
                => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void OutPointerOfNullableArray([Expects("out int[]* param1")] out int[]?* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void OutPointerOfNullableArrayWithNullableItem([Expects("out int?[]* param1")] out int?[]?* param1)
                => throw new NotImplementedException();

            // Does not compile
            // public static void PointerOfArrayWithNullableItem(int?[]* param1) => throw new NotImplementedException();

            // Does not compile
            // public static void RefPointerOfArrayWithNullableItem(ref int?[]* param1) => throw new NotImplementedException();
        }

        private static unsafe class TestClassMethods
        {
            public static ref TestClass RefReturnValue() => throw new NotImplementedException();

            public static ref TestClass? RefNullableReturnValue() => throw new NotImplementedException();

            public static void Item([Expects("TestClass param1")] TestClass param1) => throw new NotImplementedException();

            // 'default' will be replaced with the types default value, here 'null'
            public static void ItemWithDefault([Expects("TestClass param1 = null")] TestClass param1 = default) => throw new NotImplementedException();

            public static void ItemWithExplicitDefault([Expects("TestClass param1 = null")] TestClass param1 = null) => throw new NotImplementedException();

            public static void NullableItem([Expects("TestClass? param1")] TestClass? param1) => throw new NotImplementedException();

            // 'default' will be replaced with the types default value, here 'null'
            public static void NullableItemWithDefault([Expects("TestClass? param1 = null")] TestClass? param1 = default)
                => throw new NotImplementedException();

            public static void NullableItemWithExplicitDefault([Expects("TestClass? param1 = null")] TestClass? param1 = null)
                => throw new NotImplementedException();

            public static void PointerItem([Expects("TestClass* param1")] TestClass* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void PointerNullableItem([Expects("TestClass* param1")] TestClass?* param1) => throw new NotImplementedException();

            public static void RefItem([Expects("ref TestClass param1")] ref TestClass param1) => throw new NotImplementedException();

            public static void RefNullableItem([Expects("ref TestClass? param1")] ref TestClass? param1) => throw new NotImplementedException();

            public static void RefPointerItem([Expects("ref TestClass* param1")] ref TestClass* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void RefPointerNullableItem([Expects("ref TestClass* param1")] ref TestClass?* param1) => throw new NotImplementedException();

            public static void InItem([Expects("in TestClass param1")] in TestClass param1) => throw new NotImplementedException();

            public static void InNullableItem([Expects("in TestClass? param1")] in TestClass? param1) => throw new NotImplementedException();

            public static void InPointerItem([Expects("in TestClass* param1")] in TestClass* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void InPointerNullableItem([Expects("in TestClass* param1")] in TestClass?* param1) => throw new NotImplementedException();

            public static void OutItem([Expects("out TestClass param1")] out TestClass param1) => throw new NotImplementedException();

            public static void OutNullableItem([Expects("out TestClass? param1")] out TestClass? param1) => throw new NotImplementedException();

            public static void OutPointerItem([Expects("out TestClass* param1")] out TestClass* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void OutPointerNullableItem([Expects("out TestClass* param1")] out TestClass?* param1) => throw new NotImplementedException();

            public static void Array([Expects("TestClass[] param1")] TestClass[] param1) => throw new NotImplementedException();

            public static void ParamsArray([Expects("params TestClass[] param1")] params TestClass[] param1) => throw new NotImplementedException();

            public static void NullableArray([Expects("TestClass[]? param1")] TestClass[]? param1) => throw new NotImplementedException();

            public static void NullableParamsArray([Expects("params TestClass[]? param1")] params TestClass[]? param1) => throw new NotImplementedException();

            public static void ArrayWithNullableItem([Expects("TestClass?[] param1")] TestClass?[] param1) => throw new NotImplementedException();

            public static void NullableArrayWithNullableItem([Expects("TestClass?[]? param1")] TestClass?[]? param1) => throw new NotImplementedException();

            public static void RefArray([Expects("ref TestClass[] param1")] ref TestClass[] param1) => throw new NotImplementedException();

            public static void RefNullableArray([Expects("ref TestClass[]? param1")] ref TestClass[]? param1) => throw new NotImplementedException();

            public static void RefArrayWithNullableItem([Expects("ref TestClass?[] param1")] ref TestClass?[] param1) => throw new NotImplementedException();

            public static void RefNullableArrayWithNullableItem([Expects("ref TestClass?[]? param1")] ref TestClass?[]? param1)
                => throw new NotImplementedException();

            public static void InArray([Expects("in TestClass[] param1")] in TestClass[] param1) => throw new NotImplementedException();

            public static void InNullableArray([Expects("in TestClass[]? param1")] in TestClass[]? param1) => throw new NotImplementedException();

            public static void InArrayWithNullableItem([Expects("in TestClass?[] param1")] in TestClass?[] param1) => throw new NotImplementedException();

            public static void InNullableArrayWithNullableItem([Expects("in TestClass?[]? param1")] in TestClass?[]? param1)
                => throw new NotImplementedException();

            public static void OutArray([Expects("out TestClass[] param1")] out TestClass[] param1) => throw new NotImplementedException();

            public static void OutNullableArray([Expects("out TestClass[]? param1")] out TestClass[]? param1) => throw new NotImplementedException();

            public static void OutArrayWithNullableItem([Expects("out TestClass?[] param1")] out TestClass?[] param1) => throw new NotImplementedException();

            public static void OutNullableArrayWithNullableItem([Expects("out TestClass?[]? param1")] out TestClass?[]? param1)
                => throw new NotImplementedException();

            // Does not compile
            // public static void PointerOfArray(TestClass[]* param1) => throw new NotImplementedException();

            // Does not compile
            // public static void RefPointerOfArray(ref TestClass[]* param1) => throw new NotImplementedException();

            public static void PointerOfNullableArray([Expects("TestClass?[]* param1")] TestClass[]?* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void PointerOfNullableArrayWithNullableItem([Expects("TestClass?[]* param1")] TestClass?[]?* param1)
                => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void RefPointerOfNullableArray([Expects("ref TestClass[]* param1")] ref TestClass[]?* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void RefPointerOfNullableArrayWithNullableItem([Expects("ref TestClass[]* param1")] ref TestClass?[]?* param1)
                => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void InPointerOfNullableArray([Expects("in TestClass[]* param1")] in TestClass[]?* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void InPointerOfNullableArrayWithNullableItem([Expects("in TestClass[]* param1")] in TestClass?[]?* param1)
                => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void OutPointerOfNullableArray([Expects("out TestClass[]* param1")] out TestClass[]?* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void OutPointerOfNullableArrayWithNullableItem([Expects("out TestClass[]* param1")] out TestClass?[]?* param1)
                => throw new NotImplementedException();

            // Does not compile
            // public static void PointerOfArrayWithNullableItem(TestClass?[]* param1) => throw new NotImplementedException();

            // Does not compile
            // public static void RefPointerOfArrayWithNullableItem(ref TestClass?[]* param1) => throw new NotImplementedException();
        }

        private static unsafe class TestStructMethods
        {
            public static ref TestStruct RefReturnValue() => throw new NotImplementedException();

            public static ref TestStruct? RefNullableReturnValue() => throw new NotImplementedException();

            public static void Item([Expects("TestStruct param1")] TestStruct param1) => throw new NotImplementedException();

            public static void ItemWithDefault([Expects("TestStruct param1 = default")] TestStruct param1 = default) => throw new NotImplementedException();

            public static void NullableItem([Expects("TestStruct? param1")] TestStruct? param1) => throw new NotImplementedException();

            public static void NullableItemWithDefault([Expects("TestStruct? param1 = null")] TestStruct? param1 = default)
                => throw new NotImplementedException();

            public static void PointerItem([Expects("TestStruct* param1")] TestStruct* param1) => throw new NotImplementedException();

            public static void PointerNullableItem([Expects("TestStruct?* param1")] TestStruct?* param1) => throw new NotImplementedException();

            public static void RefItem([Expects("ref TestStruct param1")] ref TestStruct param1) => throw new NotImplementedException();

            public static void RefNullableItem([Expects("ref TestStruct? param1")] ref TestStruct? param1) => throw new NotImplementedException();

            public static void RefPointerItem([Expects("ref TestStruct* param1")] ref TestStruct* param1) => throw new NotImplementedException();

            public static void RefPointerNullableItem([Expects("ref TestStruct?* param1")] ref TestStruct?* param1) => throw new NotImplementedException();

            public static void InItem([Expects("in TestStruct param1")] in TestStruct param1) => throw new NotImplementedException();

            public static void InNullableItem([Expects("in TestStruct? param1")] in TestStruct? param1) => throw new NotImplementedException();

            public static void InPointerItem([Expects("in TestStruct* param1")] in TestStruct* param1) => throw new NotImplementedException();

            public static void InPointerNullableItem([Expects("in TestStruct?* param1")] in TestStruct?* param1) => throw new NotImplementedException();

            public static void OutItem([Expects("out TestStruct param1")] out TestStruct param1) => throw new NotImplementedException();

            public static void OutNullableItem([Expects("out TestStruct? param1")] out TestStruct? param1) => throw new NotImplementedException();

            public static void OutPointerItem([Expects("out TestStruct* param1")] out TestStruct* param1) => throw new NotImplementedException();

            public static void OutPointerNullableItem([Expects("out TestStruct?* param1")] out TestStruct?* param1) => throw new NotImplementedException();

            public static void Array([Expects("TestStruct[] param1")] TestStruct[] param1) => throw new NotImplementedException();

            public static void ParamsArray([Expects("params TestStruct[] param1")] params TestStruct[] param1) => throw new NotImplementedException();

            public static void NullableArray([Expects("TestStruct[]? param1")] TestStruct[]? param1) => throw new NotImplementedException();

            public static void NullableParamsArray([Expects("params TestStruct[]? param1")] params TestStruct[]? param1) => throw new NotImplementedException();

            public static void ArrayWithNullableItem([Expects("TestStruct?[] param1")] TestStruct?[] param1) => throw new NotImplementedException();

            public static void NullableArrayWithNullableItem([Expects("TestStruct?[]? param1")] TestStruct?[]? param1) => throw new NotImplementedException();

            public static void RefArray([Expects("ref TestStruct[] param1")] ref TestStruct[] param1) => throw new NotImplementedException();

            public static void RefNullableArray([Expects("ref TestStruct[]? param1")] ref TestStruct[]? param1) => throw new NotImplementedException();

            public static void RefArrayWithNullableItem([Expects("ref TestStruct?[] param1")] ref TestStruct?[] param1) => throw new NotImplementedException();

            public static void RefNullableArrayWithNullableItem([Expects("ref TestStruct?[]? param1")] ref TestStruct?[]? param1)
                => throw new NotImplementedException();

            public static void InArray([Expects("in TestStruct[] param1")] in TestStruct[] param1) => throw new NotImplementedException();

            public static void InNullableArray([Expects("in TestStruct[]? param1")] in TestStruct[]? param1) => throw new NotImplementedException();

            public static void InArrayWithNullableItem([Expects("in TestStruct?[] param1")] in TestStruct?[] param1) => throw new NotImplementedException();

            public static void InNullableArrayWithNullableItem([Expects("in TestStruct?[]? param1")] in TestStruct?[]? param1)
                => throw new NotImplementedException();

            public static void OutArray([Expects("out TestStruct[] param1")] out TestStruct[] param1) => throw new NotImplementedException();

            public static void OutNullableArray([Expects("out TestStruct[]? param1")] out TestStruct[]? param1) => throw new NotImplementedException();

            public static void OutArrayWithNullableItem([Expects("out TestStruct?[] param1")] out TestStruct?[] param1) => throw new NotImplementedException();

            public static void OutNullableArrayWithNullableItem([Expects("out TestStruct?[]? param1")] out TestStruct?[]? param1)
                => throw new NotImplementedException();

            // Does not compile
            // public static void PointerOfArray(TestStruct[]* param1) => throw new NotImplementedException();

            // Does not compile
            // public static void RefPointerOfArray(ref TestStruct[]* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void PointerOfNullableArray([Expects("TestStruct[]* param1")] TestStruct[]?* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void PointerOfNullableArrayWithNullableItem([Expects("TestStruct?[]* param1")] TestStruct?[]?* param1)
                => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void RefPointerOfNullableArray([Expects("ref TestStruct[]* param1")] ref TestStruct[]?* param1)
                => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void RefPointerOfNullableArrayWithNullableItem([Expects("ref TestStruct?[]* param1")] ref TestStruct?[]?* param1)
                => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void InPointerOfNullableArray([Expects("in TestStruct[]* param1")] in TestStruct[]?* param1) => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void InPointerOfNullableArrayWithNullableItem([Expects("in TestStruct?[]* param1")] in TestStruct?[]?* param1)
                => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void OutPointerOfNullableArray([Expects("out TestStruct[]* param1")] out TestStruct[]?* param1)
                => throw new NotImplementedException();

            // NullabilityInfoContext does not provide enough information
            public static void OutPointerOfNullableArrayWithNullableItem([Expects("out TestStruct?[]* param1")] out TestStruct?[]?* param1)
                => throw new NotImplementedException();

            // Does not compile
            // public static void PointerOfArrayWithNullableItem(TestStruct?[]* param1) => throw new NotImplementedException();

            // Does not compile
            // public static void RefPointerOfArrayWithNullableItem(ref TestStruct?[]* param1) => throw new NotImplementedException();
        }
#pragma warning restore CS8500
    }
}