using FluentAssertions;
using JanHafner.TypeNameR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;
using System.Diagnostics;
using Xunit;

namespace Serilog.Extensions.TypeNameR.Tests.SerilogExtensionsTests;

public sealed class For
{
    private ILogger logger;

    private ITypeNameR? typeNameR;

    private bool fullTypeName;

    private bool throwExceptions;

    private bool selfLog;

    [Fact]
    public void ThrowsExceptionIfLoggerIsNullByInstance()
    {
        // Arrange
        var instance = new object();

        // Act, Assert
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
        var argumentNullException = Assert.Throws<ArgumentNullException>(() => CallByInstance(instance));
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
        argumentNullException.ParamName.Should().Be(nameof(logger));
    }

    [Fact]
    public void ThrowsExceptionIfInstanceIsNull()
    {
        // Arrange
        logger = Substitute.For<ILogger>();
        object instance = null;

        // Act, Assert
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
        var argumentNullException = Assert.Throws<ArgumentNullException>(() => CallByInstance(instance));
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
        argumentNullException.ParamName.Should().Be(nameof(instance));
    }

    [Fact]
    public void RethrowsExceptionOnErrorByInstance()
    {
        // Arrange
        logger = Substitute.For<ILogger>();
        var instance = Substitute.For<object>();

        var expectedException = Substitute.For<Exception>();

        throwExceptions = true;

        typeNameR = Substitute.For<ITypeNameR>();
        typeNameR.GenerateDisplay(instance.GetType(), fullTypeName, NameRControlFlags.All)
            .Throws(expectedException);

        // Act, Assert
        var thrownException = Assert.Throws(expectedException.GetType(), () => CallByInstance(instance));
        thrownException.Should().BeSameAs(expectedException);
    }

    [Fact]
    public void SelfLogsExceptionsByInstance()
    {
        // Arrange
        logger = Substitute.For<ILogger>();
        var instance = Substitute.For<object>();

        var expectedException = Substitute.For<Exception>();

        selfLog = true;
        throwExceptions = true;

        typeNameR = Substitute.For<ITypeNameR>();
        typeNameR.GenerateDisplay(instance.GetType(), fullTypeName, NameRControlFlags.All)
            .Throws(expectedException);

        // Act, Assert
        var thrownException = Assert.Throws(expectedException.GetType(), () => CallByInstance(instance));
        thrownException.Should().BeSameAs(expectedException);
        logger.Received(1).Verbose(thrownException, Arg.Any<string>(), instance.GetType().FullName);
    }

    [Fact]
    public void FallsBackToForContextWithTypeByInstance()
    {
        // Arrange
        logger = Substitute.For<ILogger>();
        logger.ReturnsForAll(logger);
        var instance = Substitute.For<object>();

        var expectedException = Substitute.For<Exception>();

        typeNameR = Substitute.For<ITypeNameR>();
        typeNameR.GenerateDisplay(instance.GetType(), fullTypeName, NameRControlFlags.All)
            .Throws(expectedException);

        // Act
        var returnedLogger = CallByInstance(instance);

        // Assert
        logger.Received(1).ForContext(instance.GetType());
        logger.Should().BeSameAs(returnedLogger);
    }

    [Fact]
    public void UsesTheGeneratedDisplayOfTypeByInstance()
    {
        // Arrange
        logger = Substitute.For<ILogger>();
        logger.ReturnsForAll(logger);
        var instance = Substitute.For<object>();

        var expectedDisplay = Guid.NewGuid().ToString();

        typeNameR = Substitute.For<ITypeNameR>();
        typeNameR.GenerateDisplay(instance.GetType(), fullTypeName, NameRControlFlags.All)
            .Returns(expectedDisplay);

        // Act
        var returnedLogger = CallByInstance(instance);

        // Assert
        logger.Received(1).ForContext(Core.Constants.SourceContextPropertyName, expectedDisplay);
        logger.Should().BeSameAs(returnedLogger);
    }

