namespace MNX.MonitoringCenter.Management.Contracts;

/// <summary>
/// Модель для пула
/// </summary>
public class PoolModel
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Домен
    /// </summary>
    public required string Domain { get; set; }

    /// <summary>
    /// Порт
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Полное название криптовалюты
    /// </summary>
    public required string Cryptocurrency { get; set; }
}
