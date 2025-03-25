namespace MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings.Models;

/// <summary>
/// Модель конфигурации майнинга криптовалюты.
/// </summary>
public class MiningCoinConfigModel
{
    /// <summary>
    /// Адрес кошелька.
    /// </summary>
    public required string WalletAddress { get; set; }

    /// <summary>
    /// Имя алгоритма (то, которое присвоил целевой майнер).
    /// </summary>
    public required string AlgorithmName { get; set; }

    /// <summary>
    /// Хост пула.
    /// </summary>
    public required string PoolHost { get; set; }

    /// <summary>
    /// Порт пула.
    /// </summary>
    public required int PoolPort { get; set; }

    /// <summary>
    /// Признак шифрования по протоколу Tls.
    /// </summary>
    public required bool Tls { get; set; }

    /// <summary>
    /// Пароль пула.
    /// </summary>
    public string? PoolPassword { get; set; }
}
