namespace MNX.MonitoringCenter.Management.Contracts;

/// <summary>
/// Модель криптовалюты
/// </summary>
public sealed class CryptocurrencyModel
{
    /// <summary>
    /// Короткое название
    /// </summary>
    public string ShortName { get; set; }

    /// <summary>
    /// Полное название
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Алгоритма
    /// </summary>
    public string Algorithm { get; set; }
}