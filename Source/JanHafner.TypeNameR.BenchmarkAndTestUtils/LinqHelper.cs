namespace JanHafner.TypeNameR.BenchmarkAndTestUtils;

public static class LinqHelper
{
    public static IEnumerable<T> RandomTake<T>(this IReadOnlyList<T> source, int take, TimeSpan? timeOut = null)
    {
        var taken = new HashSet<T>(take);

        using var cancellationTokenSource = new CancellationTokenSource(timeOut ?? TimeSpan.FromSeconds(5));
        while (taken.Count < take && !cancellationTokenSource.IsCancellationRequested)
        {
            var randomIndex = Random.Shared.Next(0, source.Count);

            var type = source[randomIndex];
            if (taken.Add(type))
            {
                yield return type;
            }
        }
    }
}