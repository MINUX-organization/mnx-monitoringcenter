namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.NetworkAdapter;

/// <summary>
/// Сетевой адаптер.
/// </summary>
public record NetworkAdapter
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Глобальный IP-адрес.
    /// </summary>
    public string? GlobalIP { get; init; }

    /// <summary>
    /// Локальный IP-адрес.
    /// </summary>
    public string? LocalIP { get; init; }

    /// <summary>
    /// Информация.
    /// </summary>
    public required NetworkAdapterInformation Information { get; init; }

    /// <summary>
    /// Получить признак того, что адаптер подключен к сети Интернет.
    /// </summary>
    /// <returns>
    /// <see langword="true"/>, если подключен, иначе <see langword="false"/>.
    /// </returns>
    public bool IsOnline { get => GlobalIP != null; }
}
