using Xunit;

namespace JanHafner.TypeNameR.Tests.StaticTypeNameRTests;

public sealed class Instance
{
    [Fact]
    public void ThrowsInvalidOperationExceptionIfTypeNameRWasNotSetUp()
    {
        // Act, Assert
        Assert.Throws<InvalidOperationException>(() => StaticTypeNameR.Instance);
    }
}