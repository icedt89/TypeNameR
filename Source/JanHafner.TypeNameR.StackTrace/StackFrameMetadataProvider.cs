using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace JanHafner.TypeNameR.StackTrace;

public sealed class StackFrameMetadataProvider : IStackFrameMetadataProvider
{
    private readonly IPdbLocator pdbLocator;
    
    public StackFrameMetadataProvider(IPdbLocator pdbLocator)
    {
        this.pdbLocator = pdbLocator ?? throw new ArgumentNullException(nameof(pdbLocator));
    }

    public StackFrameMetadata? ProvideStackFrameMetadata(StackFrame stackFrame, MethodBase method)
    {
        var ilOffset = stackFrame.GetILOffset();
        if (ilOffset == StackFrame.OFFSET_UNKNOWN)
        {
            return null;
        }

        var methodBaseMetadataTokenHandle = MetadataTokens.Handle(method.GetMetadataToken());
        if (methodBaseMetadataTokenHandle.Kind != HandleKind.MethodDefinition || methodBaseMetadataTokenHandle.IsNil)
        {
            return null;
        }

        var debugInformationHandle = ((MethodDefinitionHandle)methodBaseMetadataTokenHandle).ToDebugInformationHandle();
        if (debugInformationHandle.IsNil)
        {
            return null;
        }
        
        var pdbStream = pdbLocator.OpenLocatedPdb(method.Module.Assembly.Location);
        if (pdbStream is null)
        {
            return null;
        }

        // According to benchmarks...
        using var metadataReaderProvider = MetadataReaderProvider.FromPortablePdbStream(pdbStream,
#if NET6_0
            // ...in .net 6 it reduced memory allocations
            MetadataStreamOptions.Default
#elif NET7_0_OR_GREATER
            // ...but in .net 7/8 memory allocation and speed was reduced
            MetadataStreamOptions.PrefetchMetadata
#endif
            );

        var metadataReader = metadataReaderProvider.GetMetadataReader();

        var methodDebugInformation = metadataReader.GetMethodDebugInformation(debugInformationHandle);
        var sequencePoints = methodDebugInformation.GetSequencePoints().ToArray();
        if (sequencePoints.Length == 0)
        {
            return null;
        }

        var bestSequencePoint = FindBestSequencePoint(ilOffset, sequencePoints);
        if (!bestSequencePoint.HasValue)
        {
            return null;
        }

        return CreateStackFrameMetadata(metadataReader, bestSequencePoint.Value);
    }

    private static StackFrameMetadata? CreateStackFrameMetadata(MetadataReader metadataReader, SequencePoint sequencePoint)
    {
        var document = metadataReader.GetDocument(sequencePoint.Document);
        var fileName = metadataReader.GetString(document.Name);
        if (fileName.Length == 0)
        {
            return null;
        }

        var lineNumber = sequencePoint.StartLine;
        var columnNumber = sequencePoint.StartColumn;

        return new StackFrameMetadata(fileName, lineNumber, columnNumber);
    }

    private static SequencePoint? FindBestSequencePoint(int ilOffset, ReadOnlySpan<SequencePoint> sequencePoints)
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
