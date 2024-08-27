using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Gpu.Information;

/// <summary>
/// Информация о памяти.
/// </summary>
[ComplexType]
public sealed record MemoryInformation
{
    /// <summary>
    /// Всего.
    /// </summary>
    public int Total {  get; set; }

    /// <summary>
    /// Тип.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Продавец.
    /// </summary>
    public string Vendor { get; set; }
}
