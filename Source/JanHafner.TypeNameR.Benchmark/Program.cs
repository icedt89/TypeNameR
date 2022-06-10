using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using JanHafner.TypeNameR.StackTrace;
using System.Diagnostics;
using System.IO.Abstractions;
using System.Reflection;
using System.Text;

namespace JanHafner.TypeNameR.Benchmark;

public class Program
{
    public static void Main(string[] args)
    {
        //BenchmarkRunner.Run<NamingBenchmarks>();
        BenchmarkRunner.Run<ExceptionBenchmarks>();
        //BenchmarkRunner.Run<StringBuilderBenchmarks>();
    }

    [MemoryDiagnoser]
    public class StringBuilderBenchmarks
    {
        private string Part1;

        private int Part2;

        private bool Part3;

        private string Part4;

        [GlobalSetup]
        public void GlobalSetup()
        {
            Part1 = Guid.NewGuid().ToString();
            Part2 = 23423;
            Part3 = true;
            Part4 = Guid.NewGuid().ToString();
        }

        [Benchmark(Baseline = true)]
        public void Append()
        {
            new StringBuilder().Append(Part1)
                               .Append(Part2)
                               .Append(Part3)
                               .Append(Part4);
        }

        [Benchmark]
        public void AppendFormat()
        {
            new StringBuilder().AppendFormat("{0} {1} {2} {3}", Part1, Part2, Part3, Part4);
        }
    }

    [MemoryDiagnoser]
    public class NamingBenchmarks
    {
        private Type Type;

        private MethodInfo MethodInfo;

        private TypeNameR TypeNameR;

        [GlobalSetup]
        public async Task GlobalSetup()
        {
            this.Type = typeof(GenericTestClass<string[,]>.InnerNonGenericTestClass.MostInnerGenericTestClass<IReadOnlyList<double?>, object[,][]>[][,,]);
            this.TypeNameR = new();
            this.MethodInfo = typeof(MethodsClass).GetMethod("Get16") ?? throw new InvalidOperationException();
        }

        [Benchmark]
        public void ExtractReadableTypeName()
        {
            TypeNameR.ExtractReadable(Type);
        }

        [Benchmark]
        public void ExtractReadableMethodName()
        {
            TypeNameR.ExtractReadable(MethodInfo, NameRControlFlags.All);
        }
    }

    [MemoryDiagnoser]
    public class ExceptionBenchmarks
    {
        private Exception catchedException;

        private TypeNameR TypeNameR_WithPdbLocator;

        [GlobalSetup]
        public async Task GlobalSetup()
        {
            this.TypeNameR_WithPdbLocator = new(new StackFrameMetadataProvider(new PdbLocator(new FileSystem()), new FileSystem()));

            try
            {
                int? int1 = 0;
                await StackTraceGenerator.CallRecursivGenericMethod<int>(ref int1);
            }
            catch (Exception exception)
            {
                this.catchedException = exception;
            }
        }

        [Benchmark]
        public void RewriteExceptionStackTraces()
        {
            TypeNameR_WithPdbLocator.RewriteStackTrace(catchedException, NameRControlFlags.All
                                                                      & ~NameRControlFlags.IncludeHiddenStackFrames
                                                                      & ~NameRControlFlags.IncludeAccessModifier
                                                                      & ~NameRControlFlags.IncludeStaticModifier
                                                                      & ~NameRControlFlags.IncludeParameterDefaultValue
                                                                      & ~NameRControlFlags.StoreOriginalStackTraceInExceptionData);
        }

        [Benchmark(Baseline = true)]
        public void RewriteExceptionStackTraces_WithBen()
        {
            catchedException.Demystify();
        }
    }
}