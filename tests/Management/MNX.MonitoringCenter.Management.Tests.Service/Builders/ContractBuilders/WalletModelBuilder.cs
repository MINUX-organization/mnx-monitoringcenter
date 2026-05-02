using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;

public class WalletModelBuilder
{
    private static int _counter = 1;
    
    protected Guid _id = Guid.NewGuid();
    protected string _name = $"WalletModelName_{_counter}";
    protected string _address = $"WalletModelAddress_{_counter}";
    protected Guid _cryptocurrencyId = Guid.NewGuid();
    protected string _cryptocurrency = $"CryptocurrencyName_{_counter++}";

    public WalletModelBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public WalletModelBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public WalletModelBuilder WithAddress(string address)
    {
        _address = address;
        return this;
    }
    public WalletModelBuilder WithCryptocurrencyId(Guid cryptocurrencyId)
    {
        _cryptocurrencyId = cryptocurrencyId;
        return this;
    }

    public WalletModelBuilder WithCryptocurrencyName(string cryptocurrencyName)
    {
        _cryptocurrency = cryptocurrencyName;
        return this;
    }

    public WalletModel Build()
    {
        return new WalletModel
        {
            Id = _id,
            Name = _name,
            Address = _address,
            CryptocurrencyId = _cryptocurrencyId,
            Cryptocurrency = _cryptocurrency,
        };
    }
}
