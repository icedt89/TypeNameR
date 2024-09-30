using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace JanHafner.TypeNameR.Experimental.Helper;

/// <summary>
/// This exists because we want to avoid null-checks and every *Dictionary.Empty variant is slower or not available on the necessary target framework.
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed class EmptyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IEnumerator<KeyValuePair<TKey, TValue>>
    where TKey : notnull
{
    public static readonly EmptyDictionary<TKey, TValue> Instance = new();

    private EmptyDictionary()
    {
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => this;

    IEnumerator IEnumerable.GetEnumerator() => this;

    public int Count => 0;

    public bool ContainsKey(TKey key) => false;

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        value = default;

        return false;
    }

    public TValue this[TKey key] => throw new NotSupportedException();

    public IEnumerable<TKey> Keys => Enumerable.Empty<TKey>();

    public IEnumerable<TValue> Values => Enumerable.Empty<TValue>();

    public bool MoveNext() => false;

    public void Reset() { }

    public KeyValuePair<TKey, TValue> Current => default;

    object IEnumerator.Current => default!;

    public void Dispose() { }
}