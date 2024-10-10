using System.Runtime.CompilerServices;

namespace JanHafner.TypeNameR.Helper;

internal static class TypeHelper
{
    public static ReadOnlySpan<Type> DetermineActualGenerics(this Type type, scoped ref Type[]? masterGenericTypes)
    {
        if (!type.IsGenericType)
        {
            return ReadOnlySpan<Type>.Empty;
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
        // In order to correctly look up these when processing a nested type it is necessary to return these (in masterGenericTypes)
        // The output would be if not processed this way: Outer<T, K>+Inner+Actual<int>
        masterGenericTypes ??= typeGenericArguments;

        var actualStartGenericParameterIndex = 0;
        var actualGenericParametersCount = typeGenericArguments.Length;

        if (type.DeclaringType is not null && type.DeclaringType.IsGenericType)
        {
            actualStartGenericParameterIndex = type.DeclaringType.GetGenericArguments().Length;
        }

        return actualStartGenericParameterIndex < actualGenericParametersCount
            ? new ReadOnlySpan<Type>(masterGenericTypes, actualStartGenericParameterIndex, actualGenericParametersCount - actualStartGenericParameterIndex)
            : ReadOnlySpan<Type>.Empty;
    }

    public static bool IsGenericValueTuple(this Type type)
    {
#if NET8_0_OR_GREATER
        return type.IsGenericType && type.IsAssignableTo(typeof(ITuple)) && type.IsValueType;
#else
        return string.Equals(type.Namespace, Constants.SystemNamespaceName, StringComparison.Ordinal) && type.Name.StartsWith(Constants.GenericValueTupleName, StringComparison.Ordinal);
#endif
    }
}