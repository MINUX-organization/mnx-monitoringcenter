using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Information;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Overclocking;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Restrictions;
using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Views;

using AmdGpuOverclockingInventory = AmdGpuOverclockingInventory;
using Gpu = Contracts.Devices.Gpu.Gpu;
using NvidiaGpuOverclockingInventory = NvidiaGpuOverclockingInventory;

/// <summary>
/// Представление сущности <see cref="Gpu"/>.
/// </summary>
public record GpuInventoryView
{
    /// <summary>
    /// Наименование рига.
    /// </summary>
    [Column("rig_name")]
    public required string RigName { get; init; }

    /// <summary>
    /// Версия драйвера.
    /// </summary>
    [Column("driver_version")]
    public string? DriverVersion { get; init; }

    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    [Column("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор инвентаризации рига.
    /// </summary>
    [Column("rig_inventory_id")]
    public int RigInventoryId { get; init; }

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
