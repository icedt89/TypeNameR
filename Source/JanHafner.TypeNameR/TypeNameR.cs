using JanHafner.TypeNameR.Exceptions;
using JanHafner.TypeNameR.Helper;
using JanHafner.TypeNameR.StackTrace;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

#if NET6_0
using NullabilityInfoContext = Nullability.NullabilityInfoContextEx;
using NullabilityInfo = Nullability.NullabilityInfoEx;
using NullabilityState = Nullability.NullabilityStateEx;
#endif

namespace JanHafner.TypeNameR;

/// <inheritdoc />
public sealed class TypeNameR : ITypeNameR
{
    private readonly IStackFrameMetadataProvider? stackFrameMetadataProvider;

    private readonly TypeNameROptions typeNameROptions;

    // TODO: Don`t store as field because it uses an internal cache which never gets cleared => put into method arguments where needed so gets used only during generation
    private readonly NullabilityInfoContext nullabilityInfoContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameR"/> class with the supplied <see cref="TypeNameROptions"/>.
    /// </summary>
    /// <param name="stackFrameMetadataProvider">Implementation that is used to retrieve <see cref="StackFrameMetadata"/> for <see cref="StackFrame"/>.</param>
    /// <param name="typeNameROptions">If set to <see langword="null"/>, a new instance initialized with the default values will be used internally.</param>
    public TypeNameR(IStackFrameMetadataProvider? stackFrameMetadataProvider = null,
                     TypeNameROptions? typeNameROptions = null)
    {
        this.stackFrameMetadataProvider = stackFrameMetadataProvider;
        this.typeNameROptions = typeNameROptions ?? new TypeNameROptions();
        nullabilityInfoContext = new NullabilityInfoContext();
    }

    #region Type specifics

    /// <inheritdoc />
    public string GenerateDisplay(Type type, bool fullTypeName, NullabilityInfo? nullabilityInfo)
    {
        ArgumentNullException.ThrowIfNull(type);

        var stringBuilder = new StringBuilder();

        ProcessTypeCore(stringBuilder, type, fullTypeName, nullabilityInfo, null);
        
        return stringBuilder.ToString();
    }

    private void ProcessTypeCore(StringBuilder stringBuilder, Type type, bool fullTypeName, NullabilityInfo? nullabilityInfo, Type[]? masterGenericTypes)
    {
        var skipTypeAndGenericsAndNullable = false;
        if (typeNameROptions.PredefinedTypeNames.TryGetValue(type, out var predefinedTypeName))
        {
            stringBuilder.Append(predefinedTypeName);

            skipTypeAndGenericsAndNullable = true;
        }
        else
        {
            var elementType = type.GetElementType();
            if (elementType is not null)
            {
                var isArray = type.IsArray;
                
                ProcessTypeCore(stringBuilder, elementType, fullTypeName, isArray ? nullabilityInfo?.ElementType : nullabilityInfo, null);
                
                if (isArray)
                {
                    stringBuilder.AppendArrayRank(type.GetArrayRank());
                }
                else if(type.IsPointer)
                {
                    stringBuilder.AppendPointerMarker();
                }
                
                skipTypeAndGenericsAndNullable = true;
            }
        }

        var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
        if (!skipTypeAndGenericsAndNullable && nullableUnderlyingType is not null)
        {
            ProcessTypeCore(stringBuilder, nullableUnderlyingType, fullTypeName, null, null);
            
            stringBuilder.AppendNullableMarker();

            return;
        }

        if (!skipTypeAndGenericsAndNullable)
        {
            var processGenerics = DetermineGenericsProcessingInfo(type, ref masterGenericTypes, out var startGenericParameterIndex, out var genericParametersCount);

            // Nested
            if (type.DeclaringType is not null && !type.IsGenericParameter)
            {
                ProcessTypeCore(stringBuilder, type.DeclaringType, true, null, masterGenericTypes);

                stringBuilder.AppendPlus();
            }
            
            // Full type name
            if (fullTypeName && type.DeclaringType is null && type.Namespace is not null)
            {
                stringBuilder.AppendNamespace(type.Namespace);
            }

            var typeNameSpan = type.Name.AsSpan();
            if (type.IsGenericType)
            {
                typeNameSpan = typeNameSpan.RemoveGenericParametersCount();
            }
            
            stringBuilder.Append(typeNameSpan);

            if (processGenerics && masterGenericTypes is not null)
            {
                ProcessGenerics(stringBuilder, masterGenericTypes, nullabilityInfo?.GenericTypeArguments, startGenericParameterIndex, genericParametersCount);
            }
        }
        
        if (nullableUnderlyingType is not null || nullabilityInfo?.ReadState != NullabilityState.Nullable)
        {
            return;
        }

        var compareType = nullabilityInfo.Type;
        if (nullabilityInfo.Type.IsByRef || nullabilityInfo.Type.IsPointer)
        {
            compareType = nullabilityInfo.Type.GetElementType();
        }

        if (type == compareType)
        {
            stringBuilder.AppendNullableMarker();
        }
    }
    
