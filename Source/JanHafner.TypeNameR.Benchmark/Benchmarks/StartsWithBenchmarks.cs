using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace JanHafner.TypeNameR.Benchmark.Benchmarks;

/// <summary>
/// This benchmark compares different methods of starts-with checks.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[HintColumn]
public class StartsWithBenchmarks
{
    // | Method                                    | Job      | Runtime  | Mean      | Error     | StdDev    | Ratio | RatioSD | Rank | Hint | Allocated | Alloc Ratio |
    // |------------------------------------------ |--------- |--------- |----------:|----------:|----------:|------:|--------:|-----:|----- |----------:|------------:|
    // | OrdinalWithLengthCheckUsingSpan           | .NET 6.0 | .NET 6.0 | 12.247 ns | 0.2266 ns | 0.2009 ns |  0.82 |    0.02 |    1 |      |         - |          NA |
    // | OrdinalWithLengthCheck                    | .NET 6.0 | .NET 6.0 | 12.345 ns | 0.0687 ns | 0.0573 ns |  0.83 |    0.02 |    1 |      |         - |          NA |
    // | OrdinalUsingSpan                          | .NET 6.0 | .NET 6.0 | 14.032 ns | 0.2778 ns | 0.3088 ns |  0.95 |    0.04 |    2 |      |         - |          NA |
    // | Ordinal                                   | .NET 6.0 | .NET 6.0 | 14.691 ns | 0.3216 ns | 0.4402 ns |  1.00 |    0.00 |    3 |      |         - |          NA |
    // | OrdinalIgnoreCaseWithLengthCheck          | .NET 6.0 | .NET 6.0 | 19.508 ns | 0.2932 ns | 0.2743 ns |  1.30 |    0.04 |    4 |      |         - |          NA |
    // | OrdinalIgnoreCaseWithLengthCheckUsingSpan | .NET 6.0 | .NET 6.0 | 19.692 ns | 0.2433 ns | 0.2157 ns |  1.32 |    0.04 |    4 |      |         - |          NA |
    // | OrdinalIgnoreCaseUsingSpan                | .NET 6.0 | .NET 6.0 | 22.136 ns | 0.0754 ns | 0.0589 ns |  1.48 |    0.03 |    5 |      |         - |          NA |
    // | OrdinalIgnoreCase                         | .NET 6.0 | .NET 6.0 | 22.619 ns | 0.4311 ns | 0.4032 ns |  1.51 |    0.02 |    6 |      |         - |          NA |
    // |                                           |          |          |           |           |           |       |         |      |      |           |             |
    // | OrdinalWithLengthCheck                    | .NET 7.0 | .NET 7.0 | 10.716 ns | 0.0391 ns | 0.0346 ns |  0.82 |    0.00 |    1 |      |         - |          NA |
    // | OrdinalWithLengthCheckUsingSpan           | .NET 7.0 | .NET 7.0 | 10.754 ns | 0.1208 ns | 0.1130 ns |  0.83 |    0.01 |    1 |      |         - |          NA |
    // | OrdinalUsingSpan                          | .NET 7.0 | .NET 7.0 | 12.700 ns | 0.2238 ns | 0.2093 ns |  0.98 |    0.02 |    2 |      |         - |          NA |
    // | Ordinal                                   | .NET 7.0 | .NET 7.0 | 13.001 ns | 0.0544 ns | 0.0482 ns |  1.00 |    0.00 |    3 |      |         - |          NA |
    // | OrdinalIgnoreCaseWithLengthCheckUsingSpan | .NET 7.0 | .NET 7.0 | 17.341 ns | 0.0808 ns | 0.0631 ns |  1.33 |    0.01 |    4 |      |         - |          NA |
    // | OrdinalIgnoreCaseWithLengthCheck          | .NET 7.0 | .NET 7.0 | 17.393 ns | 0.1564 ns | 0.1306 ns |  1.34 |    0.01 |    4 |      |         - |          NA |
    // | OrdinalIgnoreCase                         | .NET 7.0 | .NET 7.0 | 23.760 ns | 0.2397 ns | 0.2242 ns |  1.83 |    0.02 |    5 |      |         - |          NA |
    // | OrdinalIgnoreCaseUsingSpan                | .NET 7.0 | .NET 7.0 | 24.353 ns | 0.4381 ns | 0.4098 ns |  1.87 |    0.03 |    6 |      |         - |          NA |
    // |                                           |          |          |           |           |           |       |         |      |      |           |             |
    // | OrdinalWithLengthCheckUsingSpan           | .NET 8.0 | .NET 8.0 |  8.494 ns | 0.1824 ns | 0.1617 ns |  1.00 |    0.02 |    1 |      |         - |          NA |
    // | Ordinal                                   | .NET 8.0 | .NET 8.0 |  8.528 ns | 0.0977 ns | 0.0816 ns |  1.00 |    0.00 |    1 |      |         - |          NA |
    // | OrdinalWithLengthCheck                    | .NET 8.0 | .NET 8.0 |  8.565 ns | 0.0787 ns | 0.0736 ns |  1.00 |    0.01 |    1 |      |         - |          NA |
    // | OrdinalUsingSpan                          | .NET 8.0 | .NET 8.0 |  8.738 ns | 0.0286 ns | 0.0267 ns |  1.02 |    0.01 |    2 |      |         - |          NA |
    // | OrdinalIgnoreCaseWithLengthCheckUsingSpan | .NET 8.0 | .NET 8.0 | 15.475 ns | 0.3319 ns | 0.4653 ns |  1.82 |    0.06 |    3 |      |         - |          NA |
    // | OrdinalIgnoreCaseWithLengthCheck          | .NET 8.0 | .NET 8.0 | 15.728 ns | 0.1971 ns | 0.1844 ns |  1.84 |    0.02 |    3 |      |         - |          NA |
    // | OrdinalIgnoreCase                         | .NET 8.0 | .NET 8.0 | 16.875 ns | 0.2901 ns | 0.2713 ns |  1.97 |    0.04 |    4 |      |         - |          NA |
    // | OrdinalIgnoreCaseUsingSpan                | .NET 8.0 | .NET 8.0 | 16.949 ns | 0.1254 ns | 0.1047 ns |  1.99 |    0.02 |    4 |      |         - |          NA |

