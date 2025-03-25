namespace MNX.MonitoringCenter.Management.Contracts;

/// <summary>
/// Модель для кошелька
/// </summary>
public class WalletModel
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Адрес
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Идентификатор криптовалюты.
    /// </summary>
    public Guid CryptocurrencyId { get; set; }

    /// <summary>
    /// Полное название криптовалюты
    /// </summary>
    public required string Cryptocurrency { get; set; }
}
