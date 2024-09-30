using JanHafner.TypeNameR.Experimental.Helper;
using System.Reflection;
using System.Text;
#if NET6_0
using NullabilityInfoContext = Nullability.NullabilityInfoContextEx;
using NullabilityInfo = Nullability.NullabilityInfoEx;
using NullabilityState = Nullability.NullabilityStateEx;
#endif

namespace JanHafner.TypeNameR.Experimental;

/// <inheritdoc />
public partial class TypeNameR
{
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