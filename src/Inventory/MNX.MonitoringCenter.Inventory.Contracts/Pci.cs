using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts;

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
    public string Bus { get; init; }
}
