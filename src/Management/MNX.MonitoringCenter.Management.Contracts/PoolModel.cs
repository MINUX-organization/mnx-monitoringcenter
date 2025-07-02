namespace MNX.MonitoringCenter.Management.Contracts;

/// <summary>
/// Модель для пула.
/// </summary>
public class PoolModel
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    /// <remarks>
    /// Если null, то сущность является доменной,
    /// иначе - пользовательской.
    /// </remarks>
    public Guid? OwnerId { get; set; }

    /// <summary>
    /// Признак шифрования по протоколу TLS.
    /// </summary>
    public bool Tls { get; set; }

    /// <summary>
    /// Домен.
    /// </summary>
    public required string Domain { get; set; }

    /// <summary>
    /// Порт.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Идентификатор криптовалюты.
    /// </summary>
    public Guid CryptocurrencyId { get; set; }

    /// <summary>
    /// Полное название криптовалюты.
    /// </summary>
    public required string Cryptocurrency { get; set; }
}
