using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

/// <summary>
/// This benchmark compares different methods of passing different types of structs to methods.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
public class InstancePassingBenchmarks
{
    // | Method                              | Job      | Runtime  | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | Rank | Allocated | Alloc Ratio |
    // |------------------------------------ |--------- |--------- |----------:|----------:|----------:|----------:|------:|--------:|-----:|----------:|------------:|    
    // | PassReadOnlyRef                     | .NET 6.0 | .NET 6.0 | 0.0051 ns | 0.0107 ns | 0.0095 ns | 0.0000 ns |     ? |       ? |    1 |         - |           ? |    
    // | PassReadOnlyRefViaScopedIn          | .NET 6.0 | .NET 6.0 | 0.0717 ns | 0.0289 ns | 0.0659 ns | 0.0572 ns |     ? |       ? |    2 |         - |           ? |    
    // | PassReadOnlyRefViaRefReadonly       | .NET 6.0 | .NET 6.0 | 0.0744 ns | 0.0292 ns | 0.0634 ns | 0.0492 ns |     ? |       ? |    2 |         - |           ? |    
    // | PassReadOnlyRefViaIn                | .NET 6.0 | .NET 6.0 | 0.2363 ns | 0.0327 ns | 0.0589 ns | 0.2332 ns |     ? |       ? |    3 |         - |           ? |    
    // | PassReadOnlyRefViaScopedRefReadonly | .NET 6.0 | .NET 6.0 | 0.4784 ns | 0.0615 ns | 0.1793 ns | 0.4476 ns |     ? |       ? |    4 |         - |           ? |    
    // |                                     |          |          |           |           |           |           |       |         |      |           |             |
    // | PassReadOnlyRef                     | .NET 7.0 | .NET 7.0 | 0.0012 ns | 0.0045 ns | 0.0067 ns | 0.0000 ns |     ? |       ? |    1 |         - |           ? |    
    // | PassReadOnlyRefViaIn                | .NET 7.0 | .NET 7.0 | 0.2301 ns | 0.0242 ns | 0.0202 ns | 0.2235 ns |     ? |       ? |    2 |         - |           ? |    
    // | PassReadOnlyRefViaScopedIn          | .NET 7.0 | .NET 7.0 | 0.2522 ns | 0.0304 ns | 0.0270 ns | 0.2580 ns |     ? |       ? |    3 |         - |           ? |    
    // | PassReadOnlyRefViaScopedRefReadonly | .NET 7.0 | .NET 7.0 | 0.2598 ns | 0.0355 ns | 0.0349 ns | 0.2535 ns |     ? |       ? |    3 |         - |           ? |    
    // | PassReadOnlyRefViaRefReadonly       | .NET 7.0 | .NET 7.0 | 0.2823 ns | 0.0350 ns | 0.0417 ns | 0.2862 ns |     ? |       ? |    3 |         - |           ? |    
    // |                                     |          |          |           |           |           |           |       |         |      |           |             |    
    // | PassReadOnlyRef                     | .NET 8.0 | .NET 8.0 | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |     ? |       ? |    1 |         - |           ? |
    // | PassReadOnlyRefViaRefReadonly       | .NET 8.0 | .NET 8.0 | 0.3313 ns | 0.0345 ns | 0.0398 ns | 0.3347 ns |     ? |       ? |    2 |         - |           ? |    
    // | PassReadOnlyRefViaIn                | .NET 8.0 | .NET 8.0 | 0.3457 ns | 0.0376 ns | 0.0539 ns | 0.3423 ns |     ? |       ? |    2 |         - |           ? |    
    // | PassReadOnlyRefViaScopedIn          | .NET 8.0 | .NET 8.0 | 0.3572 ns | 0.0371 ns | 0.0412 ns | 0.3597 ns |     ? |       ? |    2 |         - |           ? |    
    // | PassReadOnlyRefViaScopedRefReadonly | .NET 8.0 | .NET 8.0 | 0.4377 ns | 0.0448 ns | 0.1271 ns | 0.4202 ns |     ? |       ? |    3 |         - |           ? |

    [Benchmark(Baseline = true)]
    public bool PassReadOnlyRef()
    {
        var readOnlyRefWithStringAndProperties = new ReadOnlyRef(@"C:\temp\dummy.bin", 1, 1);

        return PassReadOnlyRefLevel1(readOnlyRefWithStringAndProperties);
    }

    #region PassReadOnlyRef

    private bool PassReadOnlyRefLevel1(ReadOnlyRef readOnlyRef)
        => PassReadOnlyRefLevel2(readOnlyRef)
           && PassReadOnlyRefLevel2(readOnlyRef);

    private bool PassReadOnlyRefLevel2(ReadOnlyRef readOnlyRef)
        => PassReadOnlyRefLevel3(readOnlyRef)
           && PassReadOnlyRefLevel3(readOnlyRef)
           && PassReadOnlyRefLevel3(readOnlyRef);

    private bool PassReadOnlyRefLevel3(ReadOnlyRef readOnlyRef) => readOnlyRef.IsEmpty;

    #endregion

