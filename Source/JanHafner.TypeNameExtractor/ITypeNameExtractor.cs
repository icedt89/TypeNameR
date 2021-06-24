using System;

namespace JanHafner.TypeNameExtractor
{
    public interface ITypeNameExtractor
    {
        string ExtractReadableName(Type type);
    }
}