namespace MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands;

/// <summary>
/// Входная модель пула
/// </summary>
public class PoolInputModel
{
    /// <summary>
    /// Признак шифрования по протоколу TLS.
    /// </summary>
    public bool Tls { get; set; }

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

    public PoolInputModel(bool tls, string domain, int port, Guid cryptocurrencyId)
    {
        Tls = tls;
        Domain = domain;
        Port = port;
        CryptocurrencyId = cryptocurrencyId;
    }
}
