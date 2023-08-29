﻿using Xunit;

namespace JanHafner.TypeNameR.Tests.StaticTypeNameRTests;

[Collection("Isolated")]
public sealed class Instance
{
    [Fact]
    public void ThrowsInvalidOperationExceptionIfTypeNameRWasNotSetUp()
    {
        // Act, Assert
        Assert.Throws<InvalidOperationException>(() => StaticTypeNameR.Instance);
    }
}