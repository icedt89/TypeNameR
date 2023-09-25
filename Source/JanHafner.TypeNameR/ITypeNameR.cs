using System.Diagnostics;
using System.Reflection;

#if NET6_0
using NullabilityInfo = Nullability.NullabilityInfoEx;
#endif

namespace JanHafner.TypeNameR;

/// <summary>
/// Provides methods for generating display names of <see cref="Type"/>, <see cref="MethodBase"/> and so on.
/// </summary>
public interface ITypeNameR
{
    /// <summary>
    /// Generates the readable display of the supplied <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/>.</param>
    /// <param name="fullTypeName">Controls whether the full <see cref="Type"/> name should be used.</param>
    /// <param name="nullabilityInfo">The <see cref="NullabilityInfo" /> used to determine nullability for reference types.</param>
    /// <returns>The readable display of the supplied <see cref="Type"/>.</returns>
    string GenerateDisplay(Type type, bool fullTypeName, NullabilityInfo? nullabilityInfo);

    /// <summary>
    /// Generates the readable display of the supplied <see cref="MethodBase"/>.
    /// </summary>
    /// <param name="methodBase">The <see cref="MethodBase"/>.</param>
    /// <param name="nameRControlFlags">Flags to customize the process.</param>
    /// <returns>The readable display of the supplied <see cref="MethodBase"/>.</returns>
    string GenerateDisplay(MethodBase methodBase, NameRControlFlags nameRControlFlags);

    /// <summary>
    /// Generates the readable display of the supplied <see cref="ParameterInfo"/>.
    /// </summary>
    /// <param name="parameterInfo">The <see cref="ParameterInfo"/>.</param>
    /// <param name="nameRControlFlags">Flags to customize the process.</param>
    /// <returns>The readable display of the supplied <see cref="ParameterInfo"/>.</returns>
    string GenerateDisplay(ParameterInfo parameterInfo, NameRControlFlags nameRControlFlags);

    /// <summary>
    /// Generates the readable display of the supplied <see cref="StackFrame"/>.
    /// </summary>
    /// <param name="stackFrame">The <see cref="StackFrame"/>.</param>
    /// <param name="nameRControlFlags">Flags to customize the process.</param>
    /// <returns>The readable display of the <see cref="StackFrame"/>.</returns>
    string GenerateDisplay(StackFrame stackFrame, NameRControlFlags nameRControlFlags);

    /// <summary>
    /// Generates the readable display of the supplied <see cref="StackTrace"/>.
    /// </summary>
    /// <param name="stackTrace">The <see cref="StackTrace"/>.</param>
    /// <param name="nameRControlFlags">Flags to customize the process.</param>
    /// <returns>The readable display of the <see cref="StackTrace"/>.</returns>
    string GenerateDisplay(System.Diagnostics.StackTrace stackTrace, NameRControlFlags nameRControlFlags);

    /// <summary>
    /// Generates the readable display of the supplied <see cref="Exception"/>.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/>.</param>
    /// <param name="nameRControlFlags">Flags to customize the process.</param>
    /// <returns>The readable display of the <see cref="Exception"/>.</returns>
    string GenerateDisplay(Exception exception, NameRControlFlags nameRControlFlags);

    /// <summary>
    /// Generates and rewrites the stacktrace of the supplied <see cref="Exception"/>.
    /// </summary>
    /// <typeparam name="TException">The <see cref="Exception"/> type.</typeparam>
    /// <param name="exception">The <see cref="Exception"/>.</param>
    /// <param name="nameRControlFlags">Flags to customize the process.</param>
    /// <returns>The same <see cref="Exception"/> as supplied, but with the readable stacktrace.</returns>
    TException RewriteStackTrace<TException>(TException exception, NameRControlFlags nameRControlFlags)
        where TException : Exception;
}