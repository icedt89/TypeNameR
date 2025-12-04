using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using JanHafner.TypeNameR.BenchmarkAndTestUtils;
using System.Diagnostics;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks.TypeNameR;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.Method)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net90)]
public class DisplayOfExceptionBenchmarks
{
    private Exception? catchedException;

    private ITypeNameR? typeNameR;

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        try
        {
            await StackTraceGenerator.CallRecursiveGenericMethodAsync<int>(stopAt: 100);
        }
        catch (Exception exception)
        {
            catchedException = exception;
        }

        typeNameR = GlobalBenchmarkSettings.TypeNameR ?? new JanHafner.TypeNameR.TypeNameR();
    }

    // [Benchmark]
    public string Exception_ToString() => catchedException!.ToString();

    // [Benchmark]
    public string Demystify() => catchedException!.Demystify().ToString();

    // Before using foreach:
    // | Method          | Job      | Runtime  | Mean       | Error    | StdDev    | Median     | Rank | Gen0    | Gen1    | Allocated |
    // |---------------- |--------- |--------- |-----------:|---------:|----------:|-----------:|-----:|--------:|--------:|----------:|
    // | GenerateDisplay | .NET 6.0 | .NET 6.0 | 1,788.1 us | 60.24 us | 176.68 us | 1,693.2 us |    3 | 82.0313 | 17.5781 | 508.95 KB |
    // | GenerateDisplay | .NET 7.0 | .NET 7.0 |   957.7 us | 22.51 us |  66.37 us |   929.9 us |    2 | 94.7266 | 23.4375 | 583.68 KB |
    // | GenerateDisplay | .NET 8.0 | .NET 8.0 |   865.3 us | 16.60 us |  16.30 us |   871.5 us |    1 | 89.8438 | 19.5313 | 566.38 KB |

    [Benchmark]
    public string GenerateDisplay()
        => typeNameR!.GenerateDisplay(catchedException!, NameRControlFlags.All
                                                       | NameRControlFlags.DontEliminateRecursiveStackFrames);
}