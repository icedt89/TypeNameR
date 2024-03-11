using FluentAssertions;
using JanHafner.TypeNameR.Helper;
using JanHafner.TypeNameR.StackTrace;
using System.Text;
using Xunit;

namespace JanHafner.TypeNameR.Tests.AppendHelperTests;

public sealed class AppendStackFrameMetadata
{
    [Fact]
    public void AppendsSourceFileName()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        var stackFrameMetadata = new StackFrameMetadata(Guid.NewGuid().ToString(), 0, 0);

        // Act
        stringBuilder.AppendStackFrameMetadata(stackFrameMetadata);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.InSourceWithLeadingAndEndingSpace}{stackFrameMetadata.FileName}");
    }

    [Fact]
    public void AppendsSourceFileNameAndLineNumber()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        var stackFrameMetadata = new StackFrameMetadata(Guid.NewGuid().ToString(), 1, 0);

        // Act
        stringBuilder.AppendStackFrameMetadata(stackFrameMetadata);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.InSourceWithLeadingAndEndingSpace}{stackFrameMetadata.FileName}{Constants.LineWithEndingSpace}{stackFrameMetadata.LineNumber}");
    }

    [Fact]
    public void AppendsSourceFileNameAndLineNumberAndColumnNumber()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        var stackFrameMetadata = new StackFrameMetadata(Guid.NewGuid().ToString(), 1, 1);

        // Act
        stringBuilder.AppendStackFrameMetadata(stackFrameMetadata);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.InSourceWithLeadingAndEndingSpace}{stackFrameMetadata.FileName}{Constants.LineWithEndingSpace}{stackFrameMetadata.LineNumber}{Constants.Colon}{stackFrameMetadata.ColumnNumber}");
    }

    [Fact]
    public void AppendsColumnNumberOnlyIfLineNumberIsSet()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        var stackFrameMetadata = new StackFrameMetadata(Guid.NewGuid().ToString(), 0, 1);

        // Act
        stringBuilder.AppendStackFrameMetadata(stackFrameMetadata);

        // Assert
        stringBuilder.ToString().Should().Be($"{Constants.InSourceWithLeadingAndEndingSpace}{stackFrameMetadata.FileName}");
    }
}