    private bool DetermineGenericsProcessingInfo(Type type, ref Type[]? masterGenericTypes, out int startGenericParameterIndex, out int genericParametersCount)
    {
        // Ignore type if it is not generic (has no inherited generic types and not defined any directly)
        if (!type.IsGenericType)
        {
            startGenericParameterIndex = 0;
            genericParametersCount = 0;

            return false;
        }

        var typeGenericArguments = type.GetGenericArguments(); 

        // Get generic arguments if we have them not yet resolved.
        // This parameter serves as the "master"-lookup for nested types, so we resolve it just once for the most inner type and reuse it for the nested types.
        // This is necessary because the "real"-types can not be resolved from the declaring type.
        masterGenericTypes ??= typeGenericArguments;
        
        startGenericParameterIndex = 0;
        genericParametersCount = typeGenericArguments.Length;

        // If the type is nested and the declaring type is generic, we need to strip out the inherited generic arguments
        if (type.DeclaringType is not null && type.DeclaringType.IsGenericType)
        {
            startGenericParameterIndex = type.DeclaringType.GetGenericArguments().Length;
        }

        return startGenericParameterIndex < genericParametersCount;
    }

    private void ProcessGenerics(StringBuilder stringBuilder, Type[] genericTypes, NullabilityInfo[]? genericTypesNullability, int startGenericParameterIndex, int genericParametersCount)
    {
        stringBuilder.AppendLessThanSign();

        for (var genericParameterIndex = startGenericParameterIndex; genericParameterIndex < genericParametersCount; genericParameterIndex++)
        {
            NullabilityInfo? genericTypeNullabilityInfo = null;
            if (genericTypesNullability is not null && genericParameterIndex < genericTypesNullability.Length)
            {
                genericTypeNullabilityInfo = genericTypesNullability[genericParameterIndex];
            }

            ProcessTypeCore(stringBuilder, genericTypes[genericParameterIndex], false, genericTypeNullabilityInfo, null);

            if (genericParameterIndex < genericParametersCount - 1)
            {
                stringBuilder.AppendCommaWithEndingSpace();
            }
        }

        stringBuilder.AppendGreaterThanSign();
    }

    #endregion

    #region Method specific

    /// <inheritdoc />
    public string GenerateDisplay(MethodBase methodBase, NameRControlFlags nameRControlFlags)
    {
        ArgumentNullException.ThrowIfNull(methodBase);

        var stringBuilder = new StringBuilder();

        ProcessMethod(stringBuilder, methodBase, nameRControlFlags, out _);

        return stringBuilder.ToString();
    }

    /// <inheritdoc />
    public string GenerateDisplay(ParameterInfo parameterInfo, NameRControlFlags nameRControlFlags)
    {
        ArgumentNullException.ThrowIfNull(parameterInfo);

        var stringBuilder = new StringBuilder();

        ProcessParameter(stringBuilder, parameterInfo, nameRControlFlags);

        return stringBuilder.ToString();
    }

