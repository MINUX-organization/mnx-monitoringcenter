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
    /// Пароль
    /// </summary>
    public string Password { get; }

    /// <summary>
    /// Идентификатор криптовалюты
    /// </summary>
    public Guid CryptocurrencyId { get; }

    public PoolInputModel(string domain, int port, string password, Guid cryptocurrencyId)
    {
        Domain = domain;
        Port = port;
        Password = password;
        CryptocurrencyId = cryptocurrencyId;
    }
}
