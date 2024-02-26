namespace MNX.MonitoringCenter.Management.UseCases.Commands.Wallets;

/// <summary>
/// Модель кошелька
/// </summary>
public class WalletModel
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

    public WalletModel(string name, string address, string cryptocurrencyFullName)
    {
        Name = name;
        Address = address;
        CryptocurrencyFullName = cryptocurrencyFullName;
    }
}