    [Fact]
    public void ThrowsExceptionIfTypeIsNull()
    {
        // Arrange
        logger = Substitute.For<ILogger>();
        Type type = null;

        // Act, Assert
        var argumentNullException = Assert.Throws<ArgumentNullException>(() => CallByType(type));
        argumentNullException.ParamName.Should().Be(nameof(type));
    }

    [Fact]
    public void ThrowsExceptionIfLoggerIsNullByType()
    {
        // Arrange
        var type = Substitute.For<Type>();

        // Act, Assert
        var argumentNullException = Assert.Throws<ArgumentNullException>(() => CallByType(type));
        argumentNullException.ParamName.Should().Be(nameof(logger));
    }

    [Fact]
    public void RethrowsExceptionOnErrorByType()
    {
        // Arrange
        logger = Substitute.For<ILogger>();
        var type = Substitute.For<Type>();

        var expectedException = Substitute.For<Exception>();

        throwExceptions = true;

        typeNameR = Substitute.For<ITypeNameR>();
        typeNameR.GenerateDisplay(Arg.Is<Type>(t => t == type), fullTypeName, NameRControlFlags.All)
            .Throws(expectedException);

        // Act, Assert
        var thrownException = Assert.Throws(expectedException.GetType(), () => CallByType(type));
        thrownException.Should().BeSameAs(expectedException);
    }

    [Fact]
    public void SelfLogsExceptionsByType()
    {
        // Arrange
        logger = Substitute.For<ILogger>();
        var type = Substitute.For<Type>();

        var expectedException = Substitute.For<Exception>();

        selfLog = true;
        throwExceptions = true;

        typeNameR = Substitute.For<ITypeNameR>();
        typeNameR.GenerateDisplay(Arg.Is<Type>(t => t == type), fullTypeName, NameRControlFlags.All)
            .Throws(expectedException);

        // Act, Assert
        var thrownException = Assert.Throws(expectedException.GetType(), () => CallByType(type));
        thrownException.Should().BeSameAs(expectedException);
        logger.Received(1).Verbose(thrownException, Arg.Any<string>(), type.FullName);
    }

    [Fact]
    public void FallsBackToForContextWithTypeByType()
    {
        // Arrange
        logger = Substitute.For<ILogger>();
        logger.ReturnsForAll(logger);
        var type = Substitute.For<Type>();

        var expectedException = Substitute.For<Exception>();

        typeNameR = Substitute.For<ITypeNameR>();
        typeNameR.GenerateDisplay(Arg.Is<Type>(t => t == type), fullTypeName, NameRControlFlags.All)
            .Throws(expectedException);

        // Act
        var returnedLogger = CallByType(type);

        // Assert
        logger.Received(1).ForContext(Arg.Is<Type>(t => t == type));
        logger.Should().BeSameAs(returnedLogger);
    }

    [Fact]
    public void UsesTheGeneratedDisplayOfTypeByType()
    {
        // Arrange
        logger = Substitute.For<ILogger>();
        logger.ReturnsForAll(logger);
        var type = Substitute.For<Type>();

        var expectedDisplay = Guid.NewGuid().ToString();

        typeNameR = Substitute.For<ITypeNameR>();
        typeNameR.GenerateDisplay(Arg.Is<Type>(t => t == type), fullTypeName, NameRControlFlags.All)
            .Returns(expectedDisplay);

        // Act
        var returnedLogger = CallByType(type);

        // Assert
        logger.Received(1).ForContext(Core.Constants.SourceContextPropertyName, expectedDisplay);
        logger.Should().BeSameAs(returnedLogger);
    }

    [DebuggerStepThrough]
    private ILogger CallByInstance<T>(T instance)
        where T : notnull
        => logger.For(instance, typeNameR, fullTypeName, NameRControlFlags.All, throwExceptions, selfLog);

    [DebuggerStepThrough]
    private ILogger CallByType(Type type) => logger.For(type, typeNameR, fullTypeName, NameRControlFlags.All, throwExceptions, selfLog);
}