    private string needle;

    private string[] array;

    [GlobalSetup]
    public void GlobalSetup()
    {
        needle = "JanHafner.Huf";
        array =
        [
            "System",
            "System.Runtime",
            "Microsoft",
            "Microsoft.EntityFrameworkCore",
            "JanHafner",
            "JanHafner.Huf.Test",
            "AspNetCore",
            "AspNetCore.Internals"
        ];
    }

    [Benchmark(Baseline = true)]
    public bool Ordinal()
    {
        foreach (var item in array)
        {
            if (needle.StartsWith(item, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    [Benchmark]
    public bool OrdinalWithLengthCheck()
    {
        foreach (var item in array)
        {
            if (needle.Length <= item.Length)
            {
                continue;
            }

            if (needle.StartsWith(item, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    [Benchmark]
    public bool OrdinalUsingSpan()
    {
        foreach (var item in array)
        {
            if (needle.StartsWith(item, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    [Benchmark]
    public bool OrdinalWithLengthCheckUsingSpan()
    {
        foreach (var item in array)
        {
            if (needle.Length <= item.Length)
            {
                continue;
            }

            if (needle.StartsWith(item, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    [Benchmark]
    public bool OrdinalIgnoreCase()
    {
        foreach (var item in array)
        {
            if (needle.StartsWith(item, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    [Benchmark]
    public bool OrdinalIgnoreCaseWithLengthCheck()
    {
        foreach (var item in array)
        {
            if (needle.Length <= item.Length)
            {
                continue;
            }

            if (needle.StartsWith(item, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    [Benchmark]
    public bool OrdinalIgnoreCaseUsingSpan()
    {
        foreach (var item in array)
        {
            if (needle.StartsWith(item, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    [Benchmark]
    public bool OrdinalIgnoreCaseWithLengthCheckUsingSpan()
    {
        foreach (var item in array)
        {
            if (needle.Length <= item.Length)
            {
                continue;
            }

            if (needle.StartsWith(item, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}