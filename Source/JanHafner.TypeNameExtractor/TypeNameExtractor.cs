using System;
using System.Collections.Generic;
using System.Text;

namespace JanHafner.TypeNameExtractor;

public sealed class TypeNameExtractor : ITypeNameExtractor
{
    private readonly TypeNameExtractorOptions typeNameExtractorOptions;

    public TypeNameExtractor(TypeNameExtractorOptions? typeNameExtractorOptions = null)
    {
        this.typeNameExtractorOptions = typeNameExtractorOptions ?? new TypeNameExtractorOptions();
    }

    public string ExtractReadableName(Type type)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        var typeNameBuilder = new StringBuilder();
        var visitedTypes = new Dictionary<Type, StringBuilder>();

        this.ExtractReadableNameCore(type, typeNameBuilder, false, visitedTypes);

        return typeNameBuilder.ToString();
    }

    private void ExtractReadableNameCore(Type type, StringBuilder typeNameBuilder, bool isInNestedContext, IDictionary<Type, StringBuilder> visitedTypes)
    {
        if (visitedTypes.TryGetValue(type, out var name))
        {
            typeNameBuilder.Append(name);

            return;
        }

        if (type.IsArray)
        {
            var elementType = type.GetElementType();

            var arrayTypeNameBuilder = new StringBuilder();

            this.ExtractReadableNameCore(elementType!, arrayTypeNameBuilder, false, visitedTypes);

            string? arrayRankPart = null;
            if (type.IsVariableBoundArray)
            {
                var arrayRank = type.GetArrayRank();

                arrayRankPart = new string(',', arrayRank - 1);
            }

            arrayTypeNameBuilder.AppendFormat("{0}{1}{2}", this.typeNameExtractorOptions.ArrayOpeningBracket, arrayRankPart, this.typeNameExtractorOptions.ArrayClosingBracket);

            typeNameBuilder.Append(arrayTypeNameBuilder);

            visitedTypes[type] = arrayTypeNameBuilder;

            return;
        }

        var nullableType = Nullable.GetUnderlyingType(type);
        if (nullableType != null && this.typeNameExtractorOptions.UseNullableTypeShortForm)
        {
            var nullableTypeNameBuilder = new StringBuilder();

            this.ExtractReadableNameCore(nullableType, nullableTypeNameBuilder, false, visitedTypes);
            nullableTypeNameBuilder.Append(this.typeNameExtractorOptions.NullableTypeMarker);

            typeNameBuilder.Append(nullableTypeNameBuilder);

            visitedTypes[type] = nullableTypeNameBuilder;

            return;
        }

        var typeName = isInNestedContext && type.DeclaringType is null && this.typeNameExtractorOptions.FullQualifyOuterMostTypeNameOnNestedTypes ? type.FullName! : type.Name;
        typeName = TypeHelper.RemoveGenericParametersCount(typeName, this.typeNameExtractorOptions.GenericParameterCountDelimiter);
        if (this.typeNameExtractorOptions.PredefinedTypeNames.TryGetValue(type, out var primitiveTypeName) && !this.typeNameExtractorOptions.UseClrTypeNameForPrimitiveTypes)
        {
            typeName = primitiveTypeName;
        }

        var innerTypeNameBuilder = new StringBuilder(typeName);

        var genericArguments = type.GetGenericArguments();
        if(genericArguments.Length > 0)
        {
            innerTypeNameBuilder.Append(this.typeNameExtractorOptions.GenericTypeOpeningBracket);

            for (int i = 0; i < genericArguments.Length; i++)
            {
                var genericArgument = genericArguments[i];

                var isNotLastParameter = i < genericArguments.Length - 1;
                var isNotFirstParameter = i > 0;

                if (!genericArgument.IsGenericParameter || this.typeNameExtractorOptions.OutputTypeVariableNames)
                {
                    if (isNotFirstParameter)
                    {
                        innerTypeNameBuilder.Append(' ');
                    }
                    
                    var genericArgumentTypeNameBuilder = new StringBuilder();

                    this.ExtractReadableNameCore(genericArgument, genericArgumentTypeNameBuilder, false, visitedTypes);

                    visitedTypes[genericArgument] = genericArgumentTypeNameBuilder;

                    innerTypeNameBuilder.Append(genericArgumentTypeNameBuilder);
                }

                if(isNotLastParameter)
                {
                    innerTypeNameBuilder.Append(this.typeNameExtractorOptions.GenericParameterDelimiter);
                }
            }

            innerTypeNameBuilder.Append(this.typeNameExtractorOptions.GenericTypeClosingBracket);
        }

        if (type.IsNested && !type.IsGenericParameter && type.DeclaringType != null)
        {
            var nestedTypeNameBuilder = new StringBuilder();

            this.ExtractReadableNameCore(type.DeclaringType, nestedTypeNameBuilder, true, visitedTypes);

            typeNameBuilder.AppendFormat("{0}{1}{2}", nestedTypeNameBuilder, this.typeNameExtractorOptions.NestedTypeDelimiter, innerTypeNameBuilder);
        }
        else
        {
            typeNameBuilder.Append(innerTypeNameBuilder);
        }

        visitedTypes[type] = innerTypeNameBuilder;
    }      
}
