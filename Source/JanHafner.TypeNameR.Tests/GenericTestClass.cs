namespace JanHafner.TypeNameR.Tests;

public sealed class GenericTestClass<T>
{
    public sealed class InnerNonGenericTestClass
    {
        public sealed class MostInnerGenericTestClass<R, M>
        {
        }
    }
}