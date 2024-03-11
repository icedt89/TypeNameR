using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System.Reflection;

namespace JanHafner.TypeNameR.Benchmark;

internal sealed class HintColumn : IColumn
{
    public string GetValue(Summary summary, BenchmarkCase benchmarkCase) => GetHint(benchmarkCase) ?? string.Empty;

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style) => GetValue(summary, benchmarkCase);

    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;

    public bool IsAvailable(Summary summary) => true;

    public string Id => nameof(HintColumn);

    public string ColumnName => "Hint";

    public bool AlwaysShow => true;

    public ColumnCategory Category => ColumnCategory.Custom;

    public int PriorityInCategory => 0;

    public bool IsNumeric => false;

    public UnitType UnitType => UnitType.Dimensionless;

    public string Legend => "Provides a hint to the benchmark";

    private static string? GetHint(BenchmarkCase benchmarkCase)
    {
        HintAttribute? fallbackHintAttribute = null;
        foreach (var hintAttribute in benchmarkCase.Descriptor.WorkloadMethod.GetCustomAttributes<HintAttribute>())
        {
            if (hintAttribute.RuntimeMoniker is null)
            {
                fallbackHintAttribute = hintAttribute;
            }
            else if (hintAttribute.RuntimeMoniker == benchmarkCase.Job.Environment.Runtime.RuntimeMoniker)
            {
                return hintAttribute.Hint;
            }
        }

        return fallbackHintAttribute?.Hint;
    }
}