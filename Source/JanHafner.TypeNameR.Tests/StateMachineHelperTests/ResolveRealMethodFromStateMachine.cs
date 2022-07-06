using FluentAssertions;
using Moq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;

namespace JanHafner.TypeNameR.Tests.StateMachineHelperTests;

public sealed class ResolveRealMethodFromStateMachine
{
    [Fact]
    public void ReturnsNoneIfDeclaringTypeIsNull()
    {
        // Arrange
        var methodInfoMock = new Mock<MethodInfo>();
        methodInfoMock.Setup(mi => mi.DeclaringType)
                      .Returns((Type?)null);

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoMock.Object.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeNull();
        stateMachineType.Should().Be(StateMachineTypes.None);
    }

    [Fact]
    public void ReturnsNoneIfDeclaringTypeOfDeclaringTypeIsNull()
    {
        // Arrange
        var declaringTypeMock = new Mock<Type>();
        
        var methodInfoMock = new Mock<MethodInfo>();
        methodInfoMock.Setup(mi => mi.DeclaringType)
                      .Returns(declaringTypeMock.Object);

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoMock.Object.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeNull();
        stateMachineType.Should().Be(StateMachineTypes.None);
    }

    [Fact]
    public void ReturnsNoneIfDeclaringTypeIsNotCompilerGenerated()
    {
        // Arrange
        var declaringTypeOfDeclaringTypeMock = new Mock<Type>();

        var declaringTypeMock = new Mock<Type>();
        declaringTypeMock.Setup(t => t.DeclaringType)
                         .Returns(declaringTypeOfDeclaringTypeMock.Object);
        declaringTypeMock.Setup(t => t.IsDefined(typeof(CompilerGeneratedAttribute), It.IsAny<bool>()))
                         .Returns(false);

        var methodInfoMock = new Mock<MethodInfo>();
        methodInfoMock.Setup(mi => mi.DeclaringType)
                      .Returns(declaringTypeMock.Object);

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoMock.Object.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeNull();
        stateMachineType.Should().Be(StateMachineTypes.None);
    }

    [Fact]
    public void ReturnsNoneIfDeclaringTypeHasNoPossibleMethods()
    {
        // Arrange
        var declaringTypeOfDeclaringTypeMock = new Mock<Type>();

        var declaringTypeMock = new Mock<Type>();
        declaringTypeMock.Setup(t => t.DeclaringType)
                         .Returns(declaringTypeOfDeclaringTypeMock.Object);
        declaringTypeMock.Setup(t => t.IsDefined(typeof(CompilerGeneratedAttribute), It.IsAny<bool>()))
                         .Returns(true);

        var methodInfoMock = new Mock<MethodInfo>();
        methodInfoMock.Setup(mi => mi.DeclaringType)
                      .Returns(declaringTypeMock.Object);

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoMock.Object.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeNull();
        stateMachineType.Should().Be(StateMachineTypes.None);
    }

    [Fact]
    public void ReturnsNoneIfNoPossibleMethodWasFound()
    {
        // Arrange
        var declaringTypeOfDeclaringTypeMock = new Mock<Type>();
        declaringTypeOfDeclaringTypeMock.Setup(t => t.GetMethods(BindingFlags.Public
                                                               | BindingFlags.NonPublic
                                                               | BindingFlags.Static
                                                               | BindingFlags.Instance
                                                               | BindingFlags.DeclaredOnly))
                                        .Returns(new[]
                                        {
                                            new Mock<MethodInfo>().Object
                                        });

        var declaringTypeMock = new Mock<Type>();
        declaringTypeMock.Setup(t => t.DeclaringType)
                         .Returns(declaringTypeOfDeclaringTypeMock.Object);
        declaringTypeMock.Setup(t => t.IsDefined(typeof(CompilerGeneratedAttribute), It.IsAny<bool>()))
                         .Returns(true);

        var methodInfoMock = new Mock<MethodInfo>();
        methodInfoMock.Setup(mi => mi.DeclaringType)
                      .Returns(declaringTypeMock.Object);

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoMock.Object.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeNull();
        stateMachineType.Should().Be(StateMachineTypes.None);
    }

