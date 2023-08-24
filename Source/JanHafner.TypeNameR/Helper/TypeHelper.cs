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
}
