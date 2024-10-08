﻿using JanHafner.TypeNameR.Experimental.Helper;
using JanHafner.TypeNameR.Experimental.StackTrace;
using System.Diagnostics;
using System.Reflection;
using System.Text;
#if NET6_0
using NullabilityInfoContext = Nullability.NullabilityInfoContextEx;
#endif

namespace JanHafner.TypeNameR.Experimental;

/// <inheritdoc />
public partial class TypeNameR
{
    /// <inheritdoc />
    public string GenerateDisplay(System.Diagnostics.StackTrace stackTrace, NameRControlFlags nameRControlFlags)
    {
        ArgumentNullException.ThrowIfNull(stackTrace);

        var stringBuilder = new StringBuilder();
        var nullabilityInfoContext = new NullabilityInfoContext();

        ProcessStackTrace(stringBuilder, nullabilityInfoContext, stackTrace, nameRControlFlags);

        return stringBuilder.ToString();
    }

    /// <inheritdoc />
    public string GenerateDisplay(StackFrame stackFrame, NameRControlFlags nameRControlFlags)
    {
        ArgumentNullException.ThrowIfNull(stackFrame);

        var stringBuilder = new StringBuilder();
        var nullabilityInfoContext = new NullabilityInfoContext();

        ProcessStackFrame(stringBuilder, nullabilityInfoContext, stackFrame, stackFrame.GetMethod(), Constants.DefaultCallDepth, nameRControlFlags);

        return stringBuilder.ToString();
    }

    private void ProcessStackTrace(StringBuilder stringBuilder, NullabilityInfoContext nullabilityInfoContext, System.Diagnostics.StackTrace stackTrace,
        NameRControlFlags nameRControlFlags)
    {
        if (nameRControlFlags.HasFlag(NameRControlFlags.DontEliminateRecursiveStackFrames))
        {
            ProcessNonRecursiveStackTrace(stringBuilder, nullabilityInfoContext, stackTrace, nameRControlFlags);

            return;
        }

        ProcessRecursiveStackTrace(stringBuilder, nullabilityInfoContext, stackTrace, nameRControlFlags);
    }

    private void ProcessNonRecursiveStackTrace(StringBuilder stringBuilder, NullabilityInfoContext nullabilityInfoContext,
        System.Diagnostics.StackTrace stackTrace,
        NameRControlFlags nameRControlFlags)
    {
        for (var stackFrameIndex = 0; stackFrameIndex < stackTrace.FrameCount; stackFrameIndex++)
        {
            if (!stackTrace.TryGetStackFrame(stackFrameIndex, nameRControlFlags.HasFlag(NameRControlFlags.IncludeHiddenStackFrames), out var stackFrame, out var stackFrameMethod))
            {
                continue;
            }

            ProcessStackFrame(stringBuilder, nullabilityInfoContext, stackFrame, stackFrameMethod, Constants.DefaultCallDepth, nameRControlFlags);

            if (stackFrameIndex < stackTrace.FrameCount - 1)
            {
                stringBuilder.AppendLine();
            }
        }
    }

    private void ProcessRecursiveStackTrace(StringBuilder stringBuilder, NullabilityInfoContext nullabilityInfoContext,
        System.Diagnostics.StackTrace stackTrace,
        NameRControlFlags nameRControlFlags)
    {
        foreach (var recursiveStackFrameMetadata2 in stackTrace.EnumerateRecursiveStackFrames(nameRControlFlags.HasFlag(NameRControlFlags.IncludeHiddenStackFrames)))
        {
            ProcessStackFrame(stringBuilder, nullabilityInfoContext, recursiveStackFrameMetadata2.StackFrame, recursiveStackFrameMetadata2.Method, recursiveStackFrameMetadata2.CallDepth, nameRControlFlags);

            stringBuilder.AppendLine();
        }

        stringBuilder.Remove(stringBuilder.Length - Environment.NewLine.Length, Environment.NewLine.Length);
    }

