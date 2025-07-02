using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Information;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Overclocking;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Restrictions;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities;

/// <summary>
/// Сущность видеокарты для базы данных.
/// </summary>
public class GpuInventory
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор инвентаризации рига.
    /// </summary>
    public long RigInventoryId { get; init; }

    /// <summary>
    /// Слот Pci видеокарты.
    /// </summary>
    public required PciInventory Pci { get; init; }

    /// <summary>
    /// Информация видеокарты.
    /// </summary>
    public required GpuInformationInventory Information { get; init; }

    /// <summary>
    /// Ограничения видеокарт Nvidia.
    /// </summary>
    public required NvidiaGpuRestrictionsInventory NvidiaRestrictions { get; init; }

    /// <summary>
    /// Ограничения видеокарт Amd.
    /// </summary>
    public required AmdGpuRestrictionsInventory AmdRestrictions { get; init; }

    /// <summary>
    /// Ограничения видеокарт Intel.
    /// </summary>
    public required IntelGpuRestrictionsInventory IntelRestrictions { get; init; }

    /// <summary>
    /// Разгон видеокарт Nvidia.
    /// </summary>
    public required NvidiaGpuOverclockingInventory NvidiaOverclocking { get; init; }

    /// <summary>
    /// Разгон видеокарт Amd.
    /// </summary>
    public required AmdGpuOverclockingInventory AmdOverclocking { get; init; }

    /// <summary>
    /// Разгон видеокарт Intel.
    /// </summary>
    public required IntelGpuOverclockingInventory IntelOverclocking { get; init; }
}