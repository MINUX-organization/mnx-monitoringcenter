using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Information;
using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Information;

/// <summary>
/// Технология параллельного вычисления.
/// </summary>
[Owned]
public record ParallelComputingTechnologyInventory
{
    /// <summary>
    /// Тип.
    /// </summary>
    [Column("technology_type")]
    public ParallelComputingTechnologyEnum Type { get; init; }

    /// <summary>
    /// Версия.
    /// </summary>
    [Column("technology_version")]
    public string? Version { get; init; }
}
