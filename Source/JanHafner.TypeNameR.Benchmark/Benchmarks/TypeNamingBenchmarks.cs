using BenchmarkDotNet.Attributes;

namespace JanHafner.TypeNameR.Benchmark;

[MemoryDiagnoser]
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
        this.simpleType = typeof(TestClass);
        this.simpleNestedNestedType = typeof(TestClass.InnerTestClass.MostInnerTestClass);
        this.genericSimpleType = typeof(GenericTestClass<int>);
        this.genericComplexNestedNestedType = typeof(GenericTestClass<string[,,]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<KeyValuePair<string, double>>, object[,][]>[][,,]);

        this.typeNameR = new();
    }

    [Benchmark]
    public void SimpleType()
    {
        this.typeNameR.ExtractReadable(this.simpleType);
    }

    [Benchmark]
    public void SimpleNestedNestedType()
    {
        this.typeNameR.ExtractReadable(this.simpleNestedNestedType);
    }

    [Benchmark]
    public void GenericSimpleType()
    {
        this.typeNameR.ExtractReadable(this.genericSimpleType);
    }

    [Benchmark]
    public void GenericComplexNestedNestedType()
    {
        this.typeNameR.ExtractReadable(this.genericComplexNestedNestedType);
    }

    [Benchmark(Baseline = true)]
    public void SimpleType_FullName()
    {
        var name = this.simpleType.FullName;
    }

    [Benchmark]
    public void SimpleNestedNestedType_FullName()
    {
        var name = this.simpleNestedNestedType.FullName;
    }

    [Benchmark]
    public void GenericSimpleType_FullName()
    {
        var name = this.genericSimpleType.FullName;
    }

    [Benchmark]
    public void GenericComplexNestedNestedType_FullName()
    {
        var name = this.genericComplexNestedNestedType.FullName;
    }
}
