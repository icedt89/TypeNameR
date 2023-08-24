using FluentAssertions;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using JanHafner.TypeNameR.Helper;
using System.Diagnostics;
using Xunit;

namespace JanHafner.TypeNameR.Tests.StackTraceHelperTests;

public sealed class IsSkippable
{
    [Fact]
    public void ReturnsTrueIfHiddenStackFramesAreExcludedAndTheStackFrameIsHidden()
    {
        // Arrange
        var excludedNamespaces = Array.Empty<string>();
        var nameRControlFlags = NameRControlFlags.None;
        var method = typeof(MethodsWithStackTraceHiddenAttribute).GetMethodOrThrow(nameof(MethodsWithStackTraceHiddenAttribute.Method));

        // Act
        var shouldSkipStackFrame = method.IsSkippable(nameRControlFlags, excludedNamespaces);

        // Assert
        shouldSkipStackFrame.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalseIfHiddenStackFramesAreIncludedAndTheStackFrameIsHidden()
    {
        // Arrange
        var excludedNamespaces = Array.Empty<string>();
        var nameRControlFlags = NameRControlFlags.IncludeHiddenStackFrames;
        var method = typeof(MethodsWithStackTraceHiddenAttribute).GetMethodOrThrow(nameof(MethodsWithStackTraceHiddenAttribute.Method));

        // Act
        var shouldSkipStackFrame = method.IsSkippable(nameRControlFlags, excludedNamespaces);

        // Assert
        shouldSkipStackFrame.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrueIfNamespaceExclusionIsEnabledAndNamespaceIsExcluded()
    {
        // Arrange
        var excludedNamespaces = new[] { "JanHafner", };
        var nameRControlFlags = NameRControlFlags.IncludeHiddenStackFrames | NameRControlFlags.ExcludeStackFrameMethodsByNamespace;
        var method = typeof(MethodsWithStackTraceHiddenAttribute).GetMethodOrThrow(nameof(MethodsWithStackTraceHiddenAttribute.Method));

        // Act
        var shouldSkipStackFrame = method.IsSkippable(nameRControlFlags, excludedNamespaces);

        // Assert
        shouldSkipStackFrame.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalseIfNamespaceExclusionIsEnabledAndNamespaceIsNotExcluded()
    {
        // Arrange
        var excludedNamespaces = Array.Empty<string>();
        var nameRControlFlags = NameRControlFlags.IncludeHiddenStackFrames | NameRControlFlags.ExcludeStackFrameMethodsByNamespace;
        var method = typeof(MethodsWithStackTraceHiddenAttribute).GetMethodOrThrow(nameof(MethodsWithStackTraceHiddenAttribute.Method));

        // Act
        var shouldSkipStackFrame = method.IsSkippable(nameRControlFlags, excludedNamespaces);

        // Assert
        shouldSkipStackFrame.Should().BeFalse();
    }

    [StackTraceHidden]
    private sealed class MethodsWithStackTraceHiddenAttribute
    {
        public void Method() => throw new NotImplementedException();
    }
}