namespace MNX.MonitoringCenter.Management.UseCases.Cryptocurrency.Commands;

/// <summary>
/// Входная модель крипты.
/// </summary>
public class CryptocurrencyInputModel
{
    /// <summary>
    /// Короткое название
    /// </summary>
    public string ShortName { get; }

    /// <summary>
    /// Полное название
    /// </summary>
    public string FullName { get; }

    /// <summary>
    /// Алгоритм
    /// </summary>
    public string Algorithm { get; }

    public CryptocurrencyInputModel(string shortName, string fullName, string algorithm)
    {
        ShortName = shortName;
        FullName = fullName;
        Algorithm = algorithm;
    }
}
