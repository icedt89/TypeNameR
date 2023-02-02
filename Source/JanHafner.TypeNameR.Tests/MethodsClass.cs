namespace JanHafner.TypeNameR.Tests;

public sealed class MethodsClass
{
    static MethodsClass()
    {
        throw new NotImplementedException();
    }

    public MethodsClass()
    {
        throw new NotImplementedException();
    }

    public static void StaticMethod() => throw new NotImplementedException();

    public string NonNullableReturnType() => throw new NotImplementedException();

    public string? NullableReturnType() => throw new NotImplementedException();

    public void VoidMethod() => throw new NotImplementedException();

    public void NonNullableParameter(string string1) => throw new NotImplementedException();

    public void NonNullableParameterWithExactDefaultValue(string string1 = "string1") => throw new NotImplementedException();

    public void NullableParameterWithExactDefaultValue(string? string1 = "string1") => throw new NotImplementedException();

    public void NonNullableIntParameterWithExactDefaultValue(int int1 = 1) => throw new NotImplementedException();

    public void NullableIntParameterWithExactDefaultValue(int? int1 = 1) => throw new NotImplementedException();

    public void OutParameter(out string string1) => throw new NotImplementedException();

    public void InParameter(in string string1) => throw new NotImplementedException();

    public void RefParameter(ref string string1) => throw new NotImplementedException();

    public void NullableParameterWithDefaultValue(object object1 = default) => throw new NotImplementedException();

    public void NonNullableParameterWithDefaultValue(CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public Task TaskMethod() => throw new NotImplementedException();

    public static Task<string?> StaticAsyncComplexMethod1(ref string? string1, out string? string2, in string string3 = "string3", 
        CancellationToken cancellationToken = default) 
        => throw new NotImplementedException();

    public static void NullableReferenceTypeArray(string?[]? string1) => throw new NotImplementedException();

    public static unsafe Task<int?> UnsafeStaticArrayPointerParameter(int?****[] int1 = null) => throw new NotImplementedException();

    public static Task<TResult> GenericRefNullable<TResult>(ref int? int1)
        where TResult : notnull =>
        throw new NotImplementedException();

    public static Task<TResult> GenericRefNonNullable<TResult>(ref int int1)
        where TResult : notnull =>
        throw new NotImplementedException();

    public static Task<TResult> GenericNullable<TResult>(int? int1)
        where TResult : notnull =>
        throw new NotImplementedException();

    public static Task RefNullable(ref int? int1) => throw new NotImplementedException();

    public static Task RefNonNullable(ref int int1) => throw new NotImplementedException();

    public static Task Nullable(int? int1) => throw new NotImplementedException();
}