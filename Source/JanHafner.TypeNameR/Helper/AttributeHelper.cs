using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace JanHafner.TypeNameR.Helper;

internal static class AttributeHelper
{
    public static bool HasStackTraceHiddenAttribute(this MemberInfo member) => member.IsDefined(typeof(StackTraceHiddenAttribute), false);

    public static bool HasDynamicAttribute(this ParameterInfo parameter) => parameter.IsDefined(typeof(DynamicAttribute), false);

    public static bool HasCompilerGeneratedAttribute(this Type type) => type.IsDefined(typeof(CompilerGeneratedAttribute), false);

    public static bool HasExtensionAttribute(this MemberInfo member) => member.IsDefined(typeof(ExtensionAttribute), false);

    public static bool HasParamArrayAttribute(this ParameterInfo parameter) => parameter.IsDefined(typeof(ParamArrayAttribute), false);

    public static StateMachineAttribute? FindStateMachineAttribute(this MethodInfo method) => method.GetCustomAttribute<StateMachineAttribute>(false);

    [Obsolete("Use this method")]
    public static TupleElementNamesAttribute? FindTupleElementNamesAttribute(this ParameterInfo parameter) => parameter.GetCustomAttribute<TupleElementNamesAttribute>(false);
}