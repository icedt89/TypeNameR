using JanHafner.TypeNameR.StackTrace;
using System.Diagnostics;

namespace JanHafner.TypeNameR;

/// <summary>
/// Defines control flags which are used during processing.
/// </summary>
[Flags]
public enum NameRControlFlags : uint
{
    /// <summary>
    /// As the name implies.
    /// </summary>
    None = 0,

    /// <summary>
    /// Includes the access modifier, <see langword="public"/> or <see langword="private"/>.
    /// </summary>
    IncludeAccessModifier = 1,

    /// <summary>
    /// Includes the <see langword="static"/> modifier.
    /// </summary>
    IncludeStaticModifier = 1 << 1,

    /// <summary>
    /// Includes the <see langword="async"/> modifier.
    /// </summary>
    IncludeAsyncModifier = 1 << 2,

    /// <summary>
    /// Includes the return parameter.
    /// </summary>
    IncludeReturnParameter = 1 << 3,

    /// <summary>
    /// Includes generic parameters.
    /// </summary>
    IncludeGenericParameters = 1 << 4,

    /// <summary>
    /// Includes parameter prefix, <see langword="ref"/>, <see langword="in"/>, <see langword="out"/>, <see langword="params"/> and <see langword="this"/>.
    /// </summary>
    IncludeParameterPrefix = 1 << 5,

    /// <summary>
    /// Includes the default value, <see langword="default"/> or a constant value.
    /// </summary>
    IncludeParameterDefaultValue = 1 << 6,

    /// <summary>
    /// Prepends the readable name of the <see cref="Type"/> containing the method.
    /// </summary>
    PrependFullTypeNameBeforeMethodName = 1 << 7,

    /// <summary>
    /// Include hidden stack frames.
    /// </summary>
    IncludeHiddenStackFrames = 1 << 8,

    /// <summary>
    /// Includes information about the source of the <see cref="StackFrame"/>, file name, line number and column.
    /// </summary>
    IncludeSourceInformation = 1 << 9,

    /// <summary>
    /// Include inner exception(s).
    /// </summary>
    IncludeInnerExceptions = 1 << 10,

    /// <summary>
    /// Stores the original <see cref="StackTrace"/> of the <see cref="Exception"/> in the <see cref="Exception.Data"/> property.
    /// </summary>
    StoreOriginalStackTraceInExceptionData = 1 << 11,

    /// <summary>
    /// Includes <see langword="params"/> keyword for arrays.
    /// </summary>
    IncludeParamsKeyword = 1 << 12,

    /// <summary>
    /// Includes <see langword="this"/> keyword for extension methods.
    /// </summary>
    IncludeThisKeyword = 1 << 13,

    /// <summary>
    /// Falls back to the <see cref="IStackFrameMetadataProvider"/> if source information is not present at the <see cref="StackFrame"/>.
    /// </summary>
    FallbackToStackFrameMetadataProvider = 1 << 14,

    /// <summary>
    /// Includes nullability info of type parameters (generics too), this will not affect display of nullable types (<see cref="Nullable{T}"/>).
    /// </summary>
    IncludeNullabilityInfo = 1 << 15,

    /// <summary>
    /// Includes <see langword="dynamic"/> keyword for dynamically typed parameters.
    /// </summary>
    /// <remarks>This will not output the real <see cref="Type"/> name of the parameter.</remarks>
    IncludeDynamic = 1 << 16,

    /// <summary>
    /// When set, recursive stack frames will not be eliminated
    /// </summary>
    DontEliminateRecursiveStackFrames = 1 << 17,

    /// <summary>
    /// Shortcut for all flags (excluding <see cref="IncludeDynamic"/>, <see cref="StoreOriginalStackTraceInExceptionData"/>, <see cref="DontEliminateRecursiveStackFrames"/> and <see cref="FallbackToStackFrameMetadataProvider"/>).
    /// </summary>
    All = IncludeAccessModifier
        | IncludeStaticModifier
        | IncludeAsyncModifier
        | IncludeReturnParameter
        | IncludeGenericParameters
        | IncludeParameterPrefix
        | IncludeParameterDefaultValue
        | PrependFullTypeNameBeforeMethodName
        | IncludeHiddenStackFrames
        | IncludeSourceInformation
        | IncludeInnerExceptions
        | IncludeParamsKeyword
        | IncludeThisKeyword
        | IncludeNullabilityInfo,
}