namespace MINUX.Backend.Worker.UseCases.Commands.Pools;

/// <summary>
/// Модель пула
/// </summary>
public class PoolModel
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

    public PoolModel(string domain, int port, string cryptocurrencyFullName)
    {
        Domain = domain;
        Port = port;
        CryptocurrencyFullName = cryptocurrencyFullName;
    }
}
