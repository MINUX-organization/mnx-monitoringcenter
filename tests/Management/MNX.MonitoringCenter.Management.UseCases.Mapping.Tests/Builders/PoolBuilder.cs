namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders;

using Cryptocurrency = Core.Mining.Cryptocurrency;
using Pool = Core.Mining.Pool;

public class PoolBuilder
{
    private static int _counter = 1;

    private Guid _id = Guid.NewGuid();
    private string _domain = $"www.pool-domain{_counter++}";
    private int _port = 8080;
    private bool _tls = false;
    private Guid? _ownerId = null;
    private Guid _cryptocurrencyId = Guid.NewGuid();
    private Cryptocurrency? _cryptocurrency = null;

    public PoolBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public PoolBuilder WithDomain(string domain)
    {
        _domain = domain;
        return this;
    }

    public PoolBuilder WithPort(int port)
    {
        _port = port;
        return this;
    }

    public PoolBuilder WithTls()
    {
        _tls = true;
        return this;
    }

    public PoolBuilder WithOwner(Guid? ownerId = null)
    {
        _ownerId = ownerId ?? Guid.NewGuid();
        return this;
    }

    public PoolBuilder WithCryptocurrency(Action<CryptocurrencyBuilder>? configure = null)
    {
        var builder = new CryptocurrencyBuilder();
        configure?.Invoke(builder);
        _cryptocurrency = builder.Build();
        _cryptocurrencyId = _cryptocurrency.Id;
        return this;
    }

    public Pool Build()
    {
        return new Pool
        {
            Id = _id,
            Domain = _domain,
            Port = _port,
            Tls = _tls,
            OwnerId = _ownerId,
            CryptocurrencyId = _cryptocurrencyId,
            Cryptocurrency = _cryptocurrency,
        };
    }
}
