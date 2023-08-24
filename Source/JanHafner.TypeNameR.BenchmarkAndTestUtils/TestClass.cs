namespace JanHafner.TypeNameR.BenchmarkAndTestUtils;

public sealed class TestClass
{
    public static void NonGenericMethod() => throw new NotImplementedException();
    
    public static void GenericMethod<T, K>() => throw new NotImplementedException();
    
    public sealed class InnerTestClass
    {
        public sealed class MostInnerTestClass
        {
        }
    }
}