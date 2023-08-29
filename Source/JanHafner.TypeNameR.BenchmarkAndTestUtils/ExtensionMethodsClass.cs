using System.Runtime.CompilerServices;

namespace JanHafner.TypeNameR.BenchmarkAndTestUtils;

public static class ExtensionMethodsClass
{
    public static void This([Expects("this int?[]? param1")] this int?[]? param1) => throw new NotImplementedException();
    
    public static void ThisRef([Expects("this ref int param1")] this ref int param1) => throw new NotImplementedException();
    
    public static void ThisIn([Expects("this in int param1")] this in int param1) => throw new NotImplementedException();
    
    [AsyncStateMachine(typeof(ExtensionMethodsClass))]
    public static Task MethodWithEveryParameterKeyword(ref string param1, out string param2, in string param3, string param4 = default, params string[] param5)
        => throw new NotImplementedException();
}