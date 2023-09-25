using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using Xunit;

namespace JanHafner.TypeNameR.Tests.ArrayHelperTests;

public sealed class ReducePrefixes
{
    [Fact]
    public void ReturnsEmptySpanIfArrayIsEmpty()
    {
        // Arrange
        var namespaces = Array.Empty<string>();
        
        // Act
        var reducesPrefixes = namespaces.ReducePrefixes();

        // Assert
        reducesPrefixes
            .ToArray()
            .Should()
            .BeEmpty();
    }
    
    [Fact]
    public void ReturnsSpanWithOneItemIfArrayContainsOnlyOne()
    {
        // Arrange
        var namespaces = new[]
        {
            "Microsoft",
        };
        
        // Act
        var reducesPrefixes = namespaces.ReducePrefixes();

        // Assert
        reducesPrefixes
            .ToArray()
            .Should()
            .HaveCount(1)
            .And
            .Contain("Microsoft");
    }
    
    [Fact]
    public void ReducesPrefixes()
    {
        // Arrange
        var namespaces = new[]
        {
            "Microsoft",
            "Microsoft.EntityFrameworkCore",
            "System",
            "System",
            "System.Collections",
            "System.Runtime",
            "System.Runtime.Caching",
        };
        
        // Act
        var reducesPrefixes = namespaces.ReducePrefixes();

        // Assert
        reducesPrefixes
            .ToArray()
            .Should()
            .HaveCount(2)
            .And
            .ContainInOrder("Microsoft", "System");
    }
}