﻿using JanHafner.TypeNameR.Helper;
using System.Reflection;
using System.Text;

#if NET6_0
using NullabilityInfoContext = Nullability.NullabilityInfoContextEx;
using NullabilityInfo = Nullability.NullabilityInfoEx;
using NullabilityState = Nullability.NullabilityStateEx;
#endif

namespace JanHafner.TypeNameR;

/// <inheritdoc />
public partial class TypeNameR
{
    /// <inheritdoc />
    public string GenerateDisplay(MethodBase methodBase, NameRControlFlags nameRControlFlags)
    {
        ArgumentNullException.ThrowIfNull(methodBase);

        var stringBuilder = new StringBuilder();
        var nullabilityInfoContext = new NullabilityInfoContext();

        ProcessMethod(stringBuilder, nullabilityInfoContext, methodBase, nameRControlFlags, out _);

        return stringBuilder.ToString();
    }

    private void ProcessMethod(StringBuilder stringBuilder, NullabilityInfoContext nullabilityInfoContext, MethodBase methodBase, NameRControlFlags nameRControlFlags, out StateMachineType stateMachineType)
    {
        switch (methodBase)
        {
            case MethodInfo method:
                stateMachineType = method.ResolveRealMethodFromStateMachine(out var realMethodInfo);

                ProcessMethod(stringBuilder, nullabilityInfoContext, realMethodInfo ?? method, stateMachineType, nameRControlFlags);

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
            ProcessParameters(stringBuilder, nullabilityInfoContext, parameters, nameRControlFlags);
        }

        stringBuilder.AppendRightParenthesis();
    }

    private void ProcessMethod(StringBuilder stringBuilder, NullabilityInfoContext nullabilityInfoContext, MethodInfo method, StateMachineType stateMachineType, NameRControlFlags nameRControlFlags)
    {
        ProcessMethodModifier(stringBuilder, method, nameRControlFlags);

        if (nameRControlFlags.IsSet(NameRControlFlags.IncludeAsyncModifier) && stateMachineType.IsSet(StateMachineType.Async))
        {
            stringBuilder.AppendAsyncWithEndingSpace();
        }

        if (nameRControlFlags.IsSet(NameRControlFlags.IncludeReturnParameter))
        {
            ProcessParameter(stringBuilder, nullabilityInfoContext, method.ReturnParameter, nameRControlFlags);

            stringBuilder.AppendSpace();
        }

        if (nameRControlFlags.IsSet(NameRControlFlags.PrependFullTypeNameBeforeMethodName) && method.DeclaringType is not null)
        {
            ProcessTypeCore(stringBuilder, method.DeclaringType, true, null, null, nameRControlFlags);

            stringBuilder.AppendFullStop();
        }

        stringBuilder.Append(method.Name);

        if (nameRControlFlags.IsSet(NameRControlFlags.IncludeGenericParameters) && method.IsGenericMethod)
        {
            stringBuilder.AppendLessThanSign();

            var genericArguments = method.GetGenericArguments();

            ProcessGenerics(stringBuilder, genericArguments, null, 0, genericArguments.Length, nameRControlFlags);

            stringBuilder.AppendGreaterThanSign();
        }
    }

    private void ProcessConstructor(StringBuilder stringBuilder, ConstructorInfo constructor, NameRControlFlags nameRControlFlags)
    {
        ProcessMethodModifier(stringBuilder, constructor, nameRControlFlags);

        if (nameRControlFlags.IsSet(NameRControlFlags.PrependFullTypeNameBeforeMethodName) && constructor.DeclaringType is not null)
        {
            ProcessTypeCore(stringBuilder, constructor.DeclaringType, true, null, null, nameRControlFlags);

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
        if (nameRControlFlags.IsSet(NameRControlFlags.IncludeAccessModifier))
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

        if (nameRControlFlags.IsSet(NameRControlFlags.IncludeStaticModifier) && methodBase.IsStatic)
        {
            stringBuilder.AppendStaticWithEndingSpace();
        }
    }
}