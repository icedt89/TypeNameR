using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace JanHafner.TypeNameR.BenchmarkAndTestUtils;

public static class StackTraceGenerator
{
    [DoesNotReturn]
    public static void RecursiveCall(int maxRecursionDepth) => RecursiveCallCore(1, maxRecursionDepth);

    [DoesNotReturn]
    public static void RecursiveCallCore(int current, int maxRecursionDepth)
    {
        if (current == maxRecursionDepth)
        {
            throw new StackOverflowException();
        }

        current++;

        if (current % 3 != 0)
        {
            RecursiveCallCore(current, maxRecursionDepth);
        }

        RecursiveSubCall(current, maxRecursionDepth);
    }

    [DoesNotReturn]
    public static void RecursiveSubCall(int current, int maxRecursionDepth) => RecursiveCallCore(current, maxRecursionDepth);

    [DoesNotReturn]
    public static Task<TResult> CallRecursiveGenericMethodAsync<TResult>(int? recursionDepth = 1, int stopAt = 10)
    {
        if (recursionDepth == stopAt)
        {
            throw new StackOverflowException($"RecursiveGenericMethod has thrown at depth '{stopAt}'");
        }

        return CallRecursiveGenericMethodAsync<TResult>(recursionDepth + 1, stopAt);
    }

    [DoesNotReturn]
    [AsyncStateMachine(typeof(StackTraceGenerator))]
    public static Task<TResult> CallRecursiveGenericMethodWithAsyncStateMachineAttributeAsync<TResult>(int? recursionDepth = 1, int stopAt = 10)
    {
        if (recursionDepth == stopAt)
        {
            throw new StackOverflowException($"RecursiveGenericMethod has thrown at depth '{stopAt}'");
        }

        return CallRecursiveGenericMethodAsync<TResult>(recursionDepth + 1, stopAt);
    }
}