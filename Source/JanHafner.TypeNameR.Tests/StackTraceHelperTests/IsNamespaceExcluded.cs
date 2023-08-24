using FluentAssertions;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using JanHafner.TypeNameR.Helper;
using NSubstitute;
using System.Reflection;
using Xunit;

namespace JanHafner.TypeNameR.Tests.StackTraceHelperTests;

public sealed class IsNamespaceExcluded
{
    private string[] namespaces;

    [Fact]
    public void ReturnsFalseIfNamespacesAreEmpty()
    {
        // Arrange
        namespaces = Array.Empty<string>();
        var method = typeof(Methods).GetMethodOrThrow(nameof(Methods.Method));

        // Act
        var isNamespaceExcluded = method.IsNamespaceExcluded(namespaces);

        // Assert
        isNamespaceExcluded.Should().BeFalse();
    }

    [Fact]
    public void ReturnsFalseIfMethodDeclaringTypeIsNull()
    {
        // Arrange
        namespaces = new[] { "JanHafner.TypeNameR.Tests.StackTraceHelperTests" };
        var methodSubstitute = Substitute.For<MethodBase>();
        methodSubstitute.DeclaringType.Returns((Type?)null);

        // Act
        var isNamespaceExcluded = methodSubstitute.IsNamespaceExcluded(namespaces);

        // Assert
        isNamespaceExcluded.Should().BeFalse();
    }

    [Fact]
    public void ReturnsFalseIfNamespaceOfMethodDeclaringTypeIsEmpty()
    {
        // Arrange
        namespaces = new[] { "JanHafner.TypeNameR.Tests.StackTraceHelperTests" };

        var methodDeclaringTypeSubstitute = Substitute.For<Type>();
        methodDeclaringTypeSubstitute.Namespace.Returns((string?)null);

        var methodSubstitute = Substitute.For<MethodBase>();
        methodSubstitute.DeclaringType.Returns(methodDeclaringTypeSubstitute);

        // Act
        var isNamespaceExcluded = methodSubstitute.IsNamespaceExcluded(namespaces);

        // Assert
        isNamespaceExcluded.Should().BeFalse();
    }

    [Fact]
    public void ReturnsFalseIfNamespaceIsNotExluded()
    {
        // Arrange
        namespaces = new[] { "System" };
        var method = typeof(Methods).GetMethodOrThrow(nameof(Methods.Method));

        // Act
        var isNamespaceExcluded = method.IsNamespaceExcluded(namespaces);

        // Assert
        isNamespaceExcluded.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue()
    {
        // Arrange
        namespaces = new[] { "JanHafner.TypeNameR.Tests.StackTraceHelperTests" };
        var method = typeof(Methods).GetMethodOrThrow(nameof(Methods.Method));

        // Act
        var isNamespaceExcluded = method.IsNamespaceExcluded(namespaces);

        // Assert
        isNamespaceExcluded.Should().BeTrue();
    }

    private sealed class Methods
    {
        public void Method() => throw new NotImplementedException();
    }
}