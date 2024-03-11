using FluentAssertions;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using System.Reflection;
using Xunit;

namespace JanHafner.TypeNameR.Tests.TypeNameRTests
{
    public sealed class GenerateDisplayOfMethodBase
    {
        public static IEnumerable<object[]> GetMethodTests(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Instance
                                          | BindingFlags.Public
                                          | BindingFlags.Static
                                          | BindingFlags.NonPublic
                                          | BindingFlags.DeclaredOnly);
            foreach (var method in methods)
            {
                var expectation = ExpectsAttribute.GetExpectation(method);
                if (expectation is not null)
                {
                    yield return new object[] { method.Name, expectation };
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetMethodTests), typeof(Methods))]
        public void GenerateMethodDisplay(string methodName, string expected)
        {
            // Arrange
            var typeNameR = GlobalTestSettings.TypeNameR ?? new TypeNameR();

            var methodInfo = typeof(Methods).GetMethodOrThrow(methodName);

            // Act
            var generated = typeNameR.GenerateDisplay(methodInfo, NameRControlFlags.All
                                                                  & ~NameRControlFlags.PrependFullTypeNameBeforeMethodName
                                                                  | NameRControlFlags.IncludeDynamic);

            // Assert
            generated.Should().Be(expected);
        }

        [Fact]
        public void GeneratePublicConstructorDisplay()
        {
            // Arrange
            var typeNameR = GlobalTestSettings.TypeNameR ?? new TypeNameR();

            var constructor = typeof(Methods).GetPublicParameterlessConstructor();
            var expected = ExpectsAttribute.GetExpectation(constructor);

            // Act
            var generated = typeNameR.GenerateDisplay(constructor, NameRControlFlags.All
                                                                   & ~NameRControlFlags.PrependFullTypeNameBeforeMethodName);

            // Assert
            generated.Should().Be(expected);
        }

        [Fact]
        public void GeneratePrivateConstructorDisplay()
        {
            // Arrange
            var typeNameR = GlobalTestSettings.TypeNameR ?? new TypeNameR();

            var constructor = typeof(Methods).GetPrivateParameterlessDistinguishConstructor();
            var expected = ExpectsAttribute.GetExpectation(constructor);

            // Act
            var generated = typeNameR.GenerateDisplay(constructor, NameRControlFlags.All
                                                                   & ~NameRControlFlags.PrependFullTypeNameBeforeMethodName);

            // Assert
            generated.Should().Be(expected);
        }

        [Fact]
        public void GenerateStaticConstructorDisplay()
        {
            // Arrange
            var typeNameR = GlobalTestSettings.TypeNameR ?? new TypeNameR();

            var constructor = typeof(Methods).GetStaticParameterlessConstructor();
            var expected = ExpectsAttribute.GetExpectation(constructor);

            // Act
            var generated = typeNameR.GenerateDisplay(constructor, NameRControlFlags.All
                                                                   & ~NameRControlFlags.PrependFullTypeNameBeforeMethodName);

            // Assert
            generated.Should().Be(expected);
        }

        public static IEnumerable<object[]> GetDoesNotThrowAnExceptionTests(int take)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).SelectMany(a => a.GetExportedTypes()).ToArray();

            var methods = types.SelectMany(t => t.GetMethods(BindingFlags.Instance
                                                             | BindingFlags.Public
                                                             | BindingFlags.Static
                                                             | BindingFlags.NonPublic
                                                             | BindingFlags.DeclaredOnly)).Take(take)
                .Cast<MethodBase>()
                .Concat(types.SelectMany(t => t.GetConstructors(BindingFlags.Instance
                                                                | BindingFlags.Public
                                                                | BindingFlags.Static
                                                                | BindingFlags.NonPublic
                                                                | BindingFlags.DeclaredOnly)).Take(take));

            foreach (var method in methods)
            {
                yield return [method];
            }
        }

        [Theory]
        [MemberData(nameof(GetDoesNotThrowAnExceptionTests), GlobalTestSettings.Take)]
        public void DoesNotThrowAnException(MethodBase method)
        {
            // Arrange
            var typeNameR = GlobalTestSettings.TypeNameR ?? new TypeNameR();

            // Act, Assert
            var generated = typeNameR.GenerateDisplay(method, NameRControlFlags.All);

            generated.Should().NotBeNullOrWhiteSpace();
        }
    }

    public sealed class Methods
    {
        [Expects("private static .cctor()")]
        static Methods()
        {
            throw new NotImplementedException();
        }

        [Expects("public .ctor()")]
        public Methods()
        {
            throw new NotImplementedException();
        }

        [Expects("private .ctor(bool distinguish)")]
        private Methods(bool distinguish) // Used to distinguish the instance constructors
        {
            throw new NotImplementedException();
        }

        [Expects("public void PublicVoid()")]
        public void PublicVoid() => throw new NotImplementedException();

        [Expects("public static dynamic PublicStaticDynamic(dynamic param1)")]
        public static dynamic PublicStaticDynamic(dynamic param1) => throw new NotImplementedException();

        [Expects("public static ref dynamic PublicStaticRefDynamic(dynamic param1)")]
        public static ref dynamic PublicStaticRefDynamic(dynamic param1) => throw new NotImplementedException();

        [Expects("public static ref dynamic PublicStaticRefDynamicWithRefDynamicParameter(ref dynamic param1)")]
        public static ref dynamic PublicStaticRefDynamicWithRefDynamicParameter(ref dynamic param1) => throw new NotImplementedException();

        [Expects("public static ref dynamic PublicStaticRefDynamicWithInDynamicParameter(in dynamic param1)")]
        public static ref dynamic PublicStaticRefDynamicWithInDynamicParameter(in dynamic param1) => throw new NotImplementedException();

        [Expects("public static ref dynamic PublicStaticRefDynamicWithOutDynamicParameter(out dynamic param1)")]
        public static ref dynamic PublicStaticRefDynamicWithOutDynamicParameter(out dynamic param1) => throw new NotImplementedException();

        [Expects("public static void PublicStaticVoid()")]
        public static void PublicStaticVoid() => throw new NotImplementedException();

        [Expects("public async void PublicAsyncVoid()")]
        public async void PublicAsyncVoid() => await CallMeAsync();

        [Expects("public static async void PublicStaticAsyncVoid()")]
        public static async void PublicStaticAsyncVoid() => await CallMeAsync();

        [Expects("private void PrivateVoid()")]
        private void PrivateVoid() => throw new NotImplementedException();

        [Expects("private static void PrivateStaticVoid()")]
        private static void PrivateStaticVoid() => throw new NotImplementedException();

        [Expects("private async void PrivateAsyncVoid()")]
        private async void PrivateAsyncVoid() => await CallMeAsync();

        [Expects("private static async void PrivateStaticAsyncVoid()")]
        private static async void PrivateStaticAsyncVoid() => await CallMeAsync();

        [Expects("private static async Task PrivateStaticAsyncTask()")]
        private static async Task PrivateStaticAsyncTask() => await CallMeAsync();

        private static Task CallMeAsync() => throw new NotImplementedException();
    }
}