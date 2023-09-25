namespace JanHafner.TypeNameR.Helper;

internal static class ArrayHelper
{
    public static ReadOnlySpan<string> ReducePrefixes(this string[] items)
    {
        if (items.Length == 0)
        {
            return ReadOnlySpan<string>.Empty;
        }
        
        var currentIndex = 1;
        var current = items[0];
        for (var i = 1; i < items.Length; i++)
        {
            if (items[i].StartsWith(current, StringComparison.Ordinal))
            {
                continue;
            }

            current = items[currentIndex++] = items[i];
        }

        return items.AsSpan()[..currentIndex];
    }
}