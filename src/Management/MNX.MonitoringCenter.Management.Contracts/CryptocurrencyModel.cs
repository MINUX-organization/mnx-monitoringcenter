namespace MNX.MonitoringCenter.Management.Contracts;

/// <summary>
/// Модель для криптовалюты.
/// </summary>
public class CryptocurrencyModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Короткое название
    /// </summary>
    public required string ShortName { get; set; }

    /// <summary>
    /// Полное название
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Алгоритм
    /// </summary>
    public required string Algorithm { get; set; }
}
