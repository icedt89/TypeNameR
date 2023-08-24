using FluentAssertions;
using NSubstitute;
using Xunit;

namespace JanHafner.TypeNameR.Tests.StaticTypeNameRTests;

public sealed class SetDefaultTypeNameR
{
    [Fact]
    public void ThrowsArgumentNullExceptionIfTypeNameRIsNull()
    {
        // Arrange
        TypeNameR typeNameR = null!;

        // Act, Assert
        var argumentNullException = Assert.Throws<ArgumentNullException>(() => StaticTypeNameR.SetDefaultTypeNameR(typeNameR));
        argumentNullException.ParamName.Should().Be(nameof(typeNameR));
    }

    [Fact]
    public void SetsTheTypeNameRAndReturnsIt()
    {
        // Arrange
        var typeNameRSubstitute = Substitute.For<ITypeNameR>();

        // Act
        StaticTypeNameR.SetDefaultTypeNameR(typeNameRSubstitute);

        // Assert
        StaticTypeNameR.Instance.Should().BeSameAs(typeNameRSubstitute);
    }
}