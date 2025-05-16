using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices;

/// <summary>
/// PCI.
/// </summary>
[ComplexType]
public record Pci
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// BUS.
    /// </summary>
    public required string Bus { get; init; }
}
