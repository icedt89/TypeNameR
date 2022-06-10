namespace JanHafner.TypeNameR.Benchmark;

public sealed class GenericTestClass<T>
{
    public sealed class InnerNonGenericTestClass
    {
        public sealed class MostInnerGenericTestClass<R, M>
        {
        }
    }
}