namespace JanHafner.TypeNameR.Tests;

public readonly struct GenericTestStruct<T>
{
    public readonly struct InnerNonGenericTestStruct
    {
        public readonly struct MostInnerGenericTestStruct<R, M>
        {
        }
    }
}