using BenchmarkDotNet.Attributes;

namespace JanHafner.TypeNameR.Benchmark;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class HintColumnAttribute : ColumnConfigBaseAttribute
{
    public HintColumnAttribute() : base(new HintColumn())
    {
    }
}