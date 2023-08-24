using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using NSubstitute;
using System.Diagnostics;
using Xunit;

namespace JanHafner.TypeNameR.Tests.TypeHelperTests;

public sealed class GetExistingStackFrameMetadata
{
    [Fact]
    public void ReturnsNullIfFileNameIsNull()
    {
        // Arrange
        var stackFrameSubstitute = Substitute.For<StackFrame>();
        stackFrameSubstitute.GetFileName().Returns((string?)null);

        // Act
        var stackFrameMetadata = stackFrameSubstitute.GetExistingStackFrameMetadata();

        // Assert
        stackFrameMetadata.Should().BeNull();
    }

    [Fact]
    public void ReturnsNullIfFileNameHasZeroLength()
    {
        // Arrange
        var stackFrameSubstitute = Substitute.For<StackFrame>();
        stackFrameSubstitute.GetFileName().Returns(string.Empty);

        // Act
        var stackFrameMetadata = stackFrameSubstitute.GetExistingStackFrameMetadata();

        // Assert
        stackFrameMetadata.Should().BeNull();
    }

    [Fact]
    public void ReturnsTheStackFrameMetadata()
    {
        // Arrange
        const string expectedFileName = @"C:\dummy.pdb";
        const int expectedLineNumber = 13;
        const int expectedColumnNumber = 42;

        var stackFrameSubstitute = Substitute.For<StackFrame>();
        stackFrameSubstitute.GetFileName().Returns(expectedFileName);
        stackFrameSubstitute.GetFileLineNumber().Returns(expectedLineNumber);
        stackFrameSubstitute.GetFileColumnNumber().Returns(expectedColumnNumber);

        // Act
        var stackFrameMetadata = stackFrameSubstitute.GetExistingStackFrameMetadata();

        // Assert
        stackFrameMetadata.Should().NotBeNull();
        stackFrameMetadata!.Value.FileName.Should().Be(expectedFileName);
        stackFrameMetadata.Value.LineNumber.Should().Be(expectedLineNumber);
        stackFrameMetadata.Value.ColumnNumber.Should().Be(expectedColumnNumber);
    }
}