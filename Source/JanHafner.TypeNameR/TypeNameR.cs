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

    #region Type specific

    /// <inheritdoc />
    public string GenerateDisplay(Type type, bool fullTypeName, NullabilityInfo? nullabilityInfo)
    {
        ArgumentNullException.ThrowIfNull(type);

        var stringBuilder = new StringBuilder();

        ProcessTypeCore(stringBuilder, type, fullTypeName, nullabilityInfo);
    
        return stringBuilder.ToString();
    }

    private void ProcessTypeCore(StringBuilder stringBuilder, Type type, bool fullTypeName, NullabilityInfo? nullabilityInfo)
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
                if (type.IsArray)
                {
                    ProcessTypeCore(stringBuilder, elementType, fullTypeName, nullabilityInfo?.ElementType);
        
                    stringBuilder.Append(Constants.LeftSquareBracket)
                        .Append(Constants.Comma, type.GetArrayRank() - 1)
                        .Append(Constants.RightSquareBracket);
        
                    skipTypeAndGenericsAndNullable = true;
                }
                else
                {
                    ProcessTypeCore(stringBuilder, elementType, fullTypeName, nullabilityInfo);

                    if (type.IsPointer)
                    {
                        stringBuilder.Append(Constants.Asterisk);
                    }
                
                    skipTypeAndGenericsAndNullable = true;
                }                
            }
        }

        var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
        if (!skipTypeAndGenericsAndNullable && nullableUnderlyingType is not null)
        {
            ProcessTypeCore(stringBuilder, nullableUnderlyingType, fullTypeName, null);
            
            stringBuilder.Append(Constants.QuestionMark);

            return;
        }

        if (!skipTypeAndGenericsAndNullable)
        {
            // Nested
            if (type.DeclaringType is not null && !type.IsGenericParameter)
            {
                ProcessTypeCore(stringBuilder, type.DeclaringType, true, null);

                stringBuilder.Append(Constants.Plus);
            }
            
            // Full type name
            if (fullTypeName && type.DeclaringType is null && type.Namespace is not null)
            {
                stringBuilder.Append(type.Namespace)
                    .Append(Constants.FullStop);
            }

            var typeNameSpan = type.Name.AsSpan();
            if (type.IsGenericType)
            {
                stringBuilder.Append(typeNameSpan.RemoveGenericParametersCount());

                ProcessGenerics(stringBuilder, type.GetGenericArguments(), nullabilityInfo?.GenericTypeArguments);
            }
            else
            {
                stringBuilder.Append(typeNameSpan);
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
            stringBuilder.Append(Constants.QuestionMark);
        }
    }

    private void ProcessGenerics(StringBuilder stringBuilder, Type[] genericTypes, NullabilityInfo[]? genericTypesNullability)
    {
        stringBuilder.Append(Constants.LessThanSign);

        for (var genericParameterIndex = 0; genericParameterIndex < genericTypes.Length; genericParameterIndex++)
        {
            NullabilityInfo? genericTypeNullabilityInfo = null;
            if (genericTypesNullability is not null && genericParameterIndex < genericTypesNullability.Length)
            {
                genericTypeNullabilityInfo = genericTypesNullability[genericParameterIndex];
            }

            ProcessTypeCore(stringBuilder, genericTypes[genericParameterIndex], false, genericTypeNullabilityInfo);

            if (genericParameterIndex < genericTypes.Length - 1)
            {
                stringBuilder.Append(Constants.CommaWithEndingSpace);
            }
        }

        stringBuilder.Append(Constants.GreaterThanSign);
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
        
        stringBuilder.Append(Constants.LeftParenthesis);

        var parameters = methodBase.GetParameters();
        if (parameters.Length > 0)
        {
            ProcessParameters(stringBuilder, parameters, nameRControlFlags);
        }

        stringBuilder.Append(Constants.RightParenthesis);
    }
    
    private void ProcessMethod(StringBuilder stringBuilder, MethodInfo method, StateMachineType stateMachineType, NameRControlFlags nameRControlFlags)
    {
        ProcessMethodModifier(stringBuilder, method, nameRControlFlags);
        
        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeAsyncModifier) && stateMachineType.HasFlag(StateMachineType.Async))
        {
            stringBuilder.Append(Constants.AsyncWithEndingSpace);
        }

        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeReturnParameter))
        {
            ProcessParameter(stringBuilder, method.ReturnParameter, nameRControlFlags);

            stringBuilder.Append(Constants.Space);
        }

        if (nameRControlFlags.HasFlag(NameRControlFlags.PrependFullTypeName) && method.DeclaringType is not null)
        {
            ProcessTypeCore(stringBuilder, method.DeclaringType, true, null);
            
            stringBuilder.Append(Constants.FullStop);
        }

        stringBuilder.Append(method.Name);

        if (method.IsGenericMethod)
        {
            ProcessGenerics(stringBuilder, method.GetGenericArguments(), null);
        }
    }

    private void ProcessConstructor(StringBuilder stringBuilder, ConstructorInfo constructor, NameRControlFlags nameRControlFlags)
    {
        ProcessMethodModifier(stringBuilder, constructor, nameRControlFlags);

        if (nameRControlFlags.HasFlag(NameRControlFlags.PrependFullTypeName) && constructor.DeclaringType is not null)
        {
            ProcessTypeCore(stringBuilder, constructor.DeclaringType, true, null);
            
            stringBuilder.Append(Constants.FullStop);
        }

        if (!constructor.IsStatic)
        {
            stringBuilder.Append(Constants.Constructor);

            return;
        }

        stringBuilder.Append(Constants.StaticConstructor);
    }

    private static void ProcessMethodModifier(StringBuilder stringBuilder, MethodBase methodBase, NameRControlFlags nameRControlFlags)
    {
        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeAccessModifier))
        {
            // Mutually exclusive, a method cant be "private" and "public" and the same time
            if (methodBase.IsPrivate)
            {
                stringBuilder.Append(Constants.PrivateWithEndingSpace);
            }
            else if (methodBase.IsPublic)
            {
                stringBuilder.Append(Constants.PublicWithEndingSpace);
            }
        }

        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeStaticModifier) && methodBase.IsStatic)
        {
            stringBuilder.Append(Constants.StaticWithEndingSpace);
        }
    }

    private void ProcessParameters(StringBuilder stringBuilder, ParameterInfo[] parameters, NameRControlFlags nameRControlFlags)
    {
        for (var parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
        {
            ProcessParameter(stringBuilder, parameters[parameterIndex], nameRControlFlags);

            if (parameterIndex < parameters.Length - 1)
            {
                stringBuilder.Append(Constants.CommaWithEndingSpace);
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

        ProcessTypeCore(stringBuilder, parameterInfo.ParameterType, false, nullabilityInfo);

        if (parameterInfo.Position == Constants.ReturnParameterIndex)
        {
            return;
        }

        stringBuilder.Append(Constants.Space)
                     .Append(parameterInfo.Name);

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
            stringBuilder.Append(Constants.ThisWithEndingSpace);

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
                stringBuilder.Append(Constants.OutWithEndingSpace);

                return;
            }

            if (parameterInfo.IsIn)
            {
                stringBuilder.Append(Constants.InWithEndingSpace);

                return;
            }    
        }

        // The "in" and "out" keywords are also considered by-ref, in order to get correct results, "in" and "out" must be evaluated first
        // The "ref" keyword is also valid on return parameters
        if (parameterInfo.ParameterType.IsByRef)
        {
            stringBuilder.Append(Constants.RefWithEndingSpace);

            return;
        }

        // The "params" keyword is only valid on non return parameter
        if (parameterInfo.Position > Constants.ReturnParameterIndex 
            && nameRControlFlags.HasFlag(NameRControlFlags.IncludeParamsKeyword)
            && parameterInfo.IsDefined(typeof(ParamArrayAttribute), false))
        {
            stringBuilder.Append(Constants.ParamsWithEndingSpace);
        }
    }

    private static void ProcessParameterSuffix(StringBuilder stringBuilder, ParameterInfo parameterInfo)
    {
        if (!parameterInfo.IsOptional || !parameterInfo.HasDefaultValue)
        {
            return;
        }

        stringBuilder.Append(Constants.Space);

        if (parameterInfo.DefaultValue is string @string)
        {
            // The compiler will join the constants at the call-site
            stringBuilder.Append(Constants.EqualsSignWithEndingSpace + Constants.QuotationMark)
                         .Append(@string)
                         .Append(Constants.QuotationMark);

            return;
        }

        if (parameterInfo.DefaultValue is null)
        {
            if (parameterInfo.ParameterType.IsValueType && Nullable.GetUnderlyingType(parameterInfo.ParameterType) is null)
            {
                // The compiler will join the constants at the call-site
                stringBuilder.Append(Constants.EqualsSign + Constants.DefaultWithLeadingSpace);
            }
            else
            {
                // The compiler will join the constants at the call-site
                stringBuilder.Append(Constants.EqualsSign + Constants.NullWithLeadingSpace);
            }

            return;
        }

        stringBuilder.Append(Constants.EqualsSignWithEndingSpace)
                     .Append(parameterInfo.DefaultValue);
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
        var recursiveStackTrace = stackTrace.GetFrames().AsSpan().FlattenRecursionAndFilterUnnecessaryStackFrames(nameRControlFlags, typeNameROptions.ExcludedNamespaces);
        for (var stackFrameIndex = 0; stackFrameIndex < recursiveStackTrace.Length; stackFrameIndex++)
        {
            var recursiveStackFrame = recursiveStackTrace[stackFrameIndex];

            ProcessStackFrame(stringBuilder, recursiveStackFrame.StackFrame, recursiveStackFrame.Method, recursiveStackFrame.CallDepth, nameRControlFlags);
            
            if (stackFrameIndex < recursiveStackTrace.Length - 1)
            {
                stringBuilder.Append(Constants.NewLine);
            }
        }
    }

    private void ProcessStackFrame(StringBuilder stringBuilder, StackFrame stackFrame, MethodBase? methodBase, uint callDepth, NameRControlFlags nameRControlFlags)
    {
        // The compiler will join the constants at the call-site
        stringBuilder.Append(Constants.Indent + Constants.AtWithEndingSpace);
        
        if (methodBase is null)
        {
            stringBuilder.Append(Constants.UnknownStackFrameName);
        }
        else
        {
            ProcessMethod(stringBuilder, methodBase, nameRControlFlags, out var stateMachineType);
            
            if (stateMachineType.HasFlag(StateMachineType.Iterator))
            {
                stringBuilder.Append(Constants.MoveNextCallSuffix);
            }

            if (callDepth > Constants.DefaultCallDepth)
            {
                stringBuilder.Append(Constants.RecursionMarkWithLeadingAndEndingSpace)
                    .Append(callDepth);
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

        stringBuilder.Append(Constants.InSourceWithLeadingAndEndingSpace)
            .Append(stackFrameMetadata.Value.FileName);

        if (stackFrameMetadata.Value.LineNumber == 0)
        {
            return;
        }

        stringBuilder.Append(Constants.LineWithEndingSpace)
            .Append(stackFrameMetadata.Value.LineNumber);

        if (stackFrameMetadata.Value.ColumnNumber > 0)
        {
            stringBuilder.Append(Constants.Colon)
                .Append(stackFrameMetadata.Value.ColumnNumber);
        }
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
        ProcessTypeCore(stringBuilder, exception.GetType(), true, null);

        stringBuilder.Append(Constants.ColonWithEndingSpace).AppendLine(exception.Message);

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