using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace JanHafner.TypeNameR;

[ExcludeFromCodeCoverage]
internal static class NullabilityHelper
{
    public static bool IsNullableReferenceType(this NullabilityInfo nullabilityInfo) => nullabilityInfo.ReadState == NullabilityState.Nullable
                                                                                        && Nullable.GetUnderlyingType(nullabilityInfo.Type) is null;

    public static bool IsNullableStruct(this Type type, [NotNullWhen(true)] out Type? underlyingType)
    {
        underlyingType = Nullable.GetUnderlyingType(type);

        return underlyingType is not null;
    }

    public static NullabilityInfo GetNullabilityInfo(this ParameterInfo parameterInfo) => new NullabilityInfoContext().Create(parameterInfo);
}
