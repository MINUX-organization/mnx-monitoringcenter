namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

using Cryptocurrency = Core.Mining.Cryptocurrency;
using Wallet = Core.Mining.Wallet;

public class WalletBuilder
{
    private static int _counter = 1;

    protected Guid _id = Guid.NewGuid();
    protected string _address = $"WalletAddress_{_counter}";
    protected string _name = $"WalletName_{_counter++}";
    protected Guid _ownerId = Guid.NewGuid();
    protected Guid _cryptocurrencyId = Guid.NewGuid();
    protected Cryptocurrency? _cryptocurrency = null;

    public WalletBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public WalletBuilder WithAddress(string address)
    {
        _address = address;
        return this;
    }

    public WalletBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public WalletBuilder WithOwnerId(Guid ownerId)
    {
        _ownerId = ownerId;
        return this;
    }

    public WalletBuilder WithCryptocurrency(Func<CryptocurrencyBuilder, CryptocurrencyBuilder> configure)
    {
        var builder = new CryptocurrencyBuilder();
        builder = configure(builder);
        _cryptocurrency = builder.Build();
        _cryptocurrencyId = _cryptocurrency.Id;
        return this;
    }

    public Wallet Build()
    {
        return new Wallet
        {
            Id = _id,
            Address = _address,
            Name = _name,
            OwnerId = _ownerId,
            CryptocurrencyId = _cryptocurrencyId,
            Cryptocurrency = _cryptocurrency,
        };
    }
}
