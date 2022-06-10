using FluentAssertions;
using System.Diagnostics;
using Xunit;

namespace JanHafner.TypeNameR.Tests.TypeNameR;

public sealed class ExtractReadableStackTrace
{
    [Fact]
    public async Task ExtractStackTrace1()
    {
        // Arrange
        var expectedReadableStackTrace = "   at private static Task<int?> JanHafner.TypeNameR.Tests.StackTraceGenerator.AsyncTaskMethod(ref string? string1, int?*[] int1, int? int2 = default)"
                               + "\r\n   at public static Task JanHafner.TypeNameR.Tests.StackTraceGenerator.CallAsyncTaskMethod()"
                               + "\r\n   at public async Task JanHafner.TypeNameR.Tests.TypeNameR.ExtractReadableStackTrace.ExtractStackTrace1()";

        var typeNameR = new JanHafner.TypeNameR.TypeNameR();

        try
        {
            // Act
            await StackTraceGenerator.CallAsyncTaskMethod();
        }
        catch (Exception exception)
        {
            // Assert
            var stackTrace = new System.Diagnostics.StackTrace(exception, true);

            var benStackTrace = exception.ToStringDemystified();

            var readableStackTrace = typeNameR.ExtractReadable(stackTrace, NameRControlFlags.All);

            readableStackTrace.Should().Be(expectedReadableStackTrace);
        }
    }

    [Fact]
    public async Task ExtractStackTrace2()
    {
        // Arrange
        var expectedReadableStackTrace = "   at private static ValueTask<int?> JanHafner.TypeNameR.Tests.StackTraceGenerator.AsyncValueTaskMethod(ref string? string1, int?[] int1, int? int2 = default)"
                               + "\r\n   at public static async ValueTask JanHafner.TypeNameR.Tests.StackTraceGenerator.CallAsyncValueTaskMethod()"
                               + "\r\n   at public async Task JanHafner.TypeNameR.Tests.TypeNameR.ExtractReadableStackTrace.ExtractStackTrace2()";

        var typeNameR = new JanHafner.TypeNameR.TypeNameR();

        try
        {
            // Act
            await StackTraceGenerator.CallAsyncValueTaskMethod();
        }
        catch (Exception exception)
        {
            // Assert
            var stackTrace = new System.Diagnostics.StackTrace(exception, true);

            var benStackTrace = exception.ToStringDemystified();

            var readableStackTrace = typeNameR.ExtractReadable(stackTrace, NameRControlFlags.All);

            readableStackTrace.Should().Be(expectedReadableStackTrace);
        }
    }

    [Fact]
    public void ExtractStackTrace3()
    {
        // Arrange
        var expectedReadableStackTrace = "   at private static IEnumerable<bool> JanHafner.TypeNameR.Tests.StackTraceGenerator.IteratorMethod()+MoveNext()"
                               + "\r\n   at public static IEnumerable<bool> JanHafner.TypeNameR.Tests.StackTraceGenerator.CallIteratorMethod()+MoveNext()"
                               + "\r\n   at public Task JanHafner.TypeNameR.Tests.TypeNameR.ExtractReadableStackTrace.ExtractStackTrace3()";

        var typeNameR = new JanHafner.TypeNameR.TypeNameR();

        try
        {
            // Act
            StackTraceGenerator.CallIteratorMethod().ToList();
        }
        catch (Exception exception)
        {
            // Assert
            var stackTrace = new System.Diagnostics.StackTrace(exception, true);

            var benStackTrace = exception.ToStringDemystified();

            var readableStackTrace = typeNameR.ExtractReadable(stackTrace, NameRControlFlags.All);

            readableStackTrace.Should().Be(expectedReadableStackTrace);
        }
    }

    [Fact]
    public async Task ExtractStackTraceFromRecursiveCall()
    {
        // Arrange
        var expectedReadableStackTrace = "   at private static Task<TResult> JanHafner.TypeNameR.Tests.StackTraceGenerator.RecursivGenericMethod<TResult>(ref int? int1, int recursionDepth, int stopAt) x 10"
                               + "\r\n   at public static Task<TResult> JanHafner.TypeNameR.Tests.StackTraceGenerator.CallRecursivGenericMethod<TResult>(ref int? int1)"
                               + "\r\n   at public async Task JanHafner.TypeNameR.Tests.TypeNameR.ExtractReadableStackTrace.ExtractStackTraceFromRecursiveCall()";

        var typeNameR = new JanHafner.TypeNameR.TypeNameR();

        try
        {
            // Act
            int? int1 = 0;
            await StackTraceGenerator.CallRecursivGenericMethod<int>(ref int1);
        }
        catch (Exception exception)
        {
            // Assert
            var stackTrace = new System.Diagnostics.StackTrace(exception, true);

            var benStackTrace = exception.ToStringDemystified();

            var readableStackTrace = typeNameR.ExtractReadable(stackTrace, NameRControlFlags.All
                                                                        & ~NameRControlFlags.IncludeHiddenStackFrames
                                                                        & ~NameRControlFlags.IncludeParameterDefaultValue
                                                                        & ~NameRControlFlags.StoreOriginalStackTraceInExceptionData
                                                                        & ~NameRControlFlags.IncludeSourceInformation);

            readableStackTrace.Should().Be(expectedReadableStackTrace);
        }
    }
}
