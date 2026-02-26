using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;

public class PoolInputModelBuilder
{
    private static int _counter = 1;

    private bool _tls = false;
    private string _domain = $"www.pool-input-domain-{_counter}";
    private int _port = 8080 + _counter++;
    private Guid _cryptocurrencyId = Guid.NewGuid();

    public PoolInputModelBuilder WithTls(bool tls = true)
    {
        _tls = tls;
        return this;
    }

    public PoolInputModelBuilder WithDomain(string domain)
    {
        _domain = domain;
        return this;
    }

    public PoolInputModelBuilder WithPort(int port)
    {
        _port = port;
        return this;
    }

    public PoolInputModelBuilder WithCryptocurrencyId(Guid cryptocurrencyId)
    {
        _cryptocurrencyId = cryptocurrencyId;
        return this;
    }

    public PoolInputModel Build() =>
        new(_tls, _domain, _port, _cryptocurrencyId);
}
