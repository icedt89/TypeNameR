using JanHafner.TypeNameR.StackTrace;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace JanHafner.TypeNameR;

public sealed class TypeNameR : ITypeNameR
{
    private readonly IStackFrameMetadataProvider stackFrameMetadataProvider;

    private readonly TypeNameROptions typeNameROptions;

    private const uint DefaultCallDepth = 1;

    /// <summary>
    /// Initializes a new instance of the <see cref="JanHafner.TypeNameR"/> class with the supplied <see cref="TypeNameROptions"/>.
    /// </summary>
    /// <param name="stackFrameMetadataProvider">If set to <see langword="null"/>, a new instance of <see cref="NullStackFrameMetadataProvider"/> will be used.</param>
    /// <param name="typeNameROptions">If set to <see langword="null"/>, a new instance initialized with the default values will be used internally.</param>
    public TypeNameR(IStackFrameMetadataProvider? stackFrameMetadataProvider = null,
                     TypeNameROptions? typeNameROptions = null)
    {
        this.stackFrameMetadataProvider = stackFrameMetadataProvider ?? new NullStackFrameMetadataProvider();
        this.typeNameROptions = typeNameROptions ?? new TypeNameROptions();
    }

    #region Type specific

    /// <inheritdoc />
    public string ExtractReadable(Type type)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        var stringBuilder = new StringBuilder();

        this.ProcessTypeCore(stringBuilder, type, false, null);
    
