using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace JanHafner.TypeNameR.Benchmark;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
[SimpleJob(RuntimeMoniker.Net70)]
// [SimpleJob(RuntimeMoniker.Net80)] TODO
public class TypeNamingBenchmarks
{
    private Type simpleType = default!;

    private Type simpleNestedNestedType = default!;

    private Type genericSimpleType = default!;

    private Type genericComplexNestedNestedType = default!;

    private TypeNameR typeNameR = default!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        simpleType = typeof(TestClass);
        simpleNestedNestedType = typeof(TestClass.InnerTestClass.MostInnerTestClass);
        genericSimpleType = typeof(GenericTestClass<int>);
        genericComplexNestedNestedType = typeof(GenericTestClass<string[,,]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<KeyValuePair<string, double>>, object[,][]>[][,,]);

        typeNameR = new();
    }

    [Benchmark]
    public void SimpleType() => typeNameR.ExtractReadable(simpleType, true);

    [Benchmark]
    public void SimpleNestedNestedType() => typeNameR.ExtractReadable(simpleNestedNestedType, true);

    [Benchmark]
    public void GenericSimpleType() => typeNameR.ExtractReadable(genericSimpleType, true);

    [Benchmark]
    public void GenericComplexNestedNestedType() => typeNameR.ExtractReadable(genericComplexNestedNestedType, true);

    [Benchmark(Baseline = true)]
    public void SimpleType_FullName()
    {
        var name = simpleType.FullName;
    }

    [Benchmark]
    public void SimpleNestedNestedType_FullName()
    {
        var name = simpleNestedNestedType.FullName;
    }

    [Benchmark]
    public void GenericSimpleType_FullName()
    {
        var name = genericSimpleType.FullName;
    }

    [Benchmark]
    public void GenericComplexNestedNestedType_FullName()
    {
        var name = genericComplexNestedNestedType.FullName;
    }
}
