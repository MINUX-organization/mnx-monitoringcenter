namespace MNX.MonitoringCenter.Management.UseCases.Commands.Pools;

/// <summary>
/// Входная модель пула
/// </summary>
public class PoolInputModel
{
    /// <summary>
    /// Домен
    /// </summary>
    public string Domain { get; }

    /// <summary>
    /// Порт
    /// </summary>
    public int Port { get; }

    /// <summary>
    /// Полное название криптовалюты
    /// </summary>
    public string CryptocurrencyFullName { get; }

    public PoolInputModel(string domain, int port, string cryptocurrencyFullName)
    {
        Domain = domain;
        Port = port;
        CryptocurrencyFullName = cryptocurrencyFullName;
    }
}
