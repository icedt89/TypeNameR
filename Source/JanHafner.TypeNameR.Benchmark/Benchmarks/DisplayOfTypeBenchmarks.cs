using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Method)]
// [SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
// [SimpleJob(RuntimeMoniker.Net80)]
public class DisplayOfTypeBenchmarks
{
    private Type simpleType = default!;

    private Type simpleNestedNestedType = default!;

    private Type genericSimpleType = default!;

    private Type genericComplexNestedNestedType = default!;

    private TypeNameR typeNameR;

    [GlobalSetup]
    public void GlobalSetup()
    {
        simpleType = typeof(TestClass);
        simpleNestedNestedType = typeof(TestClass.InnerTestClass.MostInnerTestClass);
        genericSimpleType = typeof(GenericTestClass<int>);
        genericComplexNestedNestedType =
            typeof(GenericTestClass<string[,,]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<KeyValuePair<string, double>>, object[,][]>[]
                [,,]);

        typeNameR = new();
    }

    [Benchmark]
    public void SimpleType_TypeName() => typeNameR.GenerateDisplay(simpleType, false, null);
    
    [Benchmark]
    public void SimpleType_FullTypeName() => typeNameR.GenerateDisplay(simpleType, true, null);

    [Benchmark]
    public void SimpleNestedNestedType_TypeName() => typeNameR.GenerateDisplay(simpleNestedNestedType, false, null);
    
    [Benchmark]
    public void SimpleNestedNestedType_FullTypeName() => typeNameR.GenerateDisplay(simpleNestedNestedType, true, null);
    
    [Benchmark]
    public void GenericSimpleType_TypeName() => typeNameR.GenerateDisplay(genericSimpleType, false, null);
    
    [Benchmark]
    public void GenericSimpleType_FullTypeName() => typeNameR.GenerateDisplay(genericSimpleType, true, null);

    [Benchmark]
    public void GenericComplexNestedNestedType_TypeName() => typeNameR.GenerateDisplay(genericComplexNestedNestedType, false, null);
    
    [Benchmark]
    public void GenericComplexNestedNestedType_FullTypeName() => typeNameR.GenerateDisplay(genericComplexNestedNestedType, true, null);

    // [Benchmark]
    public void SimpleType_Name()
    {
        var _ = simpleType.Name;
    }
    
    // [Benchmark]
    public void SimpleType_FullName()
    {
        var _ = simpleType.FullName;
    }
    
    // [Benchmark]
    public void SimpleNestedNestedType_Name()
    {
        var _ = simpleNestedNestedType.Name;
    }

    // [Benchmark]
    public void SimpleNestedNestedType_FullName()
    {
        var _ = simpleNestedNestedType.FullName;
    }
    
    // [Benchmark]
    public void GenericSimpleType_Name()
    {
        var _ = genericSimpleType.Name;
    }

    // [Benchmark]
    public void GenericSimpleType_FullName()
    {
        var _ = genericSimpleType.FullName;
    }
    
    // [Benchmark]
    public void GenericComplexNestedNestedType_Name()
    {
        var _ = genericComplexNestedNestedType.Name;
    }

    // [Benchmark]
    public void GenericComplexNestedNestedType_FullName()
    {
        var _ = genericComplexNestedNestedType.FullName;
    }
}
