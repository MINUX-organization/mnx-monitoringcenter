namespace MNX.MonitoringCenter.Inventory.Contracts.NetworkAdapter;

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
    public required string GlobalIP { get; init; }

    /// <summary>
    /// Локальный IP-адрес.
    /// </summary>
    public required string LocalIP { get; init; }

    /// <summary>
    /// Информация.
    /// </summary>
    public required NetworkAdapterInformation Information { get; init; }
}
