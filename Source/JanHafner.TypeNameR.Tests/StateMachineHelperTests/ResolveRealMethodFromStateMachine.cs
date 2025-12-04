using AwesomeAssertions;
using JanHafner.TypeNameR.Helper;
using NSubstitute;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;

namespace JanHafner.TypeNameR.Tests.StateMachineHelperTests;

public sealed class ResolveRealMethodFromStateMachine
{
    [Fact]
    public void ReturnsAsyncStateMachineIfSetUpOnMethodInfoDirectly()
    {
        // Arrange
        var declaringTypeSubstitute = Substitute.For<Type>();

        // HINT: Using collection initializer feature will fail this test
        var methodInfoSubstitute = Substitute.For<MethodInfo>();
        // Necessary setup to fix fail on net 7 and above
        methodInfoSubstitute.GetCustomAttributes(typeof(StateMachineAttribute), false)
            .Returns(new[] { new AsyncStateMachineAttribute(declaringTypeSubstitute) });

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoSubstitute.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeSameAs(methodInfoSubstitute);
        stateMachineType.Should().Be(StateMachineType.Async);
    }

    [Fact]
    public void ReturnsNoneIfDeclaringTypeIsNull()
    {
        // Arrange
        var methodInfoSubstitute = Substitute.For<MethodInfo>();
        methodInfoSubstitute.DeclaringType.Returns((Type?)null);
        // Necessary setup to fix fail on net 7 and above
        methodInfoSubstitute.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(Array.Empty<Attribute>());

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoSubstitute.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeNull();
        stateMachineType.Should().Be(StateMachineType.None);
    }

    [Fact]
    public void ReturnsNoneIfDeclaringTypeOfDeclaringTypeIsNull()
    {
        // Arrange
        var declaringTypeSubstitute = Substitute.For<Type>();

        var methodInfoSubstitute = Substitute.For<MethodInfo>();
        methodInfoSubstitute.DeclaringType.Returns(declaringTypeSubstitute);
        // Necessary setup to fix fail on net 7 and above
        methodInfoSubstitute.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(Array.Empty<Attribute>());

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoSubstitute.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeNull();
        stateMachineType.Should().Be(StateMachineType.None);
    }

    [Fact]
    public void ReturnsNoneIfDeclaringTypeIsNotCompilerGenerated()
    {
        // Arrange
        var declaringTypeOfDeclaringTypeSubstitute = Substitute.For<Type>();

        var declaringTypeSubstitute = Substitute.For<Type>();
        declaringTypeSubstitute.DeclaringType.Returns(declaringTypeOfDeclaringTypeSubstitute);
        declaringTypeSubstitute.IsDefined(typeof(CompilerGeneratedAttribute), Arg.Any<bool>()).Returns(false);

        var methodInfoSubstitute = Substitute.For<MethodInfo>();
        methodInfoSubstitute.DeclaringType.Returns(declaringTypeSubstitute);
        // Necessary setup to fix fail on net 7 and above
        methodInfoSubstitute.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(Array.Empty<Attribute>());

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoSubstitute.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeNull();
        stateMachineType.Should().Be(StateMachineType.None);
    }

    [Fact]
    public void ReturnsNoneIfDeclaringTypeHasNoPossibleMethods()
    {
        // Arrange
        var declaringTypeOfDeclaringTypeSubstitute = Substitute.For<Type>();

        var declaringTypeSubstitute = Substitute.For<Type>();
        declaringTypeSubstitute.DeclaringType.Returns(declaringTypeOfDeclaringTypeSubstitute);
        declaringTypeSubstitute.IsDefined(typeof(CompilerGeneratedAttribute), Arg.Any<bool>()).Returns(true);

        var methodInfoSubstitute = Substitute.For<MethodInfo>();
        methodInfoSubstitute.DeclaringType.Returns(declaringTypeSubstitute);
        // Necessary setup to fix fail on net 7 and above
        methodInfoSubstitute.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(Array.Empty<Attribute>());

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoSubstitute.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeNull();
        stateMachineType.Should().Be(StateMachineType.None);
    }

