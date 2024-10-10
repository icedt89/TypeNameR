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
    private void ProcessGenerics(StringBuilder stringBuilder, ReadOnlySpan<Type> genericTypes, NullabilityInfo[]? genericTypesNullability, NameRControlFlags nameRControlFlags)
    {
        for (var genericParameterIndex = 0; genericParameterIndex < genericTypes.Length; genericParameterIndex++)
        {
            NullabilityInfo? genericTypeNullabilityInfo = null;
            if (genericTypesNullability is not null && genericParameterIndex < genericTypesNullability.Length)
            {
                genericTypeNullabilityInfo = genericTypesNullability[genericParameterIndex];
            }

            ProcessTypeCore(stringBuilder, genericTypes[genericParameterIndex], false, genericTypeNullabilityInfo, null, nameRControlFlags);

            if (genericParameterIndex < genericTypes.Length - 1)
            {
                stringBuilder.AppendCommaWithEndingSpace();
            }
        }
    }
}