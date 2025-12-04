using System.Reflection;

namespace JanHafner.TypeNameR.BenchmarkAndTestUtils;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.ReturnValue | AttributeTargets.Constructor | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public sealed class ExpectsAttribute : Attribute
{
    public ExpectsAttribute(string expected)
    {
        Expected = expected;
    }

    public string Expected { get; }

    public static string? GetExpectation(MemberInfo customAttributeProvider)
    {
        var expectsAttribute = customAttributeProvider.GetCustomAttribute<ExpectsAttribute>();

        return expectsAttribute?.Expected;
    }

    public static string? GetExpectation(ParameterInfo customAttributeProvider)
    {
        var expectsAttribute = customAttributeProvider.GetCustomAttribute<ExpectsAttribute>();

        return expectsAttribute?.Expected;
    }
}