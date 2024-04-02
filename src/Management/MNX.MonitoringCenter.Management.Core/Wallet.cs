namespace MNX.MonitoringCenter.Management.Core;

/// <summary>
/// Кошулёк
/// </summary>
public class Wallet
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Адрес
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Идентификатор криптовалюты
    /// </summary>
    public Guid CryptocurrencyId { get; set; }

    /// <summary>
    /// Криптовалюта
    /// </summary>
    public Cryptocurrency? Cryptocurrency { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }
}