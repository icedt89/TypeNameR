using System;

namespace JanHafner.TypeNameExtractor
{
    public static class TypeNameExtractorExtensions
    {
        public static string ExtractReadableName<T>(this ITypeNameExtractor typeNameExtractor)
        {
            if (typeNameExtractor is null)
            {
                throw new ArgumentNullException(nameof(typeNameExtractor));
            }

            return typeNameExtractor.ExtractReadableName(typeof(T));
        }
    }
}
