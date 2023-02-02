using System.Diagnostics;
using System.Reflection;

namespace JanHafner.TypeNameR;

public interface ITypeNameR
{
    /// <summary>
    /// Extracts the readable form of the supplied <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> from which to extract the readable name.</param>
    /// <param name="fullTypename">Controls whether the full <see cref="Type"/> name should be used.</param>
    /// <returns>The readable name of the <see cref="Type"/> supplied via <paramref name="type"/>.</returns>
    string ExtractReadable(Type type, bool fullTypename);

    /// <summary>
    /// Extracts the readable form of the supplied <see cref="MethodInfo"/>.
    /// </summary>
    /// <param name="methodBase">The <see cref="MethodBase"/> from which to extract the readable name.</param>
    /// <param name="nameRControlFlags">Flags to customize the process.</param>
    /// <returns>The readable name of the <see cref="MethodInfo"/> supplied via <paramref name="methodBase"/>.</returns>
    string ExtractReadable(MethodBase methodBase, NameRControlFlags nameRControlFlags);

    /// <summary>
    /// Extracts the readable form of the supplied <see cref="ParameterInfo"/>.
    /// </summary>
    /// <param name="parameterInfo">The <see cref="ParameterInfo"/> from which to extract the readable name.</param>
    /// <param name="nameRControlFlags">Flags to customize the process.</param>
    /// <returns>The readable name of the <see cref="ParameterInfo"/> supplied via <paramref name="parameterInfo"/>.</returns>
    string ExtractReadable(ParameterInfo parameterInfo, NameRControlFlags nameRControlFlags);

    /// <summary>
    /// Extracts the readable form of the supplied <see cref="StackFrame"/>.
    /// </summary>
    /// <param name="stackFrame">The <see cref="StackFrame"/> from which to extract the readable name.</param>
    /// <param name="nameRControlFlags">Flags to customize the process.</param>
    /// <returns>The readable name of the <see cref="StackFrame"/> supplied via <paramref name="stackFrame"/>.</returns>
    string ExtractReadable(StackFrame stackFrame, NameRControlFlags nameRControlFlags);

    /// <summary>
    /// Extract the readable form of the supplied <see cref="System.Diagnostics.StackTrace"/>.
    /// </summary>
    /// <param name="stackTrace">The <see cref="System.Diagnostics.StackTrace"/> from which to extract the readable form.</param>
    /// <param name="nameRControlFlags">Flags to customize the process.</param>
    /// <returns>The readable form of the <see cref="System.Diagnostics.StackTrace"/> supplied via <paramref name="stackTrace"/>.</returns>
    string ExtractReadable(System.Diagnostics.StackTrace stackTrace, NameRControlFlags nameRControlFlags);

    /// <summary>
    /// Extract the readable form of <see cref="System.Diagnostics.StackTrace"/> the supplied <see cref="Exception"/>.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/> from which to extract the readable form.</param>
    /// <param name="nameRControlFlags">Flags to customize the process.</param>
    /// <returns>The readable form of the <see cref="Exception"/> supplied via <paramref name="exception"/>.</returns>
    string ExtractReadableStackTrace(Exception exception, NameRControlFlags nameRControlFlags);

    /// <summary>
    /// Extracts and rewrites the readable stacktrace of the supplied <see cref="Exception"/>.
    /// </summary>
    /// <typeparam name="TException">The <see cref="Exception"/> type.</typeparam>
    /// <param name="exception">The <see cref="Exception"/>.</param>
    /// <param name="nameRControlFlags">Flags to customize the process.</param>
    /// <returns>The same <see cref="Exception"/> as supplied, but with the readable stacktrace.</returns>
    TException RewriteStackTrace<TException>(TException exception, NameRControlFlags nameRControlFlags)
        where TException : Exception;
}