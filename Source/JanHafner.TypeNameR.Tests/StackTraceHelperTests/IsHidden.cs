using AwesomeAssertions;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using JanHafner.TypeNameR.Helper;
using NSubstitute;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;

namespace JanHafner.TypeNameR.Tests.StackTraceHelperTests;

public sealed class IsHidden
{
    [Fact]
    public void ReturnsTrueIfTheMethodHasAggressiveInliningFlag()
    {
        // Arrange
        var method = typeof(Methods).GetMethodOrThrow(nameof(Methods.MethodWithAggressiveInliningFlag));

        // Act
        var isHidden = method.IsHidden();

        // Assert
        isHidden.Should().BeTrue();
    }

    [Fact]
    public void ReturnsTrueIfTheMethodHasStackTraceHiddenAttribute()
    {
        // Arrange
        var method = typeof(Methods).GetMethodOrThrow(nameof(Methods.MethodWithStackTraceHiddenAttribute));

        // Act
        var isHidden = method.IsHidden();

        // Assert
        isHidden.Should().BeTrue();
    }

    [Fact]
    public void ReturnsTrueIfTheMethodDeclaringTypeHasStackTraceHiddenAttribute()
    {
        // Arrange
        var method = typeof(MethodsWithStackTraceHiddenAttribute).GetMethodOrThrow(nameof(MethodsWithStackTraceHiddenAttribute.Method));

        // Act
        var isHidden = method.IsHidden();

        // Assert
        isHidden.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalseIfTheMethodDeclaringTypeIsNull()
    {
        // Arrange
        var methodSubstitute = Substitute.For<MethodBase>();
        methodSubstitute.DeclaringType.Returns((Type?)null);

        // Act
        var isHidden = methodSubstitute.IsHidden();

        // Assert
        isHidden.Should().BeFalse();
    }

    [Fact]
    public void ReturnsFalse()
    {
        // Arrange
        var method = typeof(Methods).GetMethodOrThrow(nameof(Methods.Method));

        // Act
        var isHidden = method.IsHidden();

        // Assert
        isHidden.Should().BeFalse();
    }

    private sealed class Methods
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MethodWithAggressiveInliningFlag() => throw new NotImplementedException();

        [StackTraceHidden]
        public void MethodWithStackTraceHiddenAttribute() => throw new NotImplementedException();

        public void Method() => throw new NotImplementedException();
    }

    [StackTraceHidden]
    private sealed class MethodsWithStackTraceHiddenAttribute
    {
        public void Method() => throw new NotImplementedException();
    }
}