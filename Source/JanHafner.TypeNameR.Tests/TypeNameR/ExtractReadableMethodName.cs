using FluentAssertions;
using Xunit;

namespace JanHafner.TypeNameR.Tests.TypeNameR;

public sealed class ExtractReadableMethodName
{
    [Theory]
    [InlineData(nameof(MethodsClass.StaticMethod), "public static void StaticMethod()")]
    [InlineData(nameof(MethodsClass.NonNullableReturnType), "public string NonNullableReturnType()")]
    [InlineData(nameof(MethodsClass.NullableReturnType), "public string? NullableReturnType()")]
    [InlineData(nameof(MethodsClass.VoidMethod), "public void VoidMethod()")]
    [InlineData(nameof(MethodsClass.NonNullableParameter), "public void NonNullableParameter(string string1)")]
    [InlineData(nameof(MethodsClass.NonNullableParameterWithExactDefaultValue), "public void NonNullableParameterWithExactDefaultValue(string string1 = \"string1\")")]
    [InlineData(nameof(MethodsClass.NullableParameterWithExactDefaultValue), "public void NullableParameterWithExactDefaultValue(string? string1 = \"string1\")")]
    [InlineData(nameof(MethodsClass.OutParameter), "public void OutParameter(out string string1)")]
    [InlineData(nameof(MethodsClass.InParameter), "public void InParameter(in string string1)")]
    [InlineData(nameof(MethodsClass.RefParameter), "public void RefParameter(ref string string1)")]
    [InlineData(nameof(MethodsClass.NullableParameterWithDefaultValue), "public void NullableParameterWithDefaultValue(object object1 = default)")]
    [InlineData(nameof(MethodsClass.NonNullableParameterWithDefaultValue), "public void NonNullableParameterWithDefaultValue(CancellationToken cancellationToken = default)")]
    [InlineData(nameof(MethodsClass.NonNullableIntParameterWithExactDefaultValue), "public void NonNullableIntParameterWithExactDefaultValue(int int1 = 1)")]
    [InlineData(nameof(MethodsClass.NullableIntParameterWithExactDefaultValue), "public void NullableIntParameterWithExactDefaultValue(int? int1 = 1)")]
    [InlineData(nameof(MethodsClass.TaskMethod), "public Task TaskMethod()")]
    [InlineData(nameof(MethodsClass.StaticAsyncComplexMethod1), "public static Task<string?> StaticAsyncComplexMethod1(ref string? string1, out string? string2, in string string3 = \"string3\", CancellationToken cancellationToken = default)")]
    [InlineData(nameof(MethodsClass.UnsafeStaticArrayPointerParameter), "public static Task<int?> UnsafeStaticArrayPointerParameter(int?****[] int1 = default)")]
    [InlineData(nameof(MethodsClass.GenericRefNullable), "public static Task<TResult> GenericRefNullable<TResult>(ref int? int1)")]
    [InlineData(nameof(MethodsClass.GenericRefNonNullable), "public static Task<TResult> GenericRefNonNullable<TResult>(ref int int1)")]
    [InlineData(nameof(MethodsClass.GenericNullable), "public static Task<TResult> GenericNullable<TResult>(int? int1)")]
    [InlineData(nameof(MethodsClass.RefNullable), "public static Task RefNullable(ref int? int1)")]
    [InlineData(nameof(MethodsClass.RefNonNullable), "public static Task RefNonNullable(ref int int1)")]
    [InlineData(nameof(MethodsClass.Nullable), "public static Task Nullable(int? int1)")]
    [InlineData(nameof(MethodsClass.NullableReferenceTypeArray), "public static void NullableReferenceTypeArray(string?[]? string1)")]
    public void ProcessMethod(string methodName, string expectedReadableName)
    {
        // Arrange
        var typeNameR = new JanHafner.TypeNameR.TypeNameR();

        var methodInfo = typeof(MethodsClass).GetMethod(methodName) ?? throw new InvalidOperationException("Method");

        // Act
        var readableMethodName = typeNameR.ExtractReadable(methodInfo, NameRControlFlags.All
                                                                    & ~NameRControlFlags.IncludeFullTypeName);

        // Assert
        readableMethodName.Should().Be(expectedReadableName);
    }
}
