using JanHafner.TypeNameR.StackTrace;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace JanHafner.TypeNameR;

internal static class TypeHelper
{
    public static ReadOnlySpan<char> RemoveGenericParametersCount(this ReadOnlySpan<char> typeName)
    {
        var lastIndexOfGenericParameterDelimiter = typeName.IndexOf(Symbol.GraveAccent);
        if (lastIndexOfGenericParameterDelimiter > 0)
        {
            return typeName[..lastIndexOfGenericParameterDelimiter];
        }

        return typeName;
    }

    [ExcludeFromCodeCoverage]
    public static bool IsCompilerGenerated(this ICustomAttributeProvider customAttributeProvider) 
        => customAttributeProvider.IsDefined(typeof(CompilerGeneratedAttribute), false);

    [ExcludeFromCodeCoverage]
    public static bool IsExtension(this ICustomAttributeProvider customAttributeProvider) 
        => customAttributeProvider.IsDefined(typeof(ExtensionAttribute), false);

    [ExcludeFromCodeCoverage]
    public static StackFrameMetadata? GetExistingStackFrameMetadata(this StackFrame stackFrame)
    {
        if (!stackFrame.HasSource())
        {
            return null;
        }

        var fileName = stackFrame.GetFileName();
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new InvalidOperationException($"{nameof(StackFrame.GetFileName)} returned null, empty or whitespace");
        }
        
        var lineNumber = stackFrame.GetFileLineNumber();
        var columnNumber = stackFrame.GetFileColumnNumber();

        return new StackFrameMetadata(fileName, lineNumber, columnNumber);
    }
}
