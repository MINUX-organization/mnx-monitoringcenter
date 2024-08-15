namespace MNX.MonitoringCenter.Management.UseCases.Wallet.Commands;

/// <summary>
/// Входная модель кошелька
/// </summary>
public class WalletInputModel
{
    /// <summary>
    /// Название кошелька
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Адрес кошелька
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Идентификатор криптовалюты
    /// </summary>
    public Guid CryptocurrencyId { get; set; }

    public WalletInputModel(string name, string address, Guid cryptocurrencyId)
    {
        Name = name;
        Address = address;
        CryptocurrencyId = cryptocurrencyId;
    }
}
