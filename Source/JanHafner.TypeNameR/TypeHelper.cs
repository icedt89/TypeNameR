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
        var lastIndexOfGenericParamterDelimiter = typeName.LastIndexOf(Symbol.GraveAccent);
        if (lastIndexOfGenericParamterDelimiter > 0)
        {
            return typeName[..lastIndexOfGenericParamterDelimiter];
        }

        return typeName;
    }

    [ExcludeFromCodeCoverage]
    public static bool IsCompilerGenerated(this ICustomAttributeProvider customAttributeProvider)
    {
        return customAttributeProvider.IsDefined(typeof(CompilerGeneratedAttribute), false);
    }

    [ExcludeFromCodeCoverage]
    public static bool IsExtension(this ICustomAttributeProvider customAttributeProvider)
    {
        return customAttributeProvider.IsDefined(typeof(ExtensionAttribute), false);
    }

    [ExcludeFromCodeCoverage]
    public static StackFrameMetadata? GetExistingStackFrameMetadata(this StackFrame stackFrame)
    {
        if (!stackFrame.HasSource())
        {
            return null;
        }

        var fileName = stackFrame.GetFileName();
        var lineNumber = stackFrame.GetFileLineNumber();
        var columnNumber = stackFrame.GetFileColumnNumber();

        return new StackFrameMetadata(fileName!, lineNumber, columnNumber);
    }
}