    [Fact]
    public void ReturnsAsyncStateMachine()
    {
        // Arrange
        var declaringTypeMock = new Mock<Type>();
        declaringTypeMock.Setup(t => t.IsDefined(typeof(CompilerGeneratedAttribute), It.IsAny<bool>()))
                         .Returns(true);

        var possibleMethodMock = new Mock<MethodInfo>();
        possibleMethodMock.Setup(m => m.GetCustomAttributes(typeof(AsyncStateMachineAttribute), It.IsAny<bool>()))
                          .Returns(new[]
                          {
                              new AsyncStateMachineAttribute(declaringTypeMock.Object)
                          });

        var declaringTypeOfDeclaringTypeMock = new Mock<Type>();
        declaringTypeOfDeclaringTypeMock.Setup(t => t.GetMethods(BindingFlags.Public
                                                               | BindingFlags.NonPublic
                                                               | BindingFlags.Static
                                                               | BindingFlags.Instance
                                                               | BindingFlags.DeclaredOnly))
                                        .Returns(new[]
                                        {
                                            possibleMethodMock.Object
                                        });

        declaringTypeMock.Setup(t => t.DeclaringType)
                         .Returns(declaringTypeOfDeclaringTypeMock.Object);

        var methodInfoMock = new Mock<MethodInfo>();
        methodInfoMock.Setup(mi => mi.DeclaringType)
                      .Returns(declaringTypeMock.Object);

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoMock.Object.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeSameAs(possibleMethodMock.Object);
        stateMachineType.Should().Be(StateMachineTypes.Async);
    }

    [Fact]
    public void ReturnsIteratorStateMachine()
    {
        // Arrange
        var declaringTypeMock = new Mock<Type>();
        declaringTypeMock.Setup(t => t.IsDefined(typeof(CompilerGeneratedAttribute), It.IsAny<bool>()))
                         .Returns(true);

        var possibleMethodMock = new Mock<MethodInfo>();
        possibleMethodMock.Setup(m => m.GetCustomAttributes(typeof(IteratorStateMachineAttribute), It.IsAny<bool>()))
                          .Returns(new[]
                          {
                              new IteratorStateMachineAttribute(declaringTypeMock.Object)
                          });

        var declaringTypeOfDeclaringTypeMock = new Mock<Type>();
        declaringTypeOfDeclaringTypeMock.Setup(t => t.GetMethods(BindingFlags.Public
                                                               | BindingFlags.NonPublic
                                                               | BindingFlags.Static
                                                               | BindingFlags.Instance
                                                               | BindingFlags.DeclaredOnly))
                                        .Returns(new[]
                                        {
                                            possibleMethodMock.Object
                                        });

        declaringTypeMock.Setup(t => t.DeclaringType)
                         .Returns(declaringTypeOfDeclaringTypeMock.Object);

        var methodInfoMock = new Mock<MethodInfo>();
        methodInfoMock.Setup(mi => mi.DeclaringType)
                      .Returns(declaringTypeMock.Object);

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoMock.Object.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeSameAs(possibleMethodMock.Object);
        stateMachineType.Should().Be(StateMachineTypes.Iterator);
    }

    [Fact]
    public void ReturnsAsyncIteratorStateMachine()
    {
        // Arrange
        var declaringTypeMock = new Mock<Type>();
        declaringTypeMock.Setup(t => t.IsDefined(typeof(CompilerGeneratedAttribute), It.IsAny<bool>()))
                         .Returns(true);

        var possibleMethodMock = new Mock<MethodInfo>();
        possibleMethodMock.Setup(m => m.GetCustomAttributes(typeof(AsyncIteratorStateMachineAttribute), It.IsAny<bool>()))
                          .Returns(new[]
                          {
                              new AsyncIteratorStateMachineAttribute(declaringTypeMock.Object)
                          });

        var declaringTypeOfDeclaringTypeMock = new Mock<Type>();
        declaringTypeOfDeclaringTypeMock.Setup(t => t.GetMethods(BindingFlags.Public
                                                               | BindingFlags.NonPublic
                                                               | BindingFlags.Static
                                                               | BindingFlags.Instance
                                                               | BindingFlags.DeclaredOnly))
                                        .Returns(new[]
                                        {
                                            possibleMethodMock.Object
                                        });

        declaringTypeMock.Setup(t => t.DeclaringType)
                         .Returns(declaringTypeOfDeclaringTypeMock.Object);

        var methodInfoMock = new Mock<MethodInfo>();
        methodInfoMock.Setup(mi => mi.DeclaringType)
                      .Returns(declaringTypeMock.Object);

        // Act
        MethodInfo? realMethodInfo = null;
        var stateMachineType = methodInfoMock.Object.ResolveRealMethodFromStateMachine(out realMethodInfo);

        // Assert
        realMethodInfo.Should().BeSameAs(possibleMethodMock.Object);
        stateMachineType.Should().Be(StateMachineTypes.AsyncIterator);
    }
}
