using System.Diagnostics;

namespace JanHafner.TypeNameR;

/// <summary>
/// Defines control flags which are used during processing.
/// </summary>
[Flags]
public enum NameRControlFlags
{
    /// <summary>
    /// As the name implies.
    /// </summary>
    None = 0,

    /// <summary>
    /// Includes the access modifier, eg <see langword="public"/> or <see langword="private"/>
    /// </summary>
    IncludeAccessModifier = 1,

    /// <summary>
    /// Includes the <see langword="static"/> modifier.
    /// </summary>
    IncludeStaticModifier = 2,

    /// <summary>
    /// Includes the <see langword="async"/> modifier.
    /// </summary>
    IncludeAsyncModifier = 4,

    /// <summary>
    /// Includes the return parameter.
    /// </summary>
    IncludeReturnParameter = 8,

    /// <summary>
    /// Includes generic parameters.
    /// </summary>
    IncludeGenericParameters = 16,

    /// <summary>
    /// Includes parameter modifier, eg <see langword="ref"/>, <see langword="in"/>, <see langword="out"/> or <see langword="this"/> (for extension methods).
    /// </summary>
    IncludeParameterModifiers = 32,

    /// <summary>
    /// Includes the default value, eg <see langword="default"/> or a constant value.
    /// </summary>
    IncludeParameterDefaultValue = 64,

    /// <summary>
    /// Prepends the readable name of the <see cref="Type"/> containing the method.
    /// </summary>
    IncludeFullTypeName = 128,

    /// <summary>
    /// Include hidden stackframes.
    /// </summary>
    IncludeHiddenStackFrames = 256,

    /// <summary>
    /// Includes information about the source of the <see cref="StackFrame"/> eg. file, line number and column.
    /// </summary>
    IncludeSourceInformation = 512,

    /// <summary>
    /// Include inner exception(s).
    /// </summary>
    IncludeInnerExceptions = 1024,

    /// <summary>
    /// Stores the original <see cref="System.Diagnostics.StackTrace"/> of the <see cref="Exception"/> in the <see cref="Exception.Data"/> property.
    /// </summary>
    StoreOriginalStackTraceInExceptionData = 2048,

    /// <summary>
    /// Include the <see langword="this"/> keyword to mark the <see langword="this"/> parameter of an extension method.
    /// </summary>
    IncludeThisKeywordForExtensions = 4096,

    /// <summary>
    /// Shortcut for all flags.
    /// </summary>
    All = IncludeAccessModifier
        | IncludeStaticModifier
        | IncludeAsyncModifier
        | IncludeReturnParameter
        | IncludeGenericParameters
        | IncludeParameterModifiers
        | IncludeParameterDefaultValue
        | IncludeFullTypeName
        | IncludeHiddenStackFrames
        | IncludeSourceInformation
        | IncludeInnerExceptions
        | StoreOriginalStackTraceInExceptionData
        | IncludeThisKeywordForExtensions,
}