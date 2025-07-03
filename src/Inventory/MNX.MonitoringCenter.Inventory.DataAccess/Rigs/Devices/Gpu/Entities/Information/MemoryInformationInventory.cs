using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Information;

/// <summary>
/// Информация о памяти видеокарты.
/// </summary>
[Owned]
public class MemoryInformationInventory
{
    /// <summary>
    /// Общий размер.
    /// </summary>
    [Column("memory_total")]
    public int Total { get; init; }

    /// <summary>
    /// Тип.
    /// </summary>
    [Column("memory_type")]
    public string? Type { get; init; }

    /// <summary>
    /// Продавец.
    /// </summary>
    [Column("memory_vendor")]
    public string? Vendor { get; init; }
}
