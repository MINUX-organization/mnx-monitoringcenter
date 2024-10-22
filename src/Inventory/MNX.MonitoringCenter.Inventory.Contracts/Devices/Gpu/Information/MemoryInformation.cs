using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Information;

/// <summary>
/// Информация о памяти.
/// </summary>
[ComplexType]
public record MemoryInformation
{
    /// <summary>
    /// Всего.
    /// </summary>
    public int Total { get; init; }

    /// <summary>
    /// Тип.
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Продавец.
    /// </summary>
    public required string Vendor { get; init; }
}
