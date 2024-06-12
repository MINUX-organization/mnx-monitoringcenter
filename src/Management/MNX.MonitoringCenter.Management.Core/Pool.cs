namespace MNX.MonitoringCenter.Management.Core;

/// <summary>
/// Пул
/// </summary>
public class Pool
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; set; }   

    /// <summary>
    /// Домен
    /// </summary>
    public string Domain {  get; set; }

    /// <summary>
    /// Порт
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Идентификатор криптовалюты
    /// </summary>
    public Guid CryptocurrencyId { get; set; }

    /// <summary>
    /// Криптовалюта
    /// </summary>
    public Cryptocurrency? Cryptocurrency { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public long UserId { get; set; }
}