namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.CoreBuilders;

using Cryptocurrency = Core.Mining.Cryptocurrency;
using Wallet = Core.Mining.Wallet;

public class WalletBuilder
{
    private static int _counter = 1;

    private Guid _id = Guid.NewGuid();
    private string _address = $"WalletAddress_{_counter}";
    private string _name = $"WalletName_{_counter++}";
    private Guid _ownerId = Guid.NewGuid();
    private Guid _cryptocurrencyId = Guid.NewGuid();
    private Cryptocurrency? _cryptocurrency = null;

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

    public WalletBuilder WithCryptocurrency(Action<CryptocurrencyBuilder>? configure = null)
    {
        var builder = new CryptocurrencyBuilder();
        configure?.Invoke(builder);
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
