using JanHafner.TypeNameR.Helper;
using System.Reflection;
using System.Runtime.InteropServices;
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
    /// <inheritdoc />
    public string GenerateDisplay(ParameterInfo parameterInfo, NameRControlFlags nameRControlFlags)
    {
        ArgumentNullException.ThrowIfNull(parameterInfo);

        var stringBuilder = new StringBuilder();
        var nullabilityInfoContext = new NullabilityInfoContext();

        ProcessParameter(stringBuilder, nullabilityInfoContext, parameterInfo, nameRControlFlags);

        return stringBuilder.ToString();
    }

    private void ProcessParameters(StringBuilder stringBuilder, NullabilityInfoContext nullabilityInfoContext, ParameterInfo[] parameters, NameRControlFlags nameRControlFlags)
    {
        for (var parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
        {
            ProcessParameter(stringBuilder, nullabilityInfoContext, parameters[parameterIndex], nameRControlFlags);

            if (parameterIndex < parameters.Length - 1)
            {
                stringBuilder.AppendCommaWithEndingSpace();
            }
        }
    }

    private void ProcessParameter(StringBuilder stringBuilder, NullabilityInfoContext nullabilityInfoContext, ParameterInfo parameterInfo, NameRControlFlags nameRControlFlags)
    {
        if (nameRControlFlags.IsSet(NameRControlFlags.IncludeParameterPrefix))
        {
            ProcessParameterPrefix(stringBuilder, parameterInfo, nameRControlFlags);
        }

        if (nameRControlFlags.IsSet(NameRControlFlags.IncludeDynamic) && parameterInfo.HasDynamicAttribute())
        {
            stringBuilder.AppendDynamic();
        }
        else
        {
            ProcessTypeCore(stringBuilder,
                parameterInfo.ParameterType,
                fullTypeName: false,
                nameRControlFlags.IsSet(NameRControlFlags.IncludeNullabilityInfo) ? nullabilityInfoContext.Create(parameterInfo) : null,
                masterGenericTypes: null,
                nameRControlFlags);
        }

        if (parameterInfo.Position == Constants.ReturnParameterIndex)
        {
            return;
        }

        stringBuilder.AppendParameterName(parameterInfo.Name!);

        if (nameRControlFlags.IsSet(NameRControlFlags.IncludeParameterDefaultValue))
        {
            ProcessParameterSuffix(stringBuilder, parameterInfo);
        }
    }

    private static void ProcessParameterPrefix(StringBuilder stringBuilder, ParameterInfo parameterInfo, NameRControlFlags nameRControlFlags)
    {
        // They are all mutually exclusive!
        // The "this" keyword is only valid on the first parameter
        if (parameterInfo.Position == Constants.ThisKeywordOnlyValidOnIndex
            && nameRControlFlags.IsSet(NameRControlFlags.IncludeThisKeyword)
            && parameterInfo.Member.HasExtensionAttribute())
        {
            stringBuilder.AppendThisWithEndingSpace();

            // Support "this in" and "this ref"
            if (!parameterInfo.IsIn && !parameterInfo.ParameterType.IsByRef)
            {
                return;
            }
        }

        // The "in" and "out" keywords are only valid on non return parameter
        if (parameterInfo.Position > Constants.ReturnParameterIndex)
        {
            if (parameterInfo.IsOut)
            {
                stringBuilder.AppendOutWithEndingSpace();

                return;
            }

            if (parameterInfo.IsIn)
            {
                stringBuilder.AppendInWithEndingSpace();

                return;
            }
        }

        // The "in" and "out" keywords are also considered by-ref, in order to get correct results, "in" and "out" must be evaluated first
        // The "ref" keyword is also valid on return parameters
        if (parameterInfo.ParameterType.IsByRef)
        {
            stringBuilder.AppendRefWithEndingSpace();

            return;
        }

        // The "params" keyword is only valid on non return parameter
        if (parameterInfo.Position > Constants.ReturnParameterIndex
            && nameRControlFlags.IsSet(NameRControlFlags.IncludeParamsKeyword)
            && parameterInfo.HasParamArrayAttribute())
        {
            stringBuilder.AppendParamsWithEndingSpace();
        }
    }

    private static void ProcessParameterSuffix(StringBuilder stringBuilder, ParameterInfo parameterInfo)
    {
        if (!parameterInfo.IsOptional || !parameterInfo.HasDefaultValue)
        {
            return;
        }

        stringBuilder.AppendSpace();

        if (parameterInfo.DefaultValue is string @string)
        {
            stringBuilder.AppendQuotedParameterValue(@string);

            return;
        }

        if (parameterInfo.DefaultValue is null)
        {
            if (parameterInfo.ParameterType.IsValueType && Nullable.GetUnderlyingType(parameterInfo.ParameterType) is null)
            {
                stringBuilder.AppendEqualsDefaultWithLeadingSpace();
            }
            else
            {
                stringBuilder.AppendEqualsNullWithLeadingSpace();
            }

            return;
        }

        stringBuilder.AppendEqualsValue(parameterInfo.DefaultValue);
    }
}