        return stringBuilder.ToString();
    }

    private void ProcessTypeCore(StringBuilder stringBuilder, Type type, bool isInNestedContext, NullabilityInfo? nullabilityInfo)
    {
        if (this.TryProcessNullableType(stringBuilder, type, nullabilityInfo))
        {
            return;
        }

        if (this.TryProcessPredefinedType(stringBuilder, type))
        {
            return;
        }

        if (this.TryProcessArrayType(stringBuilder, type, nullabilityInfo))
        {
            return;
        }

        if (this.TryProcessPointerType(stringBuilder, type, nullabilityInfo))
        {
            return;
        }

        if (this.TryProcessByRefType(stringBuilder, type, nullabilityInfo))
        {
            return;
        }

        this.ProcessNested(stringBuilder, type);

        TypeNameR.ProcessTypeName(stringBuilder, type, isInNestedContext);

        this.ProcessGenerics(stringBuilder, type.GetGenericArguments(), nullabilityInfo?.GenericTypeArguments);
    }

    private static void ProcessTypeName(StringBuilder stringBuilder, Type type, bool isInNestedContext)
    {
        ReadOnlySpan<char> readableTypeName = isInNestedContext && type.DeclaringType is null
                                   ? type.FullName ?? type.Name
                                   : type.Name;

        stringBuilder.Append(readableTypeName.RemoveGenericParametersCount());
    }

    private bool TryProcessPredefinedType(StringBuilder stringBuilder, Type type)
    {
        if (!this.typeNameROptions.PredefinedTypeNames.TryGetValue(type, out var readablePrimitiveTypeName1))
        {
            return false;
        }

        stringBuilder.Append(readablePrimitiveTypeName1);

        return true;
    }

    private bool TryProcessArrayType(StringBuilder stringBuilder, Type type, NullabilityInfo? nullabilityInfo)
    {
        if (!type.IsArray)
        {
            return false;
        }

        var elementType = type.GetElementType()!;

        this.ProcessTypeCore(stringBuilder, elementType, false, nullabilityInfo);

        stringBuilder.Append(Symbol.LeftSquareBracket);

        if (type.IsVariableBoundArray)
        {
            var arrayRank = type.GetArrayRank();

            stringBuilder.Append(Symbol.Comma, arrayRank - 1);
        }

        stringBuilder.Append(Symbol.RightSquareBracket);

        return true;
    }

    private bool TryProcessPointerType(StringBuilder stringBuilder, Type type, NullabilityInfo? nullabilityInfo)
    {
        if (!type.IsPointer)
        {
            return false;
        }

        var elementType = type.GetElementType()!;

        this.ProcessTypeCore(stringBuilder, elementType, false, nullabilityInfo?.ElementType);

        stringBuilder.Append(Symbol.Asterisk);

        return true;
    }

    private bool TryProcessByRefType(StringBuilder stringBuilder, Type type, NullabilityInfo? nullabilityInfo)
    {
        if (!type.IsByRef)
        {
            return false;
        }

        var elementType = type.GetElementType()!;

        this.ProcessTypeCore(stringBuilder, elementType, false, nullabilityInfo?.ElementType);

        return true;
    }

    private bool TryProcessNullableType(StringBuilder stringBuilder, Type type, NullabilityInfo? nullabilityInfo)
    {
        if (type.IsNullableStruct(out var underlyingType))
        {
            this.ProcessTypeCore(stringBuilder, underlyingType, false, null);

            stringBuilder.Append(Symbol.QuestionMark);

            return true;
        }

        if (nullabilityInfo is not null && nullabilityInfo.Type == type && nullabilityInfo.IsNullableReferenceType())
        {
            this.ProcessTypeCore(stringBuilder, type, false, nullabilityInfo.ElementType);

            stringBuilder.Append(Symbol.QuestionMark);

            return true;
        }

        return false;
    }

    private void ProcessGenerics(StringBuilder stringBuilder, Type[] genericTypes, NullabilityInfo[]? genericTypesNullability)
    {
        if (genericTypes.Length == 0)
        {
            return;
        }

        stringBuilder.Append(Symbol.LessThanSign);

        for (var i = 0; i < genericTypes.Length; i++)
        {
            NullabilityInfo? genericTypeNullabilityInfo = null;
            if (genericTypesNullability is not null && i < genericTypesNullability.Length)
            {
                genericTypeNullabilityInfo = genericTypesNullability[i];
            }

            this.ProcessTypeCore(stringBuilder, genericTypes[i], false, genericTypeNullabilityInfo);

            if (i < genericTypes.Length - 1)
            {
                stringBuilder.Append(Symbol.CommaWithEndingSpace);
            }
        }

        stringBuilder.Append(Symbol.GreaterThanSign);
    }
    
    private void ProcessNested(StringBuilder stringBuilder, Type type)
    {
        if (!type.IsNested || type.IsGenericParameter || type.DeclaringType is null)
        {
            return;
        }

        this.ProcessTypeCore(stringBuilder, type.DeclaringType, true, null);

        stringBuilder.Append(Symbol.Plus);
    }

    #endregion

    #region Method specific

    /// <inheritdoc />
    public string ExtractReadable(MethodBase methodBase, NameRControlFlags nameRControlFlags)
    {
        if (methodBase is null)
        {
            throw new ArgumentNullException(nameof(methodBase));
        }

        var stringBuilder = new StringBuilder();

        this.ProcessMethod(stringBuilder, methodBase, StateMachineTypes.None, nameRControlFlags);

        return stringBuilder.ToString();
    }

    /// <inheritdoc />
    public string ExtractReadable(ParameterInfo parameterInfo, NameRControlFlags nameRControlFlags)
    {
        if (parameterInfo is null)
        {
            throw new ArgumentNullException(nameof(parameterInfo));
        }

        var stringBuilder = new StringBuilder();

        this.ProcessParameter(stringBuilder, parameterInfo, nameRControlFlags);

        return stringBuilder.ToString();
    }

    private void ProcessMethod(StringBuilder stringBuilder, MethodBase methodBase, StateMachineTypes stateMachineType, NameRControlFlags nameRControlFlags)
    {
        TypeNameR.ProcessMethodModifier(stringBuilder, methodBase, stateMachineType, nameRControlFlags);

        // Otherwise it is a constructor
        var asMethodInfo = methodBase as MethodInfo;
        if (asMethodInfo is not null && nameRControlFlags.HasFlag(NameRControlFlags.IncludeReturnParameter))
        {
            this.ProcessParameter(stringBuilder, asMethodInfo.ReturnParameter, nameRControlFlags);

            stringBuilder.Append(Symbol.Space);
        }

        if (methodBase.DeclaringType is not null && nameRControlFlags.HasFlag(NameRControlFlags.IncludeFullTypeName))
        {
            stringBuilder.Append(methodBase.DeclaringType.Namespace)
                         .Append(Symbol.FullStop);
            
            this.ProcessTypeCore(stringBuilder, methodBase.DeclaringType, false, null);
            
            stringBuilder.Append(Symbol.FullStop);
        }

        if (asMethodInfo is not null)
        {
            stringBuilder.Append(methodBase.Name);
        }
        else if(methodBase.IsStatic)
        {
            stringBuilder.Append(Symbol.StaticConstructor);
        }
        else
        {
            stringBuilder.Append(Symbol.Constructor);
        }

        if (asMethodInfo is not null)
        {
            this.ProcessGenerics(stringBuilder, methodBase.GetGenericArguments(), null);
        }

        stringBuilder.Append(Symbol.LeftParenthesis);

        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeThisKeywordForExtensions) && methodBase.IsExtension())
        {
            stringBuilder.Append(Modifier.ThisWithEndingSpace);
        }
        
        this.ProcessParameters(stringBuilder, methodBase.GetParameters(), nameRControlFlags);

        stringBuilder.Append(Symbol.RightParenthesis);
    }

    private static void ProcessMethodModifier(StringBuilder stringBuilder, MethodBase methodBase, StateMachineTypes stateMachineType, NameRControlFlags nameRControlFlags)
    {
        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeAccessModifier))
        {
            if (methodBase.IsPrivate)
            {
                stringBuilder.Append(Modifier.PrivateWithEndingSpace);
            }
            else if (methodBase.IsPublic)
            {
                stringBuilder.Append(Modifier.PublicWithEndingSpace);
            }
        }

        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeStaticModifier) && methodBase.IsStatic)
        {
            stringBuilder.Append(Modifier.StaticWithEndingSpace);
        }

        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeAsyncModifier) && stateMachineType.HasFlag(StateMachineTypes.Async))
        {
            stringBuilder.Append(Modifier.AsyncWithEndingSpace);
        }
    }

    private void ProcessParameters(StringBuilder stringBuilder, ParameterInfo[] parameters, NameRControlFlags nameRControlFlags)
    {
        if (parameters.Length == 0)
        {
            return;
        }

        for (var i = 0; i < parameters.Length; i++)
        {
            this.ProcessParameter(stringBuilder, parameters[i], nameRControlFlags);

            if (i < parameters.Length - 1)
            {
                stringBuilder.Append(Symbol.CommaWithEndingSpace);
            }
        }
    }

    private void ProcessParameter(StringBuilder stringBuilder, ParameterInfo parameterInfo, NameRControlFlags nameRControlFlags)
    {
        if (parameterInfo.Position > -1 && nameRControlFlags.HasFlag(NameRControlFlags.IncludeParameterModifiers))
        {
            ProcessParameterModifier(stringBuilder, parameterInfo);
        }

        var nullabilityInfo = parameterInfo.GetNullabilityInfo();

        this.ProcessTypeCore(stringBuilder, parameterInfo.ParameterType, false, nullabilityInfo);

        if (parameterInfo.Position == -1)
        {
            return;
        }

        stringBuilder.Append(Symbol.Space)
                     .Append(parameterInfo.Name);

        if (nameRControlFlags.HasFlag(NameRControlFlags.IncludeParameterDefaultValue))
        {
            TypeNameR.ProcessParameterSuffix(stringBuilder, parameterInfo);
        }
    }

    private static void ProcessParameterModifier(StringBuilder stringBuilder, ParameterInfo parameterInfo)
    {
        if (parameterInfo.IsOut)
        {
            stringBuilder.Append(Modifier.OutWithEndingSpace);

            return;
        }

        if (parameterInfo.IsIn)
        {
            stringBuilder.Append(Modifier.InWithEndingSpace);

            return;
        }

        if (parameterInfo.ParameterType.IsByRef)
        {
            stringBuilder.Append(Modifier.RefWithEndingSpace);
        }
    }

    private static void ProcessParameterSuffix(StringBuilder stringBuilder, ParameterInfo parameterInfo)
    {
        if (!parameterInfo.IsOptional || !parameterInfo.HasDefaultValue)
        {
            return;
        }

        stringBuilder.Append(Symbol.Space);

        if (parameterInfo.DefaultValue is string @string)
        {
            stringBuilder.Append(Symbol.EqualsSignWithEndingSpace)
                         .Append(Symbol.QuotationMark)
                         .Append(@string)
                         .Append(Symbol.QuotationMark);

            return;
        }

        if (parameterInfo.DefaultValue is null)
        {
            stringBuilder.Append(Symbol.EqualsSign)
                         .Append(Literal.DefaultWithLeadingSpace);

            return;
        }

        stringBuilder.Append(Symbol.EqualsSignWithEndingSpace)
                     .Append(parameterInfo.DefaultValue);
    }

    #endregion

    #region StackTrace specific

    /// <inheritdoc />
    public string ExtractReadable(System.Diagnostics.StackTrace stackTrace, NameRControlFlags nameRControlFlags)
    {
        if (stackTrace is null)
        {
            throw new ArgumentNullException(nameof(stackTrace));
        }
        
        var stringBuilder = new StringBuilder();

        this.ProcessStackTrace(stringBuilder, stackTrace, nameRControlFlags);

        return stringBuilder.ToString();
    }

    /// <inheritdoc />
    public string ExtractReadable(StackFrame stackFrame, NameRControlFlags nameRControlFlags)
    {
        if (stackFrame is null)
        {
            throw new ArgumentNullException(nameof(stackFrame));
        }

        var stringBuilder = new StringBuilder();

        if (this.ProcessStackFrame(stringBuilder, stackFrame, stackFrame.GetMethod(), TypeNameR.DefaultCallDepth, nameRControlFlags))
        {
            throw new InvalidOperationException("Unable to process stack frame");
        }

        return stringBuilder.ToString();
    }

    private void ProcessStackTrace(StringBuilder stringBuilder, System.Diagnostics.StackTrace stackTrace, NameRControlFlags nameRControlFlags)
    {
        var recursiveStackTrace = stackTrace.EliminateRecursion();
        for (var frameIndex = 0; frameIndex < recursiveStackTrace.Length; frameIndex++)
        {
            var recursiveStackFrame = recursiveStackTrace[frameIndex];

            if (this.ProcessStackFrame(stringBuilder, recursiveStackFrame.StackFrame, recursiveStackFrame.Method, recursiveStackFrame.CallDepth, nameRControlFlags) && frameIndex < recursiveStackTrace.Length - 1)
            {
                stringBuilder.Append(Symbol.NewLine);
            }
        }
    }

    private bool ProcessStackFrame(StringBuilder stringBuilder, StackFrame currentStackFrame, MethodBase? methodBase, uint callDepth, NameRControlFlags nameRControlFlags)
    {
        if (methodBase is null)
        {
            return false;
        }

        if (this.SkipStackFrame(nameRControlFlags, methodBase))
        {
            return false;
        }

        var realMethodInfo = methodBase as MethodInfo;
        var stateMachineType = StateMachineTypes.None;
        if(realMethodInfo is not null)
        {
            stateMachineType = realMethodInfo.ResolveRealMethodFromStateMachine(out realMethodInfo);
        }

        stringBuilder.Append(StackTraceSymbol.Indent)
                     .Append(StackTraceSymbol.AtWithEndingSpace);

        this.ProcessMethod(stringBuilder, realMethodInfo ?? methodBase, stateMachineType, nameRControlFlags);

        if (stateMachineType.HasFlag(StateMachineTypes.Iterator))
        {
            stringBuilder.Append(StackTraceSymbol.MoveNextCallSuffix);
        }

        if (callDepth > DefaultCallDepth)
        {
            stringBuilder.Append(StackTraceSymbol.RecursionMarkWithLeadingAndEndingSpace)
                         .Append(callDepth);
        }

        if (!nameRControlFlags.HasFlag(NameRControlFlags.IncludeSourceInformation))
        {
            return true;
        }

        var stackFrameMetadata = currentStackFrame.GetExistingStackFrameMetadata() ?? this.stackFrameMetadataProvider.GetStackFrameMetadata(currentStackFrame);
        if (stackFrameMetadata.HasValue)
        {
            stringBuilder.Append(StackTraceSymbol.InSourceWithLeadingAndEndingSpace)
                         .Append(stackFrameMetadata.Value.FileName)
                         .Append(StackTraceSymbol.LineWithEndingSpace)
                         .Append(stackFrameMetadata.Value.LineNumber);
        }

        return true;
    }

    private bool SkipStackFrame(NameRControlFlags nameRControlFlags, MethodBase methodBase)
    {
        return !nameRControlFlags.HasFlag(NameRControlFlags.IncludeHiddenStackFrames) && (methodBase.IsHidden() || methodBase.IsValueTaskSource()
                                                                                 || (nameRControlFlags.HasFlag(NameRControlFlags.ExcludeStackFrameMethodsByNamespace)
                                                                                  && this.typeNameROptions.ExcludedNamespaces.IsNamespaceExcluded(methodBase)));
    }

    #endregion

    #region Exception specific

    /// <inheritdoc />
    public string ExtractReadableStackTrace(Exception exception, NameRControlFlags nameRControlFlags)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        var stackTrace = new System.Diagnostics.StackTrace(exception, nameRControlFlags.HasFlag(NameRControlFlags.IncludeSourceInformation));

        var stringBuilder = new StringBuilder();

        this.ProcessStackTrace(stringBuilder, stackTrace, nameRControlFlags);

        return stringBuilder.ToString();
    }

        /// <inheritdoc />
    public TException RewriteStackTrace<TException>(TException exception,
                                                    NameRControlFlags nameRControlFlags)
        where TException : notnull, Exception
    {
        if (!ExceptionStackTraceManipulator.WasStackTraceBackingFieldFound)
        {
            throw new StackTraceNotRewritableException("Backingfield for stacktrace does not exist on type 'Exception'");
        }

        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        this.RewriteStackTraceCore(exception, nameRControlFlags);

        return exception;
    }

    private void RewriteStackTraceCore<TException>(TException exception,
                                                    NameRControlFlags nameRControlFlags)
          where TException : notnull, Exception
    {
        try
        {
            if (exception.StackTrace is not null)
            {
                var stringBuilder = new StringBuilder();
                
                var originalStackTrace = new System.Diagnostics.StackTrace(exception, nameRControlFlags.HasFlag(NameRControlFlags.IncludeSourceInformation));

                this.ProcessStackTrace(stringBuilder, originalStackTrace, nameRControlFlags);

                exception.SetStackTrace(stringBuilder, nameRControlFlags.HasFlag(NameRControlFlags.StoreOriginalStackTraceInExceptionData));
            }

            if (!nameRControlFlags.HasFlag(NameRControlFlags.IncludeInnerExceptions))
            {
                return;
            }

            if (exception.InnerException is not null)
            {
                this.RewriteStackTraceCore(exception.InnerException, nameRControlFlags);
            }

            if (exception is not AggregateException aggregateException)
            {
                return;
            }

            foreach (var innerException in aggregateException.InnerExceptions)
            {
                this.RewriteStackTraceCore(innerException, nameRControlFlags);
            }
        }
        catch
        {
            Debug.Fail(null);
        }
    }

    #endregion
}