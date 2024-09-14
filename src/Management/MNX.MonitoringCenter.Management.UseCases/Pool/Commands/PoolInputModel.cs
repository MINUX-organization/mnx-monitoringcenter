namespace MNX.MonitoringCenter.Management.UseCases.Pool.Commands;

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
    /// Идентификатор криптовалюты
    /// </summary>
    public Guid CryptocurrencyId { get; }

    public PoolInputModel(string domain, int port, Guid cryptocurrencyId)
    {
        Domain = domain;
        Port = port;
        CryptocurrencyId = cryptocurrencyId;
    }
}
