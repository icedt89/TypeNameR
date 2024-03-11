using System.Reflection;

namespace JanHafner.TypeNameR.BenchmarkAndTestUtils;

public static class ReflectionHelper
{
    public static MethodInfo GetMethodOrThrow(this Type type, string methodName)
        => type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static) ?? throw new InvalidOperationException("Method");

    public static ParameterInfo GetParameter(this Type type, string methodName, int parameterIndex)
        => type.GetMethodOrThrow(methodName).GetParameters()[parameterIndex];

    public static ConstructorInfo GetPublicParameterlessConstructor(this Type type)
        => type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, Type.EmptyTypes) ?? throw new InvalidOperationException("Constructor");

    public static ConstructorInfo GetPrivateParameterlessDistinguishConstructor(this Type type)
        => type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, [typeof(bool)]) ?? throw new InvalidOperationException("Constructor");

    public static ConstructorInfo GetStaticParameterlessConstructor(this Type type)
        => type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Static, Type.EmptyTypes) ?? throw new InvalidOperationException("Constructor");
}