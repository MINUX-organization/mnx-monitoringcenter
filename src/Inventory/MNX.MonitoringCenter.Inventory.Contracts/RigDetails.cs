using MNX.MonitoringCenter.Inventory.Contracts.Devices.CountDevices;

namespace MNX.MonitoringCenter.Inventory.Contracts;

/// <summary>
/// Детали рига.
/// </summary>
public class RigDetails
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор владельца.
    /// </summary>
    public Guid OwnerId { get; init; }

    /// <summary>
    /// Название рига.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Mac адрес.
    /// </summary>
    public string? Mac {  get; init; }

    /// <summary>
    /// Глобальный IP адрес.
    /// </summary>
    public string? GlobalIP { get; init; }

    /// <summary>
    /// Локальный IP адрес.
    /// </summary>
    public string? LocalIP { get; init; }

    /// <summary>
    /// Программное обеспечение.
    /// </summary>
    public required SoftwareInventory Software { get; init; }

    /// <summary>
    /// Кол-во устройств.
    /// </summary>
    public required ModelWithCountDevices CountDevices { get; init; }
}
