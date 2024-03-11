using BenchmarkDotNet.Jobs;

namespace JanHafner.TypeNameR.Benchmark;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
internal sealed class HintAttribute : Attribute
{
    public HintAttribute(string? hint)
    {
        Hint = hint;
    }

    public HintAttribute(string? hint, RuntimeMoniker runtimeMoniker)
    {
        Hint = hint;
        RuntimeMoniker = runtimeMoniker;
    }

    public string? Hint { get; }

    public RuntimeMoniker? RuntimeMoniker { get; }
}