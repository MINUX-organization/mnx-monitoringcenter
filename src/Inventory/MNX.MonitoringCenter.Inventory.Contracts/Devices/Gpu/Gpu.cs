using MNX.MonitoringCenter.Inventory.Contracts.Devices;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Information;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;

/// <summary>
/// Видеокарта.
/// </summary>
public record Gpu
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// PCI.
    /// </summary>
    public required Pci Pci { get; init; }

    /// <summary>
    /// Информация.
    /// </summary>
    public required GpuInformation Information { get; init; }

    /// <summary>
    /// Ограничения.
    /// </summary>
    public required GpuRestrictions Restrictions { get; init; }
}
