namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

using Cryptocurrency = Core.Mining.Cryptocurrency;
using Pool = Core.Mining.Pool;

public class PoolBuilder
{
    private static int _counter = 1;

    protected Guid _id = Guid.NewGuid();
    protected string _domain = $"www.pool-domain{_counter++}";
    protected int _port = 8080;
    protected bool _tls = false;
    protected Guid? _ownerId = null;
    protected Guid _cryptocurrencyId = Guid.NewGuid();
    protected Cryptocurrency? _cryptocurrency = null;

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

    public PoolBuilder WithTls(bool tls = true)
    {
        _tls = tls;
        return this;
    }

    public PoolBuilder WithOwner(Guid? ownerId = null)
    {
        _ownerId = ownerId ?? Guid.NewGuid();
        return this;
    }

    public PoolBuilder WithCryptocurrency(Func<CryptocurrencyBuilder, CryptocurrencyBuilder> configure)
    {
        var builder = new CryptocurrencyBuilder();
        builder = configure(builder);
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
