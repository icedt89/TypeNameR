namespace JanHafner.TypeNameR.Benchmark;

public sealed class StackTraceGenerator
{
    public static Task<TResult> CallRecursivGenericMethod<TResult>(ref int? int1) => RecursivGenericMethod<TResult>(ref int1, 1, 10);

    private static Task<TResult> RecursivGenericMethod<TResult>(ref int? int1, int recursionDepth, int stopAt)
    {
        if (recursionDepth == stopAt)
        {
            throw new StackOverflowException("RecursivGenericMethod has thrown at depth '{stopAt}'");
        }

        return RecursivGenericMethod<TResult>(ref int1, recursionDepth + 1, stopAt);
    }
}
