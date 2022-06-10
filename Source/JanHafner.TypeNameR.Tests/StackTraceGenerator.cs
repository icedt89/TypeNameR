namespace JanHafner.TypeNameR.Tests;

public sealed class StackTraceGenerator
{
    public unsafe static Task CallAsyncTaskMethod()
    {
        string string1 = null;

        return AsyncTaskMethod(ref string1, null, null);
    }

    private static unsafe Task<int?> AsyncTaskMethod(ref string? string1, int?*[] int1, int? int2 = null)
    {
        throw new InvalidOperationException("AsyncTaskMethod has thrown");
    }

    public static async ValueTask CallAsyncValueTaskMethod()
    {
        string string1 = null;

        await AsyncValueTaskMethod(ref string1, null, null);
    }

    private static ValueTask<int?> AsyncValueTaskMethod(ref string? string1, int?[] int1, int? int2 = null)
    {
        throw new InvalidOperationException("AsyncValueTaskMethod has thrown");
    }

    public static IEnumerable<bool> CallIteratorMethod()
    {
        var iterables = IteratorMethod();
        foreach (var _ in iterables)
        {
            yield return _;
        }
    }

    private static IEnumerable<bool> IteratorMethod()
    {
        yield return true;

        throw new InvalidOperationException("IteratorMethod has thrown");
    }

    public static Task<TResult> CallRecursivGenericMethod<TResult>(ref int? int1)
    {
        return RecursivGenericMethod<TResult>(ref int1, 1, 10);
    }

    private static Task<TResult> RecursivGenericMethod<TResult>(ref int? int1, int recursionDepth, int stopAt)
    {
        if (recursionDepth == stopAt)
        {
            throw new StackOverflowException("RecursivGenericMethod has thrown at depth '{stopAt}'");
        }

        return RecursivGenericMethod<TResult>(ref int1, recursionDepth + 1, stopAt);
    }
}
