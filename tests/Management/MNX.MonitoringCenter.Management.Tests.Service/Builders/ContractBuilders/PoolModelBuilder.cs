using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;

public class PoolModelBuilder
{
    private static int _counter = 1;

    private Guid _id = Guid.NewGuid();
    private string _domain = $"www.pool-model-domain{_counter++}";
    private string _cryptocurrency = $"CryptocurrencyName_{_counter}";
    private Guid _cryptocurrencyId = Guid.NewGuid();
    private Guid? _ownerId = null;
    private int _port = 8080 + _counter++;
    private bool _tls = false;

    public PoolModelBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public PoolModelBuilder WithDomain(string domain)
    {
        _domain = domain;
        return this;
    }
    
    public PoolModelBuilder WithCryptocurrencyName(string cryptocurrencyName)
    {
        _cryptocurrency = cryptocurrencyName;
        return this;
    }
    
    public PoolModelBuilder WithCryptocurrencyId(Guid cryptocurrencyId)
    {
        _cryptocurrencyId = cryptocurrencyId;
        return this;
    }
    
    public PoolModelBuilder WithOwner(Guid? ownerId = null)
    {
        _ownerId = ownerId ?? Guid.NewGuid();
        return this;
    }
    
    public PoolModelBuilder WithPort(int port)
    {
        _port = port;
        return this;
    }

    public PoolModelBuilder WithTls(bool tls = true)
    {
        _tls = tls;
        return this;
    }

    public PoolModel Build()
    {
        return new PoolModel
        {
            Id = _id,
            Domain = _domain,
            Cryptocurrency = _cryptocurrency,
            CryptocurrencyId = _cryptocurrencyId,
            OwnerId = _ownerId,
            Port = _port,
            Tls = _tls,
        };
    }
}
