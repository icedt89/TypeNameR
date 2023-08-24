using System.Reflection;
using System.Runtime.CompilerServices;

namespace JanHafner.TypeNameR.Helper;

internal static class StateMachineHelper
{
    public static StateMachineType ResolveRealMethodFromStateMachine(this MethodInfo methodInfo, out MethodInfo? realMethodInfo)
    {
        realMethodInfo = null;
        
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
            return StateMachineType.None;
        }

        return generatedType.ResolveRealMethodFromStateMachineType(out realMethodInfo);
    }

    private static StateMachineType ResolveRealMethodFromStateMachineType(this Type generatedType, out MethodInfo? realMethodInfo)
    {
        realMethodInfo = null;
        
        // We need the type in which the state machine resides
        var originalType = generatedType.DeclaringType;
        if (originalType is null)
        {
            return StateMachineType.None;
        }

        // ...and has a compiler generated annotation
        if (!generatedType.IsDefined(typeof(CompilerGeneratedAttribute), false))
        {
            return StateMachineType.None;
        }

        var possibleMethods = originalType.GetPossibleMethods().AsSpan();
        foreach (var possibleMethod in possibleMethods)
        {
            var stateMachineType = possibleMethod.GetStateMachineType(out var stateMachineImplementationType);
            if (stateMachineType != StateMachineType.None && stateMachineImplementationType is not null && stateMachineImplementationType == generatedType)
            {
                realMethodInfo = possibleMethod;

                return stateMachineType;
            }
        }

        return StateMachineType.None;
    }

    private static StateMachineType GetStateMachineType(this MethodInfo methodInfo, out Type? stateMachineImplementationType)
    {
        var stateMachineAttribute = methodInfo.GetCustomAttribute<StateMachineAttribute>(false);
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
