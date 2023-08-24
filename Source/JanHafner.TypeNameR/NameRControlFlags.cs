using JanHafner.TypeNameR.StackTrace;
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
    /// Includes the access modifier,  <see langword="public"/> or <see langword="private"/>.
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
    /// Includes parameter prefix, <see langword="ref"/>, <see langword="in"/>, <see langword="out"/>, <see langword="params"/> and <see langword="this"/>.
    /// </summary>
    IncludeParameterPrefix = 32,

    /// <summary>
    /// Includes the default value, <see langword="default"/> or a constant value.
    /// </summary>
    IncludeParameterDefaultValue = 64,

    /// <summary>
    /// Prepends the readable name of the <see cref="Type"/> containing the method.
    /// </summary>
    PrependFullTypeName = 128,

    /// <summary>
    /// Include hidden stack frames.
    /// </summary>
    IncludeHiddenStackFrames = 256,

    /// <summary>
    /// Includes information about the source of the <see cref="StackFrame"/>, file name, line number and column.
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
    /// Exclude namespaces of declaring methods of stack frames starting with <see cref="TypeNameROptions.ExcludedNamespaces"/>.
    /// </summary>
    ExcludeStackFrameMethodsByNamespace = 8192,
    
    /// <summary>
    /// Includes <see langword="params"/> keyword for arrays.
    /// </summary>
    IncludeParamsKeyword = 16384,
    
    /// <summary>
    /// Includes <see langword="this"/> keyword for extension methods.
    /// </summary>
    IncludeThisKeyword = 32768,
    
    /// <summary>
    /// Falls back to the <see cref="IStackFrameMetadataProvider"/> if source information is not present at the <see cref="StackFrame"/>.
    /// </summary>
    FallbackToStackFrameMetadataProvider = 65536,
    
    /// <summary>
    /// Includes nullability info of type parameters (generics too), this will not affect display of nullable types (<see cref="Nullable{T}"/>).
    /// </summary>
    IncludeNullabilityInfo = 131072,
    
    /// <summary>
    /// Shortcut for all flags.
    /// </summary>
    All = IncludeAccessModifier
        | IncludeStaticModifier
        | IncludeAsyncModifier
        | IncludeReturnParameter
        | IncludeGenericParameters
        | IncludeParameterPrefix
        | IncludeParameterDefaultValue
        | PrependFullTypeName
        | IncludeHiddenStackFrames
        | IncludeSourceInformation
        | IncludeInnerExceptions
        | StoreOriginalStackTraceInExceptionData
        | ExcludeStackFrameMethodsByNamespace
        | IncludeParamsKeyword
        | IncludeThisKeyword
        | FallbackToStackFrameMetadataProvider
        | IncludeNullabilityInfo,
}