using MNX.MonitoringCenter.Inventory.Contracts.Gpu.Information;
using MNX.MonitoringCenter.Inventory.Contracts.Gpu.Restrictions;

namespace MNX.MonitoringCenter.Inventory.Contracts.Gpu;

/// <summary>
/// Видеокарта.
/// </summary>
public class Gpu
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// PCI.
    /// </summary>
    public Pci Pci { get; set; }

    /// <summary>
    /// Информация.
    /// </summary>
    public GpuInformation Information { get; set; }

    /// <summary>
    /// Ограничения.
    /// </summary>
    public GpuRestrictions Restrictions { get; set; }
}
