namespace JanHafner.TypeNameExtractor.Tests;

#pragma warning disable S2326 // Unused type parameters should be removed
public sealed class GenericTestClass<T>
{
    public sealed class InnerNonGenericTestClass
    {
        public sealed class MostInnerGenericTestClass<R, M>
        {
        }
    }
}
#pragma warning restore S2326 // Unused type parameters should be removed