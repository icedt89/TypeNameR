using JanHafner.TypeNameR.Helper;
using System.Reflection;
using System.Text;

#if NET6_0
using NullabilityInfoContext = Nullability.NullabilityInfoContextEx;
using NullabilityInfo = Nullability.NullabilityInfoEx;
using NullabilityState = Nullability.NullabilityStateEx;
#endif

namespace JanHafner.TypeNameR;

/// <inheritdoc />
public partial class TypeNameR
{
    private static bool DetermineGenericsProcessingInfo(Type type, ref Type[]? masterGenericTypes, out int startGenericParameterIndex, out int genericParametersCount)
    {
        // Ignore type if it is not generic (has no inherited generic types and not defined any directly)
        if (!type.IsGenericType)
        {
            startGenericParameterIndex = 0;
            genericParametersCount = 0;

            return false;
        }

        var typeGenericArguments = type.GetGenericArguments();

        // Get generic arguments if we have them not yet resolved.
        // This parameter serves as the "master"-lookup for nested types, so we resolve it just once for the most inner type and reuse it for the nested types.
        // This is necessary because the "real"-types can not be resolved from the declaring type.
        masterGenericTypes ??= typeGenericArguments;

        startGenericParameterIndex = 0;
        genericParametersCount = typeGenericArguments.Length;

        // If the type is nested and the declaring type is generic, we need to strip out the inherited generic arguments
        if (type.DeclaringType is not null && type.DeclaringType.IsGenericType)
        {
            startGenericParameterIndex = type.DeclaringType.GetGenericArguments().Length;
        }

        return startGenericParameterIndex < genericParametersCount;
    }

    private void ProcessGenerics(StringBuilder stringBuilder, Type[] genericTypes, NullabilityInfo[]? genericTypesNullability, int startGenericParameterIndex, int genericParametersCount, NameRControlFlags nameRControlFlags)
    {
        for (var genericParameterIndex = startGenericParameterIndex; genericParameterIndex < genericParametersCount; genericParameterIndex++)
        {
            NullabilityInfo? genericTypeNullabilityInfo = null;
            if (genericTypesNullability is not null && genericParameterIndex < genericTypesNullability.Length)
            {
                genericTypeNullabilityInfo = genericTypesNullability[genericParameterIndex];
            }

            ProcessTypeCore(stringBuilder, genericTypes[genericParameterIndex], false, genericTypeNullabilityInfo, null, nameRControlFlags);

            if (genericParameterIndex < genericParametersCount - 1)
            {
                stringBuilder.AppendCommaWithEndingSpace();
            }
        }
    }
}