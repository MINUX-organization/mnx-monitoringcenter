using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency.Commands;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;

public class CryptocurrencyInputModelBuilder
{
    private static int _counter = 1;

    protected string _fullName = $"CryptocurrencyInputFullName_{_counter}";
    protected string _shortName = $"CryptoInShortName_{_counter++}";
    protected Guid _algorithmId = Guid.NewGuid();

    public CryptocurrencyInputModelBuilder WithFullName(string fullName)
    {
        _fullName = fullName;
        return this;
    }
    
    public CryptocurrencyInputModelBuilder WithShortName(string shortName)
    {
        _shortName = shortName;
        return this;
    }

    public CryptocurrencyInputModelBuilder WithAlgorithmId(Guid algorithmId)
    {
        _algorithmId = algorithmId;
        return this;
    }

    public CryptocurrencyInputModel Build() =>
        new(_fullName, _shortName, _algorithmId);
}
