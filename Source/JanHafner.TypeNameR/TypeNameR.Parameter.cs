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
        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeParameterPrefix))
        {
            ProcessParameterPrefix(stringBuilder, parameterInfo, nameRControlFlags);
        }

        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeDynamic) && parameterInfo.HasDynamicAttribute())
        {
            stringBuilder.AppendDynamic();
        }
        else
        {
            ProcessTypeCore(stringBuilder,
                parameterInfo.ParameterType,
                fullTypeName: false,
                nameRControlFlags.HasFlag(NameRControlFlags.IncludeNullabilityInfo) ? nullabilityInfoContext.Create(parameterInfo) : null,
                masterGenericTypes: null,
                nameRControlFlags);
        }

        if (parameterInfo.Position == Constants.ReturnParameterIndex)
        {
            return;
        }

        stringBuilder.AppendParameterName(parameterInfo.Name!);

        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeParameterDefaultValue))
        {
            ProcessParameterSuffix(stringBuilder, parameterInfo, nameRControlFlags);
        }
    }

    private static void ProcessParameterPrefix(StringBuilder stringBuilder, ParameterInfo parameterInfo, NameRControlFlags nameRControlFlags)
    {
        // They are all mutually exclusive!
        // The "this" keyword is only valid on the first parameter
        if (parameterInfo.Position == Constants.ThisKeywordOnlyValidOnIndex
            && nameRControlFlags.HasFlag(NameRControlFlags.IncludeThisKeyword)
            && parameterInfo.Member.HasExtensionAttribute())
        {
            stringBuilder.AppendThisWithEndingSpace();

            // Support "this in" and "this ref"
            if (!parameterInfo.IsIn && !parameterInfo.ParameterType.IsByRef)
            {
                return;
            }
        }

        // The "in" and "out" keywords are only valid on non-return parameter
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

        // The "params" keyword is only valid on non-return parameter
        if (parameterInfo.Position > Constants.ReturnParameterIndex
            && nameRControlFlags.HasFlag(NameRControlFlags.IncludeParamsKeyword)
            && parameterInfo.HasParamArrayAttribute())
        {
            stringBuilder.AppendParamsWithEndingSpace();
        }
    }

    private static void ProcessParameterSuffix(StringBuilder stringBuilder, ParameterInfo parameterInfo, NameRControlFlags nameRControlFlags)
    {
        if (!parameterInfo.IsOptional || !parameterInfo.HasDefaultValue)
        {
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

        switch (parameterInfo.DefaultValue)
        {
            case Enum value:
                if (nameRControlFlags.HasFlag(NameRControlFlags.PrintEnumParameterDefaultValueWithoutEnumTypeName))
                {
                    stringBuilder.AppendEqualsValue(value.ToString());

                    return;
                }

                stringBuilder.AppendEqualsEnumValue(parameterInfo.ParameterType.Name, value.ToString());

                return;
            case string value:
                stringBuilder.AppendEqualsQuotedValue(value);

                return;
            case bool value:
                stringBuilder.AppendEqualsValue(value);

                return;
            case char value:
                stringBuilder.AppendEqualsValue(value);

                return;
            case sbyte value:
                stringBuilder.AppendEqualsValue(value);

                return;
            case byte value:
                stringBuilder.AppendEqualsValue(value);

                return;
            case short value:
                stringBuilder.AppendEqualsValue(value);

                return;
            case int value:
                stringBuilder.AppendEqualsValue(value);

                return;
            case long value:
                stringBuilder.AppendEqualsValue(value);

                return;
            case float value:
                stringBuilder.AppendEqualsValue(value);

                return;
            case double value:
                stringBuilder.AppendEqualsValue(value);

                return;
            case decimal value:
                stringBuilder.AppendEqualsValue(value);

                return;
            case ushort value:
                stringBuilder.AppendEqualsValue(value);

                return;
            case uint value:
                stringBuilder.AppendEqualsValue(value);

                return;
            case ulong value:
                stringBuilder.AppendEqualsValue(value);

                return;
            default:
                throw new NotSupportedException($"Parameter default value of type '{parameterInfo.DefaultValue.GetType().Name}' is not supported.");
        }
    }
}