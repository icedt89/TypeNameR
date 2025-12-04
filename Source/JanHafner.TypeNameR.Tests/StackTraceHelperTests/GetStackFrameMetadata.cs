using AwesomeAssertions;
using JanHafner.TypeNameR.Helper;
using NSubstitute;
using System.Diagnostics;
using Xunit;

namespace JanHafner.TypeNameR.Tests.StackTraceHelperTests;

public sealed class GetStackFrameMetadata
{
    [Fact]
    public void ReturnsEmptyStackFrameMetadataIfFileNameIsNull()
    {
        // Arrange
        var stackFrameSubstitute = Substitute.For<StackFrame>();
        stackFrameSubstitute.GetFileName().Returns((string?)null);

        // Act
        var stackFrameMetadata = stackFrameSubstitute.GetStackFrameMetadata();

        // Assert
        stackFrameMetadata.IsEmpty.Should().BeTrue();
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
        var stackFrameMetadata = stackFrameSubstitute.GetStackFrameMetadata();

        // Assert
        stackFrameMetadata.FileName.ToString().Should().Be(expectedFileName);
        stackFrameMetadata.LineNumber.Should().Be(expectedLineNumber);
        stackFrameMetadata.ColumnNumber.Should().Be(expectedColumnNumber);
    }
}