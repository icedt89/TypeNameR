using System.Reflection;

namespace JanHafner.TypeNameR.StackTrace;

/// <summary>
/// Defines methods for resolving the location of a .pdb-file.
/// </summary>
public interface IPdbLocator
{
    /// <summary>
    /// Tries to locate the .pdb-file for the supplied assembly location.
    /// </summary>
    /// <param name="assemblyLocation">The absolute path to the <see cref="Assembly"/> file.</param>
    /// <returns>The location of the .pdb-file or <see langword="null"/>.</returns>
    string? GetPdbLocation(string assemblyLocation);
}
