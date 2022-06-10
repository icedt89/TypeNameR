using System.Reflection;
using System.Runtime.CompilerServices;

namespace JanHafner.TypeNameR;

internal static class StateMachineHelper
{
    public static StateMachineTypes ResolveRealMethodFromStateMachine(this MethodInfo methodBase, out MethodInfo? realMethodInfo)
    {
        realMethodInfo = null;

        // It is assumed that any state machine is nested inside the type which uses it
        var generatedType = methodBase.DeclaringType;
        if (generatedType is null)
        {
            return StateMachineTypes.None;
        }

        // We need the type in which the state machine resides
        var originalType = generatedType.DeclaringType;
        if (originalType is null)
        {
            return StateMachineTypes.None;
        }

        // and has a compiler generated annotation
        if (!generatedType.IsCompilerGenerated())
        {
            return StateMachineTypes.None;
        }

#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
        var possibleMethods = originalType.GetMethods(BindingFlags.Public
                                                    | BindingFlags.NonPublic
                                                    | BindingFlags.Static
                                                    | BindingFlags.Instance
                                                    | BindingFlags.DeclaredOnly);
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
        if (possibleMethods.Length == 0)
        {
            return StateMachineTypes.None;
        }

        foreach (var possibleMethod in possibleMethods)
        {
            var stateMachineType = possibleMethod.GetStateMachineImplementationType(out var stateMachineImplementationType);
            if (stateMachineType != StateMachineTypes.None && stateMachineImplementationType is not null && stateMachineImplementationType == generatedType)
            {
                realMethodInfo = possibleMethod;

                return stateMachineType;
            }
        }

        return StateMachineTypes.None;
    }

    private static StateMachineTypes GetStateMachineImplementationType(this MethodInfo methodInfo, out Type? stateMachineImplementationType)
    {
        stateMachineImplementationType = methodInfo.GetStateMachineImplementationTypeCore<AsyncStateMachineAttribute>();
        if(stateMachineImplementationType is not null)
        {
            return StateMachineTypes.Async;
        }

        stateMachineImplementationType = methodInfo.GetStateMachineImplementationTypeCore<IteratorStateMachineAttribute>();
        if (stateMachineImplementationType is not null)
        {
            return StateMachineTypes.Iterator;
        }

        stateMachineImplementationType = methodInfo.GetStateMachineImplementationTypeCore<AsyncIteratorStateMachineAttribute>();

        return stateMachineImplementationType is not null ? StateMachineTypes.AsyncIterator
                                                          : StateMachineTypes.None;
    }

    private static Type? GetStateMachineImplementationTypeCore<TStateMachineAttribute>(this MethodInfo methodInfo)
        where TStateMachineAttribute : StateMachineAttribute
    {
        return methodInfo.GetCustomAttribute<TStateMachineAttribute>()?.StateMachineType;
    }
}
