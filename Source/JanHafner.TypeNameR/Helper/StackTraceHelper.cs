using JanHafner.TypeNameR.StackTrace;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace JanHafner.TypeNameR.Helper;

internal static class StackTraceHelper
{
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MethodInfo[] GetPossibleMethods(this Type type)
        => type.GetMethods(BindingFlags.Public
                           | BindingFlags.NonPublic
                           | BindingFlags.Static
                           | BindingFlags.Instance
                           | BindingFlags.DeclaredOnly);
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsStackTraceHidden(this MemberInfo member)
        => member.IsDefined(typeof(StackTraceHiddenAttribute), false);
    
    public static bool IsHidden(this MethodBase method) 
        => method.MethodImplementationFlags.HasFlag(MethodImplAttributes.AggressiveInlining)
           || method.IsStackTraceHidden()
           || (method.DeclaringType?.IsStackTraceHidden() ?? false);
    
    public static bool IsSkippable(this MethodBase methodBase, NameRControlFlags nameRControlFlags, string[] namespaces)
        => (!nameRControlFlags.HasFlag(NameRControlFlags.IncludeHiddenStackFrames)
           && methodBase.IsHidden()) || (nameRControlFlags.HasFlag(NameRControlFlags.ExcludeStackFrameMethodsByNamespace)
                                                                   && methodBase.IsNamespaceExcluded(namespaces));

    public static bool IsNamespaceExcluded(this MethodBase methodBase, ReadOnlySpan<string> namespaces)
    {
        if (namespaces.Length == 0)
        {
            return false;
        }
        
        ReadOnlySpan<char> declaringTypeNamespace = methodBase.DeclaringType?.Namespace;
        if(declaringTypeNamespace.Length == 0)
        {
            return false;
        }

        foreach (var @namespace in namespaces)
        {
            if (declaringTypeNamespace.StartsWith(@namespace, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    public static (StackFrame StackFrame, MethodBase? Method, uint CallDepth)[] FlattenRecursionAndFilterUnnecessaryStackFrames(this Span<StackFrame> stackFrames, NameRControlFlags nameRControlFlags, string[] namespaces)
    {
        var recursiveStackFrames = new Dictionary<object, (StackFrame StackFrame, MethodBase? Method, uint CallDepth)>(stackFrames.Length);
        foreach (var stackFrame in stackFrames)
        {
            var stackFrameMethod = stackFrame.GetMethod();
            if (stackFrameMethod is not null && stackFrameMethod.IsSkippable(nameRControlFlags, namespaces))
            {
                continue;
            }

            // To process stack frames which have no method it is necessary to use another key for the dictionary
            var key = stackFrameMethod ?? (object)stackFrame;
            
            ref var valOrNew = ref CollectionsMarshal.GetValueRefOrAddDefault(recursiveStackFrames, key, out var exists);
            if (!exists)
            {
                valOrNew.StackFrame = stackFrame;
                valOrNew.Method = stackFrameMethod;
                valOrNew.CallDepth = 1;
            }
            else
            {
                valOrNew.CallDepth++;
            }
        }
        
        return recursiveStackFrames.Values.ToArray();
    }

    public static StackFrameMetadata? GetExistingStackFrameMetadata(this StackFrame stackFrame)
    {
        var fileName = stackFrame.GetFileName();
        if (fileName is null || fileName.Length == 0)
        {
            return null;
        }
        
        var lineNumber = stackFrame.GetFileLineNumber();
        var columnNumber = stackFrame.GetFileColumnNumber();

        return new StackFrameMetadata(fileName, lineNumber, columnNumber);
    }
}