    [Benchmark]
    public bool PassReadOnlyRefViaIn()
    {
        var readOnlyRefWithStringAndProperties = new ReadOnlyRef(@"C:\temp\dummy.bin", 1, 1);

        return PassReadOnlyRefViaInLevel1(in readOnlyRefWithStringAndProperties);
    }

    #region PassReadOnlyRefViaIn

    private bool PassReadOnlyRefViaInLevel1(in ReadOnlyRef readOnlyRef)
        => PassReadOnlyRefViaInLevel2(in readOnlyRef)
           && PassReadOnlyRefViaInLevel2(in readOnlyRef);

    private bool PassReadOnlyRefViaInLevel2(in ReadOnlyRef readOnlyRef)
        => PassReadOnlyRefViaInLevel3(in readOnlyRef)
           && PassReadOnlyRefViaInLevel3(in readOnlyRef)
           && PassReadOnlyRefViaInLevel3(in readOnlyRef);

    private bool PassReadOnlyRefViaInLevel3(in ReadOnlyRef readOnlyRef) => readOnlyRef.IsEmpty;

    #endregion

    [Benchmark]
    public bool PassReadOnlyRefViaRefReadonly()
    {
        var readOnlyRefWithStringAndProperties = new ReadOnlyRef(@"C:\temp\dummy.bin", 1, 1);

        return PassReadOnlyRefViaRefReadonlyLevel1(in readOnlyRefWithStringAndProperties);
    }

    #region PassReadOnlyRefViaRefReadonly

    private bool PassReadOnlyRefViaRefReadonlyLevel1(ref readonly ReadOnlyRef readOnlyRef)
        => PassReadOnlyRefViaRefReadonlyLevel2(in readOnlyRef)
           && PassReadOnlyRefViaRefReadonlyLevel2(in readOnlyRef);

    private bool PassReadOnlyRefViaRefReadonlyLevel2(ref readonly ReadOnlyRef readOnlyRef)
        => PassReadOnlyRefViaRefReadonlyLevel3(in readOnlyRef)
           && PassReadOnlyRefViaRefReadonlyLevel3(in readOnlyRef)
           && PassReadOnlyRefViaRefReadonlyLevel3(in readOnlyRef);

    private bool PassReadOnlyRefViaRefReadonlyLevel3(ref readonly ReadOnlyRef readOnlyRef) => readOnlyRef.IsEmpty;

    #endregion

    [Benchmark]
    public bool PassReadOnlyRefViaScopedRefReadonly()
    {
        var readOnlyRefWithStringAndProperties = new ReadOnlyRef(@"C:\temp\dummy.bin", 1, 1);

        return PassReadOnlyRefViaScopedRefReadonlyLevel1(in readOnlyRefWithStringAndProperties);
    }

    #region PassReadOnlyRefViaScopedRefReadonly

    private bool PassReadOnlyRefViaScopedRefReadonlyLevel1(scoped ref readonly ReadOnlyRef readOnlyRef)
        => PassReadOnlyRefViaScopedRefReadonlyLevel2(in readOnlyRef)
           && PassReadOnlyRefViaScopedRefReadonlyLevel2(in readOnlyRef);

    private bool PassReadOnlyRefViaScopedRefReadonlyLevel2(scoped ref readonly ReadOnlyRef readOnlyRef)
        => PassReadOnlyRefViaScopedRefReadonlyLevel3(in readOnlyRef)
           && PassReadOnlyRefViaScopedRefReadonlyLevel3(in readOnlyRef)
           && PassReadOnlyRefViaScopedRefReadonlyLevel3(in readOnlyRef);

    private bool PassReadOnlyRefViaScopedRefReadonlyLevel3(scoped ref readonly ReadOnlyRef readOnlyRef) => readOnlyRef.IsEmpty;

    #endregion

    [Benchmark]
    public bool PassReadOnlyRefViaScopedIn()
    {
        var readOnlyRefWithStringAndProperties = new ReadOnlyRef(@"C:\temp\dummy.bin", 1, 1);

        return PassReadOnlyRefViaScopedInLevel1(in readOnlyRefWithStringAndProperties);
    }

    #region PassReadOnlyRefViaScopedRefReadonly

    private bool PassReadOnlyRefViaScopedInLevel1(scoped in ReadOnlyRef readOnlyRef)
        => PassReadOnlyRefViaScopedInLevel2(in readOnlyRef)
           && PassReadOnlyRefViaScopedInLevel2(in readOnlyRef);

    private bool PassReadOnlyRefViaScopedInLevel2(scoped in ReadOnlyRef readOnlyRef)
        => PassReadOnlyRefViaScopedInLevel3(in readOnlyRef)
           && PassReadOnlyRefViaScopedInLevel3(in readOnlyRef)
           && PassReadOnlyRefViaScopedInLevel3(in readOnlyRef);

    private bool PassReadOnlyRefViaScopedInLevel3(scoped in ReadOnlyRef readOnlyRef) => readOnlyRef.IsEmpty;

    #endregion

    private readonly ref struct ReadOnlyRef
    {
        public ReadOnlyRef(string? fileName, int lineNumber, int columnNumber)
        {
            FileName = fileName;
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
        }

        public int LineNumber { get; }

        public int ColumnNumber { get; }

        public bool IsEmpty => FileName is null;

        public string? FileName { get; }
    }
}