using System.Diagnostics;
using System.IO.Abstractions;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace JanHafner.TypeNameR.StackTrace;

public sealed class StackFrameMetadataProvider : IStackFrameMetadataProvider
{
    private readonly IPdbLocator pdbLocator;

    private readonly IFileSystem fileSystem;

    public StackFrameMetadataProvider(IPdbLocator pdbLocator, IFileSystem fileSystem)
    {
        this.pdbLocator = pdbLocator ?? throw new ArgumentNullException(nameof(pdbLocator));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public StackFrameMetadata? GetStackFrameMetadata(StackFrame stackFrame)
    {
        if (stackFrame is null)
        {
            throw new ArgumentNullException(nameof(stackFrame));
        }

        var methodBase = stackFrame.GetMethod();
        if (methodBase is null)
        {
            return null;
        }

        var pdbLocation = pdbLocator.GetPdbLocation(methodBase.Module.Assembly.Location);
        if (pdbLocation is null)
        {
            return null;
        }

        if (!fileSystem.File.Exists(pdbLocation))
        {
            return null;
        }

        using var pdbFileStream = fileSystem.FileStream.Create(pdbLocation, FileMode.Open, FileAccess.Read, FileShare.Read);

        using var metadataReaderProvider = MetadataReaderProvider.FromPortablePdbStream(pdbFileStream);

        var metadataReader = metadataReaderProvider.GetMetadataReader();
        if (metadataReader is null)
        {
            return null;
        }

        var methodBaseMetadataTokenHandle = MetadataTokens.Handle(methodBase.MetadataToken);
        if (methodBaseMetadataTokenHandle.IsNil || methodBaseMetadataTokenHandle.Kind != HandleKind.MethodDefinition)
        {
            return null;
        }

        var debugInformationHandle = ((MethodDefinitionHandle)methodBaseMetadataTokenHandle).ToDebugInformationHandle();
        if (debugInformationHandle.IsNil)
        {
            return null;
        }

        var methodDebugInformation = metadataReader.GetMethodDebugInformation(debugInformationHandle);
        var sequencePoints = methodDebugInformation.GetSequencePoints().ToArray();
        if (sequencePoints.Length == 0)
        {
            return null;
        }

        var ilOffset = stackFrame.GetILOffset();
        var bestSequencePoint = FindBestSequencePoint(ilOffset, sequencePoints);
        if (!bestSequencePoint.HasValue)
        {
            return null;
        }

        return CreateStackFrameMetadata(metadataReader, bestSequencePoint.Value);
    }

    private static StackFrameMetadata CreateStackFrameMetadata(MetadataReader metadataReader, SequencePoint sequencePoint)
    {
        var document = metadataReader.GetDocument(sequencePoint.Document);
        var fileName = metadataReader.GetString(document.Name);

        var lineNumber = sequencePoint.StartLine;
        var columnNumber = sequencePoint.StartColumn;

        return new StackFrameMetadata(fileName, lineNumber, columnNumber);
    }

    private static SequencePoint? FindBestSequencePoint(int ilOffset, SequencePoint[] sequencePoints)
    {
        SequencePoint? bestSequencePoint = null;
        foreach (var sequencePoint in sequencePoints)
        {
            if (sequencePoint.Offset > ilOffset)
            {
                break;
            }

            if (!sequencePoint.IsHidden)
            {
                bestSequencePoint = sequencePoint;
            }
        }

        return bestSequencePoint;
    }
}
