namespace JanHafner.TypeNameR.Experimental.Helper;

internal static class TypeHelper
{
    public static ReadOnlySpan<char> RemoveGenericParametersArity(this string typeName)
        => typeName.AsSpan(0, typeName.IndexOf(Constants.GraveAccent));

    public static bool DetermineActualGenerics(this Type type, ref Type[]? masterGenericTypes, out int actualStartGenericParameterIndex, out int actualGenericParametersCount)
    {
        if (!type.IsGenericType)
        {
            actualStartGenericParameterIndex = -1;
            actualGenericParametersCount = -1;

            return false;
        }

        var typeGenericArguments = type.GetGenericArguments();

        // Because GetGenericArguments() returns the generic parameters of the declaring type too,
        // we must determine which generic types are the actual declared ones the current type.
        // An example:
        // Given the type: Outer<T, K>+Inner+Actual<M> (Outer<string, bool>+Inner+Actual<int>)
        // type.GetGenericArguments() will return an array of three types [T, K, M] for Actual<M>
        // and an array of two types [T, K] for Inner.
        // This would produce the following output: Outer<T, K>+Inner<T, K>+Actual<T, K, M>
        // To circumvent this we resolve the generic parameters of the declaring type and skip these.
        // Specialty: Only the most inner (e.g. Actual<int>) holds the real parameters (string, bool, int)
        // In order to correctly lookup these when processing a nested type it is necessary to return these (in masterGenericTypes)
        // The output would be if not processed this way: Outer<T, K>+Inner+Actual<int>
        masterGenericTypes ??= typeGenericArguments;

        actualStartGenericParameterIndex = 0;
        actualGenericParametersCount = typeGenericArguments.Length;

        if (type.DeclaringType is not null && type.DeclaringType.IsGenericType)
        {
            actualStartGenericParameterIndex = type.DeclaringType.GetGenericArguments().Length;
        }

        return actualStartGenericParameterIndex < actualGenericParametersCount;
    }

    [Obsolete("Use method")]
    public static bool IsGenericValueTuple(this Type type) => type.Namespace == "System" && type.Name.StartsWith("ValueTuple`", StringComparison.Ordinal);
}