    private void ProcessStackFrame(StringBuilder stringBuilder, NullabilityInfoContext nullabilityInfoContext, StackFrame stackFrame, MethodBase? methodBase,
        int callDepth, NameRControlFlags nameRControlFlags)
    {
        stringBuilder.AppendStackFramePreamble();

        if (methodBase is null)
        {
            stringBuilder.AppendUnknownStackFrameName();
        }
        else
        {
            ProcessMethod(stringBuilder, nullabilityInfoContext, methodBase, nameRControlFlags, out var stateMachineType);

            if (stateMachineType.HasFlag(StateMachineType.Iterator))
            {
                stringBuilder.AppendMoveNextCallSuffix();
            }

            if (callDepth > Constants.DefaultCallDepth)
            {
                stringBuilder.AppendCallDepth(callDepth);
            }
        }

        if (!nameRControlFlags.HasFlag(NameRControlFlags.IncludeSourceInformation))
        {
            return;
        }

        var stackFrameMetadata = stackFrame.GetStackFrameMetadata();
        if (stackFrameMetadata.IsEmpty && methodBase is not null && nameRControlFlags.HasFlag(NameRControlFlags.FallbackToStackFrameMetadataProvider) && stackFrameMetadataProvider is not null)
        {
            stackFrameMetadata = stackFrameMetadataProvider.ProvideStackFrameMetadata(stackFrame, methodBase);
        }

        if (stackFrameMetadata.IsEmpty)
        {
            return;
        }

        stringBuilder.AppendStackFrameMetadata(stackFrameMetadata);
    }

    /// <inheritdoc />
    public string GenerateDisplay(Exception exception, NameRControlFlags nameRControlFlags)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var stackTrace = new System.Diagnostics.StackTrace(exception, nameRControlFlags.HasFlag(NameRControlFlags.IncludeSourceInformation));

        var stringBuilder = new StringBuilder();

        // Write exception header
        ProcessTypeCore(stringBuilder, exception.GetType(), true, null, null, nameRControlFlags);

        stringBuilder.AppendExceptionMessage(exception.Message);
        stringBuilder.AppendLine();

        var nullabilityInfoContext = new NullabilityInfoContext();

        // Write exception stacktrace
        ProcessStackTrace(stringBuilder, nullabilityInfoContext, stackTrace, nameRControlFlags);

        return stringBuilder.ToString();
    }

    /// <inheritdoc />
    public TException RewriteStackTrace<TException>(TException exception,
                                                    NameRControlFlags nameRControlFlags)
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(exception);

        var nullabilityInfoContext = new NullabilityInfoContext();

        RewriteStackTraceCore(exception, nullabilityInfoContext, nameRControlFlags);

        return exception;
    }

    private void RewriteStackTraceCore<TException>(TException exception, NullabilityInfoContext nullabilityInfoContext, NameRControlFlags nameRControlFlags)
          where TException : Exception
    {
        if (exception.StackTrace is not null)
        {
            var stringBuilder = new StringBuilder();

            var originalStackTrace = new System.Diagnostics.StackTrace(exception, nameRControlFlags.HasFlag(NameRControlFlags.IncludeSourceInformation));

            ProcessStackTrace(stringBuilder, nullabilityInfoContext, originalStackTrace, nameRControlFlags);

            exception.SetStackTrace(stringBuilder, nameRControlFlags.HasFlag(NameRControlFlags.StoreOriginalStackTraceInExceptionData));
        }

        if (!nameRControlFlags.HasFlag(NameRControlFlags.IncludeInnerExceptions))
        {
            return;
        }

        if (exception.InnerException is not null)
        {
            RewriteStackTraceCore(exception.InnerException, nullabilityInfoContext, nameRControlFlags);
        }

        if (exception is not AggregateException aggregateException)
        {
            return;
        }

        foreach (var innerException in aggregateException.InnerExceptions)
        {
            RewriteStackTraceCore(innerException, nullabilityInfoContext, nameRControlFlags);
        }
    }
}