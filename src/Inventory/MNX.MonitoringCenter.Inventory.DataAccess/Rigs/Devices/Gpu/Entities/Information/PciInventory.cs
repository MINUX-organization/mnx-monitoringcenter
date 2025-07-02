using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Information;

/// <summary>
/// Инвентаризация Pci-слотов устройств.
/// </summary>
[Owned]
public class PciInventory
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    [Column("pci_id")]
    public int Id { get; init; }

    /// <summary>
    /// BUS.
    /// </summary>
    [Column("pci_bus")]
    public required string Bus { get; init; }
}
