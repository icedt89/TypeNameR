using System.Reflection;

namespace JanHafner.TypeNameR.Helper;

internal static class EnumHelper
{
    public static bool IsSet(this NameRControlFlags flags, NameRControlFlags flag) => (flags & flag) == flag;

    public static bool IsSet(this StateMachineType flags, StateMachineType flag) => (flags & flag) == flag;

    // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
    public static bool IsSet(this MethodImplAttributes flags, MethodImplAttributes flag) => (flags & flag) == flag;
}