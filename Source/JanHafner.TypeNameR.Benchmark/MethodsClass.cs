namespace JanHafner.TypeNameR.Benchmark;

public sealed class MethodsClass
{
    public void Call1() => throw new NotImplementedException();

    public Task<int?> Get16(int? int1 = default, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public async Task<bool?> Get18(int[,] int1, string? identifier = null, CancellationToken cancellationToken = default)
    {
        var i = await Get18_2(identifier, cancellationToken);

        return i is 1;
    }

    private async Task<int> Get18_2(string identifier, CancellationToken cancellationToken) => await Get18_3(identifier, cancellationToken);

    private async Task<int> Get18_3(string identifier, CancellationToken cancellationToken) => throw new InvalidOperationException(identifier);
}
