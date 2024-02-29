namespace MNX.MonitoringCenter.Management.UseCases.Commands.Wallets;

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
    /// Полное название криптовалюты
    /// </summary>
    public string CryptocurrencyFullName { get; set; }

    public WalletInputModel(string name, string address, string cryptocurrencyFullName)
    {
        Name = name;
        Address = address;
        CryptocurrencyFullName = cryptocurrencyFullName;
    }
}
