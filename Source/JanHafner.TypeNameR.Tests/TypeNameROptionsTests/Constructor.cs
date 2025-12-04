using AwesomeAssertions;
using JanHafner.TypeNameR.Helper;
using Xunit;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace JanHafner.TypeNameR.Tests.TypeNameROptionsTests;

public sealed class Constructor
{
    [Fact]
    public void InitializesPredefinedTypeNamesToEmptyDictionaryIfNull()
    {
        // Arrange
        IReadOnlyDictionary<Type, string>? predefinedTypeNames = null;

        // Act
        var typeNameROptions = new TypeNameROptions(predefinedTypeNames);

        // Assert
        typeNameROptions.PredefinedTypeNames.Should()
            .BeEmpty()
            .And.BeOfType<EmptyDictionary<Type, string>>();
    }

    [Fact]
    public void InitializesPredefinedTypeNamesToEmptyDictionaryIfEmpty()
    {
        // Arrange
        IReadOnlyDictionary<Type, string> predefinedTypeNames = new Dictionary<Type, string>(0);

        // Act
        var typeNameROptions = new TypeNameROptions(predefinedTypeNames);

        // Assert
        typeNameROptions.PredefinedTypeNames.Should()
            .BeEmpty()
            .And.BeOfType<EmptyDictionary<Type, string>>();
    }

    [Fact]
    public void InitializesPredefinedTypeNamesCorrectly()
    {
        // Arrange
        IReadOnlyDictionary<Type, string> predefinedTypeNames = new Dictionary<Type, string>(1)
        {
            { typeof(string), "string" },
        };

        // Act
        var typeNameROptions = new TypeNameROptions(predefinedTypeNames);

        // Assert
        typeNameROptions.PredefinedTypeNames.Should()
            .NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(kvp => kvp.Key == typeof(string) && kvp.Value == "string")
            .And.NotBeSameAs(predefinedTypeNames);
#if NET8_0_OR_GREATER
        typeNameROptions.PredefinedTypeNames.Should().BeAssignableTo<FrozenDictionary<Type, string>>();
#endif
    }

#if NET8_0_OR_GREATER
    [Fact]
    public void InitializesPredefinedTypeNamesCorrectlyButReusesFrozenDictionary()
    {
        // Arrange
        IReadOnlyDictionary<Type, string> predefinedTypeNames = new Dictionary<Type, string>(1)
        {
            { typeof(string), "string" },
        }.ToFrozenDictionary();
        
        // Act
        var typeNameROptions = new TypeNameROptions(predefinedTypeNames);
        
        // Assert
        typeNameROptions.PredefinedTypeNames.Should().BeSameAs(predefinedTypeNames);
    }
#endif
}