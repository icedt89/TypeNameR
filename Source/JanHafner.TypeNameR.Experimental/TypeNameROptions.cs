using JanHafner.TypeNameR.Experimental.Helper;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace JanHafner.TypeNameR.Experimental;

/// <summary>
/// The options class to configure a <see cref="TypeNameR"/> instance.
/// </summary>
public sealed class TypeNameROptions
{
    public static TypeNameROptions Default = new(Helper.PredefinedTypeNames.Default);

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="predefinedTypeNames">
    /// If <see langword="null"/>, a default list with well known type names, provided by <see cref="Helper.PredefinedTypeNames"/>.<see cref="Helper.PredefinedTypeNames.Default"/>, will be populated. The supplied dictionary will be copied and frozen.
    /// </param>
    public TypeNameROptions(
        IReadOnlyDictionary<Type, string>? predefinedTypeNames = null)
    {
        PredefinedTypeNames = InitializePredefinedTypeNames(predefinedTypeNames);
    }

    /// <summary>
    /// Defines the names used for predefined types.
    /// </summary>
    public IReadOnlyDictionary<Type, string> PredefinedTypeNames { get; }

    private static IReadOnlyDictionary<Type, string> InitializePredefinedTypeNames(IReadOnlyDictionary<Type, string>? predefinedTypeNames)
    {
        if (predefinedTypeNames is null || predefinedTypeNames.Count == 0)
        {
            return EmptyDictionary<Type, string>.Instance;
        }

#if NET8_0_OR_GREATER
        if (predefinedTypeNames is FrozenDictionary<Type, string>)
        {
            return predefinedTypeNames;
        }
#endif

        return predefinedTypeNames.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
#if NET8_0_OR_GREATER
            .ToFrozenDictionary()
#endif
            ;
    }
}