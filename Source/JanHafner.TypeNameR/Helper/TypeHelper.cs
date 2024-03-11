using System.Runtime.CompilerServices;

namespace JanHafner.TypeNameR.Helper;

internal static class TypeHelper
{
    public static ReadOnlySpan<char> RemoveGenericParametersCount(this ReadOnlySpan<char> typeName)
    {
        var indexOfGenericParameterDelimiter = typeName.IndexOf(Constants.GraveAccent);
        if (indexOfGenericParameterDelimiter > 0)
        {
            return typeName[..indexOfGenericParameterDelimiter];
        }

        return typeName;
    }

    [Obsolete("Use method")]
    public static bool IsGenericValueTuple(this Type type) => type.IsValueType && type.IsGenericType && type.IsAssignableTo(typeof(ITuple));
}