    [Fact]
    public void ReturnsNoneIfNoPossibleMethodWasFound()
    {
        // Arrange
        var possibleMethodSubstitute = Substitute.For<MethodInfo>();
        // Necessary setup to fix fail on net 7 and above
        possibleMethodSubstitute.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(Array.Empty<Attribute>());

        var declaringTypeOfDeclaringTypeSubstitute = Substitute.For<Type>();
        declaringTypeOfDeclaringTypeSubstitute.GetMethods(BindingFlags.Public
                                                          | BindingFlags.NonPublic
                                                          | BindingFlags.Static
                                                          | BindingFlags.Instance
                                                          | BindingFlags.DeclaredOnly)
            .Returns([possibleMethodSubstitute]);

        var declaringTypeSubstitute = Substitute.For<Type>();
        declaringTypeSubstitute.DeclaringType.Returns(declaringTypeOfDeclaringTypeSubstitute);
        declaringTypeSubstitute.IsDefined(typeof(CompilerGeneratedAttribute), Arg.Any<bool>()).Returns(true);

        var methodInfoSubstitute = Substitute.For<MethodInfo>();
        methodInfoSubstitute.DeclaringType.Returns(declaringTypeSubstitute);
        // Necessary setup to fix fail on net 7 and above
        methodInfoSubstitute.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(Array.Empty<Attribute>());

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoSubstitute.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeNull();
        stateMachineType.Should().Be(StateMachineType.None);
    }

    [Fact]
    public void ReturnsAsyncStateMachine()
    {
        // Arrange
        var declaringTypeSubstitute = Substitute.For<Type>();
        declaringTypeSubstitute.IsDefined(typeof(CompilerGeneratedAttribute), Arg.Any<bool>())
            .Returns(true);

        // HINT: Using collection initializer feature will fail this test
        var possibleMethodSubstitute = Substitute.For<MethodInfo>();
        possibleMethodSubstitute.GetCustomAttributes(typeof(StateMachineAttribute), false)
            .Returns(new[] { new AsyncStateMachineAttribute(declaringTypeSubstitute) });

        var declaringTypeOfDeclaringTypeSubstitute = Substitute.For<Type>();
        declaringTypeOfDeclaringTypeSubstitute.GetMethods(BindingFlags.Public
                                                          | BindingFlags.NonPublic
                                                          | BindingFlags.Static
                                                          | BindingFlags.Instance
                                                          | BindingFlags.DeclaredOnly)
            .Returns(new[] { possibleMethodSubstitute });

        declaringTypeSubstitute.DeclaringType.Returns(declaringTypeOfDeclaringTypeSubstitute);

        var methodInfoSubstitute = Substitute.For<MethodInfo>();
        methodInfoSubstitute.DeclaringType.Returns(declaringTypeSubstitute);
        // Necessary setup to fix fail on net 7 and above
        methodInfoSubstitute.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(Array.Empty<Attribute>());

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoSubstitute.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeSameAs(possibleMethodSubstitute);
        stateMachineType.Should().Be(StateMachineType.Async);
    }

    [Fact]
    public void ReturnsIteratorStateMachine()
    {
        // Arrange
        var declaringTypeSubstitute = Substitute.For<Type>();
        declaringTypeSubstitute.IsDefined(typeof(CompilerGeneratedAttribute), Arg.Any<bool>())
            .Returns(true);

        var possibleMethodSubstitute = Substitute.For<MethodInfo>();
        // Necessary setup to fix fail on net 7 and above
        possibleMethodSubstitute.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(Array.Empty<Attribute>());
        // Necessary setup for test
        // HINT: Using collection initializer feature will fail this test
        possibleMethodSubstitute.GetCustomAttributes(typeof(StateMachineAttribute), false)
            .Returns(new[] { new IteratorStateMachineAttribute(declaringTypeSubstitute) });

        var declaringTypeOfDeclaringTypeSubstitute = Substitute.For<Type>();
        declaringTypeOfDeclaringTypeSubstitute.GetMethods(BindingFlags.Public
                                                          | BindingFlags.NonPublic
                                                          | BindingFlags.Static
                                                          | BindingFlags.Instance
                                                          | BindingFlags.DeclaredOnly)
            .Returns(new[] { possibleMethodSubstitute });

        declaringTypeSubstitute.DeclaringType.Returns(declaringTypeOfDeclaringTypeSubstitute);

        var methodInfoSubstitute = Substitute.For<MethodInfo>();
        methodInfoSubstitute.DeclaringType.Returns(declaringTypeSubstitute);
        // Necessary setup to fix fail on net 7 and above
        methodInfoSubstitute.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(Array.Empty<Attribute>());

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoSubstitute.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeSameAs(possibleMethodSubstitute);
        stateMachineType.Should().Be(StateMachineType.Iterator);
    }

    [Fact]
    public void ReturnsAsyncIteratorStateMachine()
    {
        // Arrange
        var declaringTypeSubstitute = Substitute.For<Type>();
        declaringTypeSubstitute.IsDefined(typeof(CompilerGeneratedAttribute), Arg.Any<bool>()).Returns(true);

        var possibleMethodSubstitute = Substitute.For<MethodInfo>();
        // Necessary setup to fix fail on net 7 and above
        possibleMethodSubstitute.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(Array.Empty<Attribute>());
        // Necessary setup for test
        // HINT: Using collection initializer feature will fail this test
        possibleMethodSubstitute.GetCustomAttributes(typeof(StateMachineAttribute), false)
            .Returns(new[] { new AsyncIteratorStateMachineAttribute(declaringTypeSubstitute) });

        var declaringTypeOfDeclaringTypeSubstitute = Substitute.For<Type>();
        declaringTypeOfDeclaringTypeSubstitute.GetMethods(BindingFlags.Public
                                                          | BindingFlags.NonPublic
                                                          | BindingFlags.Static
                                                          | BindingFlags.Instance
                                                          | BindingFlags.DeclaredOnly)
            .Returns(new[] { possibleMethodSubstitute });

        declaringTypeSubstitute.DeclaringType.Returns(declaringTypeOfDeclaringTypeSubstitute);

        var methodInfoSubstitute = Substitute.For<MethodInfo>();
        methodInfoSubstitute.DeclaringType.Returns(declaringTypeSubstitute);
        // Necessary setup to fix fail on net 7 and above
        methodInfoSubstitute.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(Array.Empty<Attribute>());

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoSubstitute.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeSameAs(possibleMethodSubstitute);
        stateMachineType.Should().Be(StateMachineType.AsyncIterator);
    }

    [Fact]
    public void ThrowsExceptionOnUnknownStateMachineAttribute()
    {
        // Arrange
        var declaringTypeSubstitute = Substitute.For<Type>();
        declaringTypeSubstitute.IsDefined(typeof(CompilerGeneratedAttribute), Arg.Any<bool>()).Returns(true);

        var unknownStateMachineAttributeSubstitute = Substitute.For<StateMachineAttribute>(declaringTypeSubstitute);

        var possibleMethodSubstitute = Substitute.For<MethodInfo>();
        // Necessary setup to fix fail on net 7 and above
        possibleMethodSubstitute.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(Array.Empty<Attribute>());
        // Necessary setup for test
        // HINT: Using collection initializer feature will fail this test
        possibleMethodSubstitute.GetCustomAttributes(typeof(StateMachineAttribute), false)
            .Returns(new[] { unknownStateMachineAttributeSubstitute });

        var declaringTypeOfDeclaringTypeSubstitute = Substitute.For<Type>();
        declaringTypeOfDeclaringTypeSubstitute.GetMethods(BindingFlags.Public
                                                          | BindingFlags.NonPublic
                                                          | BindingFlags.Static
                                                          | BindingFlags.Instance
                                                          | BindingFlags.DeclaredOnly)
            .Returns(new[] { possibleMethodSubstitute });

        declaringTypeSubstitute.DeclaringType.Returns(declaringTypeOfDeclaringTypeSubstitute);

        var methodInfoSubstitute = Substitute.For<MethodInfo>();
        methodInfoSubstitute.DeclaringType.Returns(declaringTypeSubstitute);
        // Necessary setup to fix fail on net 7 and above
        methodInfoSubstitute.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(Array.Empty<Attribute>());

        // Act, Assert
        var exception = Assert.Throws<InvalidOperationException>(() => methodInfoSubstitute.ResolveRealMethodFromStateMachine(out _));
        exception.Message.Should().StartWith("Unknown");
    }
}