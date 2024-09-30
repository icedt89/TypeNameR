using System.Reflection;
using System.Runtime.CompilerServices;

namespace JanHafner.TypeNameR.Experimental.Helper;

internal static class StateMachineHelper
{
    public static StateMachineType ResolveRealMethodFromStateMachine(this MethodInfo methodInfo, out MethodInfo? realMethodInfo)
    {
        // If the method has the desired attribute return instant
        var stateMachineType = methodInfo.GetStateMachineType(out var stateMachineImplementationType);
        if (stateMachineType != StateMachineType.None && stateMachineImplementationType is not null)
        {
            realMethodInfo = methodInfo;

            return stateMachineType;
        }

        // It is assumed that any state machine is nested inside a type of the type which uses it
        var generatedType = methodInfo.DeclaringType;
        if (generatedType is null)
        {
            realMethodInfo = null;

            return StateMachineType.None;
        }

        return generatedType.ResolveRealMethodFromStateMachineType(out realMethodInfo);
    }

    private static StateMachineType ResolveRealMethodFromStateMachineType(this Type generatedType, out MethodInfo? realMethodInfo)
    {
        // We need the type in which the state machine resides
        var originalType = generatedType.DeclaringType;
        if (originalType is null)
        {
            realMethodInfo = null;

            return StateMachineType.None;
        }

        // ...and has a compiler generated annotation
        if (!generatedType.HasCompilerGeneratedAttribute())
        {
            realMethodInfo = null;

            return StateMachineType.None;
        }
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
        var possibleMethods = originalType.GetMethods(BindingFlags.Public
                                                      | BindingFlags.NonPublic
                                                      | BindingFlags.Static
                                                      | BindingFlags.Instance
                                                      | BindingFlags.DeclaredOnly);
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
        foreach (var possibleMethod in possibleMethods)
        {
            var stateMachineType = possibleMethod.GetStateMachineType(out var stateMachineImplementationType);
            if (stateMachineType == StateMachineType.None || stateMachineImplementationType is null || stateMachineImplementationType != generatedType)
            {
                continue;
            }

            realMethodInfo = possibleMethod;

            return stateMachineType;
        }

        realMethodInfo = null;

        return StateMachineType.None;
    }

    private static StateMachineType GetStateMachineType(this MethodInfo methodInfo, out Type? stateMachineImplementationType)
    {
        var stateMachineAttribute = methodInfo.FindStateMachineAttribute();
        if (stateMachineAttribute?.StateMachineType is null)
        {
            stateMachineImplementationType = null;

            return StateMachineType.None;
        }

        stateMachineImplementationType = stateMachineAttribute.StateMachineType;

        return stateMachineAttribute switch
        {
            AsyncStateMachineAttribute => StateMachineType.Async,
            IteratorStateMachineAttribute => StateMachineType.Iterator,
            AsyncIteratorStateMachineAttribute => StateMachineType.AsyncIterator,
            _ => throw new InvalidOperationException($"Unknown {nameof(StateMachineAttribute)} => {stateMachineAttribute.GetType().Name}")
        };
    }
}