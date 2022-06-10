using System.IO.Abstractions;
using System.Reflection.PortableExecutable;

namespace JanHafner.TypeNameR.StackTrace;

public sealed class PdbLocator : IPdbLocator
{
    private readonly IFileSystem fileSystem;

    public PdbLocator(IFileSystem fileSystem)
    {
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public string? GetPdbLocation(string assemblyLocation)
    {
        if (!fileSystem.File.Exists(assemblyLocation))
        {
            return null;
        }

        using var assemblyFileStream = fileSystem.FileStream.Create(assemblyLocation, FileMode.Open, FileAccess.Read, FileShare.Read);

        using var peReader = new PEReader(assemblyFileStream);

        foreach (var debugDirectoryEntry in peReader.ReadDebugDirectory())
        {
            var pdbLocation = this.GetPdbLocationFromDebugDirectoryEntry(assemblyLocation, peReader, debugDirectoryEntry);
            if (pdbLocation is not null)
            {
                return pdbLocation;
            }
        }

        return null;
    }

    private string? GetPdbLocationFromDebugDirectoryEntry(ReadOnlySpan<char> assemblyLocation,
                                                          PEReader peReader,
                                                          DebugDirectoryEntry debugDirectoryEntry)
    {
        if (debugDirectoryEntry.Type != DebugDirectoryEntryType.CodeView)
        {
            return null;
        }

        var codeViewdebugDirectoryEntry = peReader.ReadCodeViewDebugDirectoryData(debugDirectoryEntry);

        var peDirectory = fileSystem.Path.GetDirectoryName(assemblyLocation);
        if (!peDirectory.IsEmpty)
        {
            return fileSystem.Path.Join(peDirectory, fileSystem.Path.GetFileName(codeViewdebugDirectoryEntry.Path));
        }

        return null;
    }
}
