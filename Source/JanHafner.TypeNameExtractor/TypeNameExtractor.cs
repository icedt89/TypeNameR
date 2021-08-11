using System;
using System.Text;

namespace JanHafner.TypeNameExtractor
{
    public sealed class TypeNameExtractor : ITypeNameExtractor
    {
        private readonly bool outputTypeVariableNames;

        public const char GENERIC_TYPE_OPENING_BRACKET = '<';

        public const char GENERIC_TYPE_CLOSING_BRACKET = '>';

        public const char GENERIC_TYPE_PARAMETER_COUNT_DELIMITER = '`';

        public const char GENERIC_TYPE_PARAMETER_DELIMITER = ',';

        public TypeNameExtractor(bool outputTypeVariableNames = false)
        {
            this.outputTypeVariableNames = outputTypeVariableNames;
        }

        public string ExtractReadableName(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var typeNameBuilder = new StringBuilder();

            this.ExtractReadableNameCore(type, typeNameBuilder, 0);

            return typeNameBuilder.ToString();
        }

        private void ExtractReadableNameCore(Type type, StringBuilder typeNameBuilder, int genericParameterIndex)
        {
            if (type.IsGenericParameter && !this.outputTypeVariableNames)
            {
                // Skip parameters of open generic types to prevent output of their names e.g. 'T' in TestClass<T>
                return;
            }

            var typeNameWithoutGenericParameterCount = RemoveGenericParametersCount(type.Name);
            if (genericParameterIndex > 0)
            {
                typeNameBuilder.Append(' ');
            }

            typeNameBuilder.Append(typeNameWithoutGenericParameterCount);

            if (!type.IsGenericType)
            {
                return;
            }

            typeNameBuilder.Append(TypeNameExtractor.GENERIC_TYPE_OPENING_BRACKET);

            var genericParameters = type.GetGenericArguments();
            for (var i = 0; i < genericParameters.Length; i++)
            {
                var genericParameter = genericParameters[i];

                this.ExtractReadableNameCore(genericParameter, typeNameBuilder, i);

                if (i < genericParameters.Length - 1)
                {
                    typeNameBuilder.Append(TypeNameExtractor.GENERIC_TYPE_PARAMETER_DELIMITER);
                }
            }

            typeNameBuilder.Append(TypeNameExtractor.GENERIC_TYPE_CLOSING_BRACKET);
        }

        /// <summary>
        /// Removes the "generic arguments count"-delimiter from the type name.
        /// E.g. TestClass`1 becomes TestClass, or TestClass`22 becomes TestClass.
        /// </summary>
        public static string RemoveGenericParametersCount(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException($"'{nameof(typeName)}' cannot be null or whitespace.", nameof(typeName));
            }

            var lastIndexOfGenericParamterDelimiter = typeName.LastIndexOf(TypeNameExtractor.GENERIC_TYPE_PARAMETER_COUNT_DELIMITER);
            if (lastIndexOfGenericParamterDelimiter > 0)
            {
                return typeName.Substring(0, lastIndexOfGenericParamterDelimiter);
            }

            return typeName;
        }
    }
}