    private void ProcessMethod(StringBuilder stringBuilder, MethodBase methodBase, NameRControlFlags nameRControlFlags, out StateMachineType stateMachineType)
    {
        switch (methodBase)
        {
            case MethodInfo method:
                stateMachineType = method.ResolveRealMethodFromStateMachine(out var realMethodInfo);

                ProcessMethod(stringBuilder, realMethodInfo ?? method, stateMachineType, nameRControlFlags);
                
                break;
            case ConstructorInfo constructor:
                stateMachineType = StateMachineType.None;
                
                ProcessConstructor(stringBuilder, constructor, nameRControlFlags);
                
                break;
            default:
                throw new NotSupportedException($"Method type '{methodBase.GetType().Name}' is not supported");
        }
        
        stringBuilder.AppendLeftParenthesis();

        var parameters = methodBase.GetParameters();
        if (parameters.Length > 0)
        {
            ProcessParameters(stringBuilder, parameters, nameRControlFlags);
        }

        stringBuilder.AppendRightParenthesis();
    }
    
    private void ProcessMethod(StringBuilder stringBuilder, MethodInfo method, StateMachineType stateMachineType, NameRControlFlags nameRControlFlags)
    {
        ProcessMethodModifier(stringBuilder, method, nameRControlFlags);
        
        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeAsyncModifier) && stateMachineType.HasFlag(StateMachineType.Async))
        {
            stringBuilder.AppendAsyncWithEndingSpace();
        }

        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeReturnParameter))
        {
            ProcessParameter(stringBuilder, method.ReturnParameter, nameRControlFlags);

            stringBuilder.AppendSpace();
        }

        if (nameRControlFlags.HasFlag(NameRControlFlags.PrependFullTypeName) && method.DeclaringType is not null)
        {
            ProcessTypeCore(stringBuilder, method.DeclaringType, true, null, null);
            
            stringBuilder.AppendFullStop();
        }

        stringBuilder.Append(method.Name);

        if (method.IsGenericMethod)
        {
            var genericArguments = method.GetGenericArguments();
            
            ProcessGenerics(stringBuilder, genericArguments, null, 0, genericArguments.Length);
        }
    }

    private void ProcessConstructor(StringBuilder stringBuilder, ConstructorInfo constructor, NameRControlFlags nameRControlFlags)
    {
        ProcessMethodModifier(stringBuilder, constructor, nameRControlFlags);

        if (nameRControlFlags.HasFlag(NameRControlFlags.PrependFullTypeName) && constructor.DeclaringType is not null)
        {
            ProcessTypeCore(stringBuilder, constructor.DeclaringType, true, null, null);
            
            stringBuilder.AppendPlus();
        }

        if (!constructor.IsStatic)
        {
            stringBuilder.AppendConstructor();

            return;
        }

        stringBuilder.AppendStaticConstructor();
    }

    private static void ProcessMethodModifier(StringBuilder stringBuilder, MethodBase methodBase, NameRControlFlags nameRControlFlags)
    {
        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeAccessModifier))
        {
            // Mutually exclusive, a method cant be "private" and "public" and the same time
            if (methodBase.IsPrivate)
            {
                stringBuilder.AppendPrivateWithEndingSpace();
            }
            else if (methodBase.IsPublic)
            {
                stringBuilder.AppendPublicWithEndingSpace();
            }
        }

        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeStaticModifier) && methodBase.IsStatic)
        {
            stringBuilder.AppendStaticWithEndingSpace();
        }
    }

    private void ProcessParameters(StringBuilder stringBuilder, ParameterInfo[] parameters, NameRControlFlags nameRControlFlags)
    {
        for (var parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
        {
            ProcessParameter(stringBuilder, parameters[parameterIndex], nameRControlFlags);

            if (parameterIndex < parameters.Length - 1)
            {
                stringBuilder.AppendCommaWithEndingSpace();
            }
        }
    }

    private void ProcessParameter(StringBuilder stringBuilder, ParameterInfo parameterInfo, NameRControlFlags nameRControlFlags)
    {
        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeParameterPrefix))
        {
            ProcessParameterPrefix(stringBuilder, parameterInfo, nameRControlFlags);
        }

        var nullabilityInfo = nullabilityInfoContext.Create(parameterInfo);

        ProcessTypeCore(stringBuilder, parameterInfo.ParameterType, false, nullabilityInfo, null);

        if (parameterInfo.Position == Constants.ReturnParameterIndex)
        {
            return;
        }

        stringBuilder.AppendParameterName(parameterInfo.Name!);

        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeParameterDefaultValue))
        {
            ProcessParameterSuffix(stringBuilder, parameterInfo);
        }
    }

    private static void ProcessParameterPrefix(StringBuilder stringBuilder, ParameterInfo parameterInfo, NameRControlFlags nameRControlFlags)
    {
        // They are all mutually exclusive!
        // The "this" keyword is only valid on the first parameter
        if (parameterInfo.Position == Constants.ThisKeywordOnlyValidOnIndex
            && nameRControlFlags.HasFlag(NameRControlFlags.IncludeThisKeyword)
            && parameterInfo.Member.IsDefined(typeof(ExtensionAttribute), false))
        {
            stringBuilder.AppendThisWithEndingSpace();

            // Support "this in" and "this ref"
            if (!parameterInfo.IsIn && !parameterInfo.ParameterType.IsByRef)
            {
                return;
            }
        }

        // The "in" and "out" keywords are only valid on non return parameter
        if (parameterInfo.Position > Constants.ReturnParameterIndex)
        {
            if (parameterInfo.IsOut)
            {
                stringBuilder.AppendOutWithEndingSpace();

                return;
            }

            if (parameterInfo.IsIn)
            {
                stringBuilder.AppendInWithEndingSpace();

                return;
            }    
        }

        // The "in" and "out" keywords are also considered by-ref, in order to get correct results, "in" and "out" must be evaluated first
        // The "ref" keyword is also valid on return parameters
        if (parameterInfo.ParameterType.IsByRef)
        {
            stringBuilder.AppendRefWithEndingSpace();

            return;
        }

        // The "params" keyword is only valid on non return parameter
        if (parameterInfo.Position > Constants.ReturnParameterIndex
            && nameRControlFlags.HasFlag(NameRControlFlags.IncludeParamsKeyword)
            && parameterInfo.IsDefined(typeof(ParamArrayAttribute), false))
        {
            stringBuilder.AppendParamsWithEndingSpace();
        }
    }

    private static void ProcessParameterSuffix(StringBuilder stringBuilder, ParameterInfo parameterInfo)
    {
        if (!parameterInfo.IsOptional || !parameterInfo.HasDefaultValue)
        {
            return;
        }

        stringBuilder.AppendSpace();

        if (parameterInfo.DefaultValue is string @string)
        {
            stringBuilder.AppendQuotedParameterValue(@string);

            return;
        }

        if (parameterInfo.DefaultValue is null)
        {
            if (parameterInfo.ParameterType.IsValueType && Nullable.GetUnderlyingType(parameterInfo.ParameterType) is null)
            {
                stringBuilder.AppendEqualsDefaultWithLeadingSpace();
            }
            else
            {
                stringBuilder.AppendEqualsNullWithLeadingSpace();
            }

            return;
        }

        stringBuilder.AppendEqualsValue(parameterInfo.DefaultValue);
    }

    #endregion

    #region StackTrace specific

    /// <inheritdoc />
    public string GenerateDisplay(System.Diagnostics.StackTrace stackTrace, NameRControlFlags nameRControlFlags)
    {
        ArgumentNullException.ThrowIfNull(stackTrace);
        
        var stringBuilder = new StringBuilder();

        ProcessStackTrace(stringBuilder, stackTrace, nameRControlFlags);

        return stringBuilder.ToString();
    }

    /// <inheritdoc />
    public string GenerateDisplay(StackFrame stackFrame, NameRControlFlags nameRControlFlags)
    {
        ArgumentNullException.ThrowIfNull(stackFrame);

        var stringBuilder = new StringBuilder();

        ProcessStackFrame(stringBuilder, stackFrame, stackFrame.GetMethod(), Constants.DefaultCallDepth, nameRControlFlags);

        return stringBuilder.ToString();
    }

    private void ProcessStackTrace(StringBuilder stringBuilder, System.Diagnostics.StackTrace stackTrace, NameRControlFlags nameRControlFlags)
    {
        var recursiveStackTrace = stackTrace.GetFrames().FlattenRecursionAndFilterUnnecessaryStackFrames(nameRControlFlags, typeNameROptions.ExcludedNamespaces);
        for (var stackFrameIndex = 0; stackFrameIndex < recursiveStackTrace.Length; stackFrameIndex++)
        {
            var recursiveStackFrame = recursiveStackTrace[stackFrameIndex];

            ProcessStackFrame(stringBuilder, recursiveStackFrame.StackFrame, recursiveStackFrame.Method, recursiveStackFrame.CallDepth, nameRControlFlags);
            
            if (stackFrameIndex < recursiveStackTrace.Length - 1)
            {
                stringBuilder.AppendNewLine();
            }
        }
    }

    private void ProcessStackFrame(StringBuilder stringBuilder, StackFrame stackFrame, MethodBase? methodBase, uint callDepth, NameRControlFlags nameRControlFlags)
    {
        stringBuilder.AppendStackFramePreamble();
        
        if (methodBase is null)
        {
            stringBuilder.AppendUnknownStackFrameName();
        }
        else
        {
            ProcessMethod(stringBuilder, methodBase, nameRControlFlags, out var stateMachineType);
            
            if (stateMachineType.HasFlag(StateMachineType.Iterator))
            {
                stringBuilder.AppendMoveNextCallSuffix();
            }

            if (callDepth > Constants.DefaultCallDepth)
            {
                stringBuilder.AppendCallDepth(callDepth);
            }
        }

        if (!nameRControlFlags.HasFlag(NameRControlFlags.IncludeSourceInformation))
        {
            return;
        }

        var stackFrameMetadata = stackFrame.GetExistingStackFrameMetadata();
        if (!stackFrameMetadata.HasValue && methodBase is not null && nameRControlFlags.HasFlag(NameRControlFlags.FallbackToStackFrameMetadataProvider))
        {
            stackFrameMetadata = stackFrameMetadataProvider?.ProvideStackFrameMetadata(stackFrame, methodBase);
        }
        
        if (!stackFrameMetadata.HasValue)
        {
            return;
        }

        stringBuilder.AppendStackFrameMetadata(stackFrameMetadata.Value);
    }

    #endregion

    #region Exception specific

    /// <inheritdoc />
    public string GenerateDisplay(Exception exception, NameRControlFlags nameRControlFlags)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var stackTrace = new System.Diagnostics.StackTrace(exception, nameRControlFlags.HasFlag(NameRControlFlags.IncludeSourceInformation));

        var stringBuilder = new StringBuilder();

        // Write exception header
        ProcessTypeCore(stringBuilder, exception.GetType(), true, null, null);

        stringBuilder.AppendExceptionMessage(exception.Message);

        // Write exception stacktrace
        ProcessStackTrace(stringBuilder, stackTrace, nameRControlFlags);

        return stringBuilder.ToString();
    }

    /// <inheritdoc />
    public TException RewriteStackTrace<TException>(TException exception,
                                                    NameRControlFlags nameRControlFlags)
        where TException : Exception
    {
        if (!ExceptionStackTraceManipulator.StackTraceBackingFieldFound)
        {
            throw new StackTraceNotRewritableException("Backingfield for stacktrace does not exist on type 'Exception'");
        }

        ArgumentNullException.ThrowIfNull(exception);

        RewriteStackTraceCore(exception, nameRControlFlags);

        return exception;
    }

    private void RewriteStackTraceCore<TException>(TException exception,
                                                    NameRControlFlags nameRControlFlags)
          where TException : Exception
    {
        if (exception.StackTrace is not null)
        {
            var stringBuilder = new StringBuilder();
            
            var originalStackTrace = new System.Diagnostics.StackTrace(exception, nameRControlFlags.HasFlag(NameRControlFlags.IncludeSourceInformation));

            ProcessStackTrace(stringBuilder, originalStackTrace, nameRControlFlags);

            exception.SetStackTrace(stringBuilder, nameRControlFlags.HasFlag(NameRControlFlags.StoreOriginalStackTraceInExceptionData));
        }

        if (!nameRControlFlags.HasFlag(NameRControlFlags.IncludeInnerExceptions))
        {
            return;
        }

        if (exception.InnerException is not null)
        {
            RewriteStackTraceCore(exception.InnerException, nameRControlFlags);
        }

        if (exception is not AggregateException aggregateException)
        {
            return;
        }

        foreach (var innerException in aggregateException.InnerExceptions)
        {
            RewriteStackTraceCore(innerException, nameRControlFlags);
        }
    }

    #endregion
}