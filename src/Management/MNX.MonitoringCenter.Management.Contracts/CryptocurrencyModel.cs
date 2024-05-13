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
    public string ShortName { get; set; }

    /// <summary>
    /// Полное название
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Алгоритм
    /// </summary>
    public string Algorithm { get; set; }
}
