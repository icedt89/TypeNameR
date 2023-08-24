namespace JanHafner.TypeNameR.BenchmarkAndTestUtils;

public class GenericTestClass<T>
{
    public sealed class InnerNonGenericTestClass
    {
        public sealed class MostInnerGenericTestClass<R, M>
        {
        }
    }
}

public sealed class InheritedGenericTestClass : GenericTestClass<string>
{
}