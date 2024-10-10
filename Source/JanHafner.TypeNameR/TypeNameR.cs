using JanHafner.TypeNameR.Helper;
using JanHafner.TypeNameR.StackTrace;
using System.Diagnostics;
using System.Reflection;
using System.Text;

#if NET6_0
using NullabilityInfoContext = Nullability.NullabilityInfoContextEx;
using NullabilityInfo = Nullability.NullabilityInfoEx;
using NullabilityState = Nullability.NullabilityStateEx;
#endif

namespace JanHafner.TypeNameR;

/// <inheritdoc />
public sealed partial class TypeNameR : ITypeNameR
{
    private readonly IStackFrameMetadataProvider? stackFrameMetadataProvider;

    private readonly TypeNameROptions typeNameROptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameR"/> class with the supplied <see cref="TypeNameROptions"/>.
    /// </summary>
    /// <param name="stackFrameMetadataProvider">Implementation that is used to retrieve <see cref="StackFrameMetadata"/> for <see cref="StackFrame"/>.</param>
    /// <param name="typeNameROptions">If set to <see langword="null"/>, a new instance initialized with the default values will be used internally.</param>
    public TypeNameR(IStackFrameMetadataProvider? stackFrameMetadataProvider = null,
                     TypeNameROptions? typeNameROptions = null)
    {
        this.stackFrameMetadataProvider = stackFrameMetadataProvider;
        this.typeNameROptions = typeNameROptions ?? TypeNameROptions.Default;
    }

    /// <inheritdoc />
    public string GenerateDisplay(Type type, bool fullTypeName, NameRControlFlags nameRControlFlags)
    {
        ArgumentNullException.ThrowIfNull(type);

        var stringBuilder = new StringBuilder();

        ProcessTypeCore(stringBuilder, type, fullTypeName, null, null, nameRControlFlags);

        return stringBuilder.ToString();
    }

    /// <inheritdoc />
    public string GenerateDisplay(NullabilityInfo nullabilityInfo, bool fullTypeName, NameRControlFlags nameRControlFlags)
    {
        ArgumentNullException.ThrowIfNull(nullabilityInfo);

        var stringBuilder = new StringBuilder();

        ProcessTypeCore(stringBuilder, nullabilityInfo.Type, fullTypeName, nullabilityInfo, null, nameRControlFlags);

        return stringBuilder.ToString();
    }

    private void ProcessTypeCore(StringBuilder stringBuilder, Type type, bool fullTypeName, NullabilityInfo? nullabilityInfo, Type[]? masterGenericTypes,
        NameRControlFlags nameRControlFlags)
    {
        var skipTypeAndGenericsAndNullable = false;
        if (typeNameROptions.PredefinedTypeNames.TryGetValue(type, out var predefinedTypeName))
        {
            stringBuilder.Append(predefinedTypeName.AsSpan());

            skipTypeAndGenericsAndNullable = true;
        }
        else
        {
            var elementType = type.GetElementType();
            if (elementType is not null)
            {
                var isArray = type.IsArray;

                ProcessTypeCore(stringBuilder, elementType, fullTypeName, isArray ? nullabilityInfo?.ElementType : nullabilityInfo, masterGenericTypes: null, nameRControlFlags: nameRControlFlags);

                if (isArray)
                {
                    stringBuilder.AppendArrayRank(type.GetArrayRank());
                }
                else if (type.IsPointer)
                {
                    stringBuilder.AppendPointerMarker();
                }

                skipTypeAndGenericsAndNullable = true;
            }
        }

        var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
        if (!skipTypeAndGenericsAndNullable && nullableUnderlyingType is not null)
        {
            ProcessTypeCore(stringBuilder, nullableUnderlyingType, fullTypeName, null, null, nameRControlFlags);

            stringBuilder.AppendNullableMarker();

            return;
        }

        if (!skipTypeAndGenericsAndNullable)
        {
            var isGenericValueTuple = type.IsGenericValueTuple();
            var actualGenericTypes = ReadOnlySpan<Type>.Empty;
            if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeGenericParameters) || isGenericValueTuple)
            {
                actualGenericTypes = type.DetermineActualGenerics(ref masterGenericTypes);
            }

            // Nested
            if (type.DeclaringType is not null && !type.IsGenericParameter)
            {
                ProcessTypeCore(stringBuilder, type.DeclaringType, true, null, masterGenericTypes, nameRControlFlags);

                stringBuilder.AppendPlus();
            }

            // Full type name
            if (fullTypeName && type.DeclaringType is null && type.Namespace is not null)
            {
                stringBuilder.AppendNamespace(type.Namespace);
            }

            if (!actualGenericTypes.IsEmpty)
            {
                if (!isGenericValueTuple)
                {
                    stringBuilder.Append(type.Name.AsSpan(0, type.Name.IndexOf(Constants.GraveAccent)));

                    stringBuilder.AppendLessThanSign();

                    ProcessGenerics(stringBuilder, actualGenericTypes, nullabilityInfo?.GenericTypeArguments, nameRControlFlags);

                    stringBuilder.AppendGreaterThanSign();
                }
                else
                {
                    stringBuilder.AppendLeftParenthesis();

                    ProcessGenerics(stringBuilder, actualGenericTypes, nullabilityInfo?.GenericTypeArguments, nameRControlFlags);

                    stringBuilder.AppendRightParenthesis();
                }
            }
            else
            {
                stringBuilder.Append(type.Name.AsSpan());
            }
        }

        if (nullableUnderlyingType is not null || nullabilityInfo?.ReadState != NullabilityState.Nullable)
        {
            return;
        }

        var compareType = nullabilityInfo.Type;
        if (nullabilityInfo.Type.IsByRef || nullabilityInfo.Type.IsPointer)
        {
            compareType = nullabilityInfo.Type.GetElementType();
        }

        if (type == compareType)
        {
            stringBuilder.AppendNullableMarker();
        }
    }
}