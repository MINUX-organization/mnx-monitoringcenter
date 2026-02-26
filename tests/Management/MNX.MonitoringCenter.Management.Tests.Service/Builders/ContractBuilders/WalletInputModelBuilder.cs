using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;

public class WalletInputModelBuilder
{
    private static int _counter = 1;

    private string _name = $"WalletInputModelName_{_counter}";
    private Guid _cryptocurrencyId = Guid.NewGuid();
    private string _address = $"WalletInputModelAddress{_counter++}";

    public WalletInputModelBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public WalletInputModelBuilder WithAddress(string address)
    { 
        _address = address;
        return this;
    }

    public WalletInputModelBuilder WithCryptocurrencyId(Guid cryptocurrencyId)
    {
        _cryptocurrencyId = cryptocurrencyId;
        return this;
    }

    public WalletInputModel Build() =>
        new(_name, _address, _cryptocurrencyId);
}
