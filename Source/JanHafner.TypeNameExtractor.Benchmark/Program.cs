using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace JanHafner.TypeNameExtractor.Benchmark;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<Benchmarks>();
    }

    [MemoryDiagnoser]
    public class Benchmarks
    {
        private static readonly Type Type = typeof(GenericTestClass<string[,]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[,][]>[][,,]);

        private static readonly ITypeNameExtractor TypeNameExtractor = new TypeNameExtractor();

        [Benchmark]
        public void Use() 
        {
            TypeNameExtractor.ExtractReadableName(Type);
        }
    }
}