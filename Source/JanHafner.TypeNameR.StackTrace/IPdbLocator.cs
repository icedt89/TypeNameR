using System.Reflection;

namespace JanHafner.TypeNameR.StackTrace;

/// <summary>
/// Defines methods for resolving the location of a pdb-file.
/// </summary>
public interface IPdbLocator
{
    /// <summary>
    /// Tries to locate and open the pdb-file for the supplied assembly location.
    /// </summary>
    /// <param name="assemblyLocation">The absolute path to the <see cref="Assembly"/> file. if the file does not exist, the method returns <see langword="null"/>.</param>
    /// <returns>The <see cref="Stream"/> of the located pdb-file or <see langword="null"/>.</returns>
    Stream? OpenLocatedPdb(string assemblyLocation);
}