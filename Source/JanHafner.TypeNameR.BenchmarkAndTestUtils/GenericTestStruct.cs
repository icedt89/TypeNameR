namespace JanHafner.TypeNameR.BenchmarkAndTestUtils;

public readonly struct GenericTestStruct<T>
{
    public readonly struct InnerNonGenericTestStruct
    {
        public readonly struct MostInnerGenericTestStruct<R, M>
        {
        }
    }
    
    public readonly struct InnerGenericTestStruct<K>
    {
        public readonly struct MostInnerGenericTestStruct<R, M>
        {
        }
    }
}

public readonly struct NonGenericTestStruct
{
    public readonly struct InnerNonGenericTestStruct
    {
        public readonly struct MostInnerGenericTestStruct<R, M>
        {
        }
    }
    
    public readonly struct InnerGenericTestStruct<K, M>
    {
        public readonly struct MostInnerNonGenericTestStruct
        {
        }
    }
}