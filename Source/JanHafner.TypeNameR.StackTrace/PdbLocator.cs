using System.IO.Abstractions;
using System.Reflection.PortableExecutable;

namespace JanHafner.TypeNameR.StackTrace;

/// <inheritdoc />
public sealed class PdbLocator : IPdbLocator
{
    private readonly IFileSystem fileSystem;

    /// <summary>
    /// Initializes a new instance of the <see cref="PdbLocator"/> class.
    /// </summary>
    /// <param name="fileSystem">Implementation of <see cref="IFileSystem"/> that is used to interact with the file system.</param>
    public PdbLocator(IFileSystem fileSystem)
    {
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    /// <inheritdoc />
    public Stream? OpenLocatedPdb(string assemblyLocation)
    {
        if (!fileSystem.File.Exists(assemblyLocation))
        {
            return null;
        }

        return OpenFromProbedLocation(assemblyLocation) ?? OpenFromPortableExecutable(assemblyLocation);
    }

    private FileSystemStream? OpenFromPortableExecutable(string assemblyLocation)
    {
        using var assemblyFileStream = fileSystem.FileStream.New(assemblyLocation, FileMode.Open, FileAccess.Read, FileShare.Read);

        using var peReader = new PEReader(assemblyFileStream);

        var debugDirectory = peReader.ReadDebugDirectory();
        foreach (var debugDirectoryEntry in debugDirectory)
        {
            if (debugDirectoryEntry.Type != DebugDirectoryEntryType.CodeView)
            {
                continue;
            }

            var pdbStream = CreateFromCodeViewDebugDirectoryData(assemblyLocation, peReader, debugDirectoryEntry);
            if (pdbStream is not null)
            {
                return pdbStream;
            }
        }

        return null;
    }

    private FileSystemStream? CreateFromCodeViewDebugDirectoryData(ReadOnlySpan<char> assemblyLocation, PEReader peReader, DebugDirectoryEntry debugDirectoryEntry)
    {
        var codeViewDebugDirectoryEntry = peReader.ReadCodeViewDebugDirectoryData(debugDirectoryEntry);

        var pdbStream = OpenIfExists(codeViewDebugDirectoryEntry.Path);
        if (pdbStream is not null)
        {
            return pdbStream;
        }

        var peDirectory = fileSystem.Path.GetDirectoryName(assemblyLocation);
        var pdbFileName = fileSystem.Path.GetFileName(codeViewDebugDirectoryEntry.Path);

        var pdbFilePath = fileSystem.Path.Join(peDirectory, pdbFileName);

        return OpenIfExists(pdbFilePath);
    }

    private FileSystemStream? OpenFromProbedLocation(string assemblyLocation)
    {
        var assemblyLocationSpan = assemblyLocation.AsSpan();
        var assemblyLocationExtension = fileSystem.Path.GetExtension(assemblyLocationSpan);
        var startOfExtension = assemblyLocationSpan.LastIndexOf(assemblyLocationExtension);
        if (startOfExtension > 0)
        {
            var assemblyLocationWithoutExtension = assemblyLocationSpan[..startOfExtension];
            var assemblyLocationWithPdbExtension = string.Concat(assemblyLocationWithoutExtension, ".pdb");

            return OpenIfExists(assemblyLocationWithPdbExtension);
        }

        return null;
    }

    private FileSystemStream? OpenIfExists(string pdbLocation)
    {
        if (fileSystem.File.Exists(pdbLocation))
        {
            return fileSystem.FileStream.New(pdbLocation, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        return null;
    }
}