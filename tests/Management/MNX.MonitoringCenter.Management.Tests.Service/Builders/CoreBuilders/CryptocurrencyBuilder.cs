using MNX.MonitoringCenter.Management.Core.Mining;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

public class CryptocurrencyBuilder
{
    private static int _counter = 1;

    private Guid _id = Guid.NewGuid();
    private string _fullName = $"Crypto_{_counter}";
    private string _shortName = $"C_{_counter++}";
    private Guid _algorithmId = Guid.NewGuid();
    private Guid? _ownerId = null;
    private Algorithm? _algorithm = null;

    public CryptocurrencyBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public CryptocurrencyBuilder WithFullName(string fullName)
    {
        _fullName = fullName;
        return this;
    }

    public CryptocurrencyBuilder WithShortName(string shortName)
    {
        _shortName = shortName;
        return this;
    }

    public CryptocurrencyBuilder WithOwner(Guid? ownerId = null)
    {
        _ownerId = ownerId ?? Guid.NewGuid();
        return this;
    }

    public CryptocurrencyBuilder WithAlgorithm(Action<AlgorithmBuilder>? configure = null)
    {
        var builder = new AlgorithmBuilder();
        configure?.Invoke(builder);
        _algorithm = builder.Build();
        _algorithmId = _algorithm.Id;
        return this;
    }

    public Cryptocurrency Build()
    {
        return new Cryptocurrency
        {
            Id = _id,
            FullName = _fullName,
            ShortName = _shortName,
            AlgorithmId = _algorithmId,
            OwnerId = _ownerId,
            Algorithm = _algorithm
        };
